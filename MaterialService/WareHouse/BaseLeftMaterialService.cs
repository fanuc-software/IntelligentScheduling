using System;
using System.Threading;
using System.Threading.Tasks;

namespace LeftMaterialService
{

    public abstract class BaseLeftMaterialService
    {
        public abstract IControlDevice ControlDevice { get; }
        public bool Temp_S_House_RequestFCS_Last { get; set; }

        private LeftMaterialServiceErrorCodeEnum cur_Display_ErrorCode;
        private string cur_Display_Message;

        CancellationTokenSource token = new CancellationTokenSource();

        public event Action<LeftMaterialServiceState> SendLeftMaterialServiceStateMessageEvent;

        public BaseLeftMaterialService()
        {

        }

        public void Start()
        {

            Task.Factory.StartNew(async () =>
            {
                SendLeftMaterialServiceStateMessage(
                    new LeftMaterialServiceState { State = LeftMaterialServiceStateEnum.INFO, Message = "START！", ErrorCode = LeftMaterialServiceErrorCodeEnum.NORMAL });


                Temp_S_House_RequestFCS_Last = false;

                bool ret = false;

                #region 初始化
                bool temp_S_House_RequestFCS_Last = false;
                ret = ControlDevice.GetHouseRequestFCS(ref temp_S_House_RequestFCS_Last);
                if (ret == false)
                {

                    while (ret == false)
                    {
                        ret = ControlDevice.SetHouseFCSAlarm(true);
                        SendLeftMaterialServiceStateMessage(
                            new LeftMaterialServiceState { State = LeftMaterialServiceStateEnum.ERROR, Message = "初始化连接,请检查网络连接与配置后复位!",ErrorCode=LeftMaterialServiceErrorCodeEnum.INI_GETREQ });
                        Thread.Sleep(1000);
                    }


                    bool dev_reset = false;
                    while (dev_reset == false)
                    {
                        ControlDevice.GetHouseFCSReset(ref dev_reset);
                        SendLeftMaterialServiceStateMessage(
                            new LeftMaterialServiceState { State = LeftMaterialServiceStateEnum.WARN, Message = "初始化失败,等待设备的复位信号",ErrorCode=LeftMaterialServiceErrorCodeEnum.NORMAL });
                        Thread.Sleep(1000);
                    }
                }
                Temp_S_House_RequestFCS_Last = temp_S_House_RequestFCS_Last;

                #endregion

                while (!token.IsCancellationRequested)
                {
                    //料库请求
                    var ret_tuple = await LeftMaterialFlow();
                    if (ret_tuple.Item1 == false)
                    {
                        ret = false;
                        while (ret == false)
                        {
                            ret = ControlDevice.SetHouseFCSAlarm(true);
                            SendLeftMaterialServiceStateMessage(
                                new LeftMaterialServiceState { State = LeftMaterialServiceStateEnum.ERROR, Message = "左侧料库请求调用失败,请检查后复位!", ErrorCode = ret_tuple.Item2 });
                            Thread.Sleep(1000);
                        }
                        
                        bool dev_reset = false;
                        while (dev_reset == false)
                        {
                            ControlDevice.GetHouseFCSReset(ref dev_reset);
                            SendLeftMaterialServiceStateMessage(
                                new LeftMaterialServiceState { State = LeftMaterialServiceStateEnum.WARN, Message = "左侧料库请求调用失败,等待设备的复位信号",ErrorCode= LeftMaterialServiceErrorCodeEnum.NORMAL });
                            Thread.Sleep(1000);
                        }

                        SendLeftMaterialServiceStateMessage(
                            new LeftMaterialServiceState { State = LeftMaterialServiceStateEnum.WARN, Message = "左侧料库请求调用失败,设备复位", ErrorCode = LeftMaterialServiceErrorCodeEnum.NORMAL });
                    }
                    else
                    {

                    }

                }
            }, token.Token);
        }

        //TODO
        public void Stop()
        {

        }

        //TODO
        public void Suspend()
        {

        }

        //TODO
        private void SendLeftMaterialServiceStateMessage(LeftMaterialServiceState state)
        {
            if(cur_Display_ErrorCode!= state.ErrorCode || state.Message != cur_Display_Message)
            {
                SendLeftMaterialServiceStateMessageEvent?.Invoke(state);
                
                cur_Display_ErrorCode = state.ErrorCode;
                cur_Display_Message = state.Message;
            }
        }

        private async Task<Tuple<bool,LeftMaterialServiceErrorCodeEnum>> LeftMaterialFlow()
        {
            bool ret = false;

            bool S_HouseRequestFCS = false;
            ret = ControlDevice.GetHouseRequestFCS(ref S_HouseRequestFCS);
            if (ret != true)
            {
                return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(false, LeftMaterialServiceErrorCodeEnum.GETREQ);
            }

            if (S_HouseRequestFCS == true)
            {
                Temp_S_House_RequestFCS_Last = S_HouseRequestFCS;

                //防错处理
                ret = CheckOnBegin();
                if (ret != true)
                {
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(false, LeftMaterialServiceErrorCodeEnum.CHECKONBEGIN);
                }

                //料库通讯
                bool S_House_InOut = false;
                var ret_inout = ControlDevice.GetHouseInOut(ref S_House_InOut);
                if (ret_inout == false)
                {
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_inout, LeftMaterialServiceErrorCodeEnum.INOUT);
                }

                if (S_House_InOut == true)
                {
                    var ret_out = await LeftMaterialOutFlow();
                    if (ret_out.Item1 == false)
                    {
                        return ret_out;
                    }
                }
                else
                {
                    var ret_in = await LeftMaterialInFlow();
                    if (ret_in.Item1 == false)
                    {
                        return ret_in;
                    }
                }

                SendLeftMaterialServiceStateMessage(
                    new LeftMaterialServiceState { State = LeftMaterialServiceStateEnum.INFO, Message = "左侧料库请求调用完成！", ErrorCode = LeftMaterialServiceErrorCodeEnum.NORMAL });

            }
            else
            {
                Temp_S_House_RequestFCS_Last = S_HouseRequestFCS;
            }


            return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(true, LeftMaterialServiceErrorCodeEnum.NORMAL);

        }

        private bool CheckOnBegin()
        {
            bool ret = false;
            
            ret = ControlDevice.SetHouseFCSAlarm(false);
            if (ret != true) return ret;

            ret = ControlDevice.SetHouseFCSReset(false);
            if (ret != true) return ret;

            ret = ControlDevice.SetHouseRequestFCSFin(false);
            if (ret != true) return ret;

            ret = ControlDevice.SetHouseRequestInfoFCSFin(false);
            if (ret != true) return ret;
            
            return true;
        }

        private async Task<Tuple<bool, LeftMaterialServiceErrorCodeEnum>> LeftMaterialOutFlow()
        {
            return await Task.Factory.StartNew(() =>
            {
                IWareHouseClient WareHouse = new LefModulaWareHouseClient("LEFT_MATERIAL_OUT");

                int S_House_ProductType = 0;
                var ret_prod_type = ControlDevice.GetHouseProductType(ref S_House_ProductType);
                if (ret_prod_type == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_prod_type, LeftMaterialServiceErrorCodeEnum.OUT_GETPRODUCTTYPE);
                }

                int S_House_MaterialType = 0;
                var ret_material_type = ControlDevice.GetHouseMaterialType(ref S_House_MaterialType);
                if (ret_material_type == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_material_type, LeftMaterialServiceErrorCodeEnum.OUT_GETMATERIALTYPE);
                }

                var ret_moveout = WareHouse.MoveOutTray(S_House_ProductType, S_House_MaterialType);
                if (ret_moveout == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_moveout, LeftMaterialServiceErrorCodeEnum.OUT_MOVEOUTTRAY);
                }

                bool S_House_TrayInposition = false;
                while (S_House_TrayInposition == false)
                {

                    var ret_inposition_info = ControlDevice.GetHouseTrayInposition(ref S_House_TrayInposition);
                    if (ret_inposition_info == false)
                    {
                        WareHouse.ReleaseWriterLock();
                        return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_inposition_info, LeftMaterialServiceErrorCodeEnum.OUT_GETTRAYINPOSITION);
                    }

                    SendLeftMaterialServiceStateMessage(
                        new LeftMaterialServiceState { State = LeftMaterialServiceStateEnum.WARN,
                            Message = "等待料库托盘到位",
                            ErrorCode = LeftMaterialServiceErrorCodeEnum.NORMAL });

                    var ret_reset = false;
                    ControlDevice.GetHouseFCSReset(ref ret_reset);
                    if (ret_reset == true)
                    {
                        WareHouse.ReleaseWriterLock();
                        return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_reset, LeftMaterialServiceErrorCodeEnum.OUT_TRAYINPOSITIONRESET);
                    }
                }

                Thread.Sleep(500);

                int S_House_ProductPosition = 0;
                int S_House_TrayPosition = 0;
                int S_House_Quantity = 0;
                var ret_warehouse_info = WareHouse.GetPositionInfo(S_House_ProductType, S_House_MaterialType,
                    out S_House_ProductPosition, out S_House_TrayPosition, out S_House_Quantity);
                if (ret_warehouse_info == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_warehouse_info, LeftMaterialServiceErrorCodeEnum.OUT_GETTRAYPOSITION);
                }


                var ret_warehouse_product_position = ControlDevice.SetHouseProductPostion(S_House_ProductPosition);
                if (ret_warehouse_product_position == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_warehouse_product_position, LeftMaterialServiceErrorCodeEnum.OUT_SETPRODUCTPOS);
                }

                var ret_warehouse_tray_position = ControlDevice.SetHouseTrayPostion(S_House_TrayPosition);
                if (ret_warehouse_tray_position == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_warehouse_tray_position, LeftMaterialServiceErrorCodeEnum.OUT_SETTRAYPOS);
                }

                var ret_warehouse_quantity= ControlDevice.SetHouseQuantity(S_House_Quantity);
                if (ret_warehouse_quantity == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_warehouse_quantity, LeftMaterialServiceErrorCodeEnum.OUT_SETQUANTITY);
                }

                var ret_confirm_materialtype_position = ControlDevice.SetHouseConfirmMaterialType(S_House_MaterialType);
                if (ret_confirm_materialtype_position == false)
              {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_confirm_materialtype_position, LeftMaterialServiceErrorCodeEnum.OUT_SETMATERIALTYPECONFIRM);
                }

                var ret_req_fin = ControlDevice.SetHouseRequestFCSFin(true);
                if (ret_req_fin == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_req_fin, LeftMaterialServiceErrorCodeEnum.OUT_SETREQUESTFIN);
                }

                bool S_House_RequestInfoFCS = false;

                while (S_House_RequestInfoFCS == false)
                {
                    var ret_req_info = ControlDevice.GetHouseRequestInfoFCS(ref S_House_RequestInfoFCS);
                    if (ret_req_info == false)
                  {
                        WareHouse.ReleaseWriterLock();
                        return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_req_info, LeftMaterialServiceErrorCodeEnum.OUT_GETREQINFO);
                  }

                    SendLeftMaterialServiceStateMessage(
                        new LeftMaterialServiceState
                        {
                            State = LeftMaterialServiceStateEnum.WARN,
                            Message = "等待料库动作完成",
                            ErrorCode = LeftMaterialServiceErrorCodeEnum.NORMAL
                        });

                    var ret_reset = false;
                    ControlDevice.GetHouseFCSReset(ref ret_reset);
                    if (ret_reset == true)
                    {
                        WareHouse.ReleaseWriterLock();
                        return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_reset, LeftMaterialServiceErrorCodeEnum.OUT_REQINFORESET);
                    }
                }

                var ret_data_input = WareHouse.WriteBackData(S_House_ProductType, S_House_MaterialType, true);
                if (ret_data_input == false)
              {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_data_input, LeftMaterialServiceErrorCodeEnum.OUT_WRITEDATA);
              }

                var ret_info_fin = ControlDevice.SetHouseRequestInfoFCSFin(true);
                if (ret_info_fin == false)
              {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_info_fin, LeftMaterialServiceErrorCodeEnum.OUT_SETREQINFOFIN);
              }

                WareHouse.ReleaseWriterLock();
                return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(true, LeftMaterialServiceErrorCodeEnum.NORMAL); ;
            });


        }

        private async Task<Tuple<bool, LeftMaterialServiceErrorCodeEnum>> LeftMaterialInFlow()
        {
            return await Task.Factory.StartNew(() =>
            {
                IWareHouseClient WareHouse = new LefModulaWareHouseClient("LEFT_MATERIAL_IN");

                int S_House_ProductType = 0;
                var ret_prod_type = ControlDevice.GetHouseProductType(ref S_House_ProductType);
                if (ret_prod_type == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_prod_type, LeftMaterialServiceErrorCodeEnum.IN_GETPRODUCTTYPE);
                }

                int S_House_MaterialType = 0;
                var ret_material_type = ControlDevice.GetHouseMaterialType(ref S_House_MaterialType);
                if (ret_material_type == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_material_type, LeftMaterialServiceErrorCodeEnum.IN_GETMATERIALTYPE); 
                }

                var ret_movein = WareHouse.MoveInTray(S_House_ProductType, S_House_MaterialType);
                if (ret_movein == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_movein, LeftMaterialServiceErrorCodeEnum.IN_MOVEINTRAY);
                }

                bool S_House_TrayInposition = false;
                while (S_House_TrayInposition == false)
                {

                    var ret_inposition_info = ControlDevice.GetHouseTrayInposition(ref S_House_TrayInposition);
                    if (ret_inposition_info == false)
                    {
                        WareHouse.ReleaseWriterLock();
                        return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_inposition_info, LeftMaterialServiceErrorCodeEnum.IN_GETTRAYINPOSITION);
                    }

                    SendLeftMaterialServiceStateMessage(
                        new LeftMaterialServiceState
                        {
                            State = LeftMaterialServiceStateEnum.WARN,
                            Message = "等待料库托盘到位",
                            ErrorCode = LeftMaterialServiceErrorCodeEnum.NORMAL
                        });

                    var ret_reset = false;
                    ControlDevice.GetHouseFCSReset(ref ret_reset);
                    if (ret_reset == true)
                    {
                        WareHouse.ReleaseWriterLock();
                        return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_reset, LeftMaterialServiceErrorCodeEnum.IN_TRAYINPOSITIONRESET);
                    }
                }

                Thread.Sleep(500);

                int S_House_ProductPosition = 0;
                int S_House_TrayPosition = 0;
                int S_House_Quantity = 0;

                var ret_warehouse_info = WareHouse.GetPositionInfo(S_House_ProductType, S_House_MaterialType,
                    out S_House_ProductPosition, out S_House_TrayPosition, out S_House_Quantity);
                if (ret_warehouse_info == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_warehouse_info, LeftMaterialServiceErrorCodeEnum.IN_GETTRAYPOSITION);
                }

                var ret_warehouse_product_position = ControlDevice.SetHouseProductPostion(S_House_ProductPosition);
                if (ret_warehouse_product_position == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_warehouse_product_position, LeftMaterialServiceErrorCodeEnum.IN_SETPRODUCTPOS);
                }

                var ret_warehouse_tray_position = ControlDevice.SetHouseTrayPostion(S_House_TrayPosition);
                if (ret_warehouse_tray_position == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_warehouse_tray_position, LeftMaterialServiceErrorCodeEnum.IN_SETTRAYPOS);
                }

                var ret_warehouse_quantity = ControlDevice.SetHouseQuantity(S_House_Quantity);
                if (ret_warehouse_quantity == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_warehouse_quantity, LeftMaterialServiceErrorCodeEnum.IN_SETQUANTITY);
                }

                var ret_confirm_materialtype_position = ControlDevice.SetHouseConfirmMaterialType(S_House_MaterialType);
                if (ret_confirm_materialtype_position == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_confirm_materialtype_position, LeftMaterialServiceErrorCodeEnum.IN_SETMATERIALTYPECONFIRM);
                }

                var ret_req_fin = ControlDevice.SetHouseRequestFCSFin(true);
                if (ret_req_fin == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_req_fin, LeftMaterialServiceErrorCodeEnum.IN_SETREQUESTFIN);
                }

                bool S_House_RequestInfoFCS = false;

                while (S_House_RequestInfoFCS == false)
                {

                    var ret_req_info = ControlDevice.GetHouseRequestInfoFCS(ref S_House_RequestInfoFCS);
                    if (ret_req_info == false)
                    {
                        WareHouse.ReleaseWriterLock();
                        return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_req_info, LeftMaterialServiceErrorCodeEnum.IN_GETREQINFO);
                    }

                    SendLeftMaterialServiceStateMessage(
                        new LeftMaterialServiceState
                        {
                            State = LeftMaterialServiceStateEnum.WARN,
                            Message = "等待料库动作完成",
                            ErrorCode = LeftMaterialServiceErrorCodeEnum.NORMAL
                        });

                    var reset = false;
                    ControlDevice.GetHouseFCSReset(ref reset);
                    if (reset == true)
                    {
                        WareHouse.ReleaseWriterLock();
                        return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_req_info, LeftMaterialServiceErrorCodeEnum.IN_REQINFORESET);
                    }
                }

                var ret_data_input = WareHouse.WriteBackData(S_House_ProductType, S_House_MaterialType, false);
                if (ret_data_input == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_data_input, LeftMaterialServiceErrorCodeEnum.IN_WRITEDATA);
                }

                var ret_info_fin = ControlDevice.SetHouseRequestInfoFCSFin(true);
                if (ret_info_fin == false)
                {
                    WareHouse.ReleaseWriterLock();
                    return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(ret_info_fin, LeftMaterialServiceErrorCodeEnum.IN_SETREQINFOFIN);
                }

                WareHouse.ReleaseWriterLock();
                return new Tuple<bool, LeftMaterialServiceErrorCodeEnum>(true, LeftMaterialServiceErrorCodeEnum.NORMAL);
            });
        }
    }
}
