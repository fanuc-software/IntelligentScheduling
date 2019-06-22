using System;
using System.Threading;
using System.Threading.Tasks;

namespace LeftMaterialService
{

    public abstract class BaseLeftMaterialService
    {
        public abstract IControlDevice ControlDevice { get; }
        public bool Temp_S_House_RequestFCS_Last { get; set; }

        CancellationTokenSource token = new CancellationTokenSource();

        public BaseLeftMaterialService()
        {

        }

        public void Start()
        {

            Task.Factory.StartNew(async () =>
            {

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
                            new LeftMaterialServiceState { State = LeftMaterialServiceStateEnum.ERROR, Message = "初始化失败,发送错误信息至设备!" });
                        Thread.Sleep(1000);
                    }


                    bool dev_reset = false;
                    while (dev_reset == false)
                    {
                        ControlDevice.GetHouseFCSReset(ref dev_reset);
                        SendLeftMaterialServiceStateMessage(
                            new LeftMaterialServiceState { State = LeftMaterialServiceStateEnum.INFO, Message = "初始化失败,等待设备的复位信号" });
                        Thread.Sleep(1000);
                    }
                }
                Temp_S_House_RequestFCS_Last = temp_S_House_RequestFCS_Last;

                #endregion

                while (!token.IsCancellationRequested)
                {
                    //料库请求
                    ret = await LeftMaterialFlow();
                    if (ret == false)
                    {
                        while (ret == false)
                        {
                            ret = ControlDevice.SetHouseFCSAlarm(true);
                            SendLeftMaterialServiceStateMessage(
                                new LeftMaterialServiceState { State = LeftMaterialServiceStateEnum.ERROR, Message = "左侧料库请求或者调用失败,发送错误信息至设备!" });
                            Thread.Sleep(1000);
                        }


                        bool dev_reset = false;
                        while (dev_reset == false)
                        {
                            ControlDevice.GetHouseFCSReset(ref dev_reset);
                            SendLeftMaterialServiceStateMessage(
                                new LeftMaterialServiceState { State = LeftMaterialServiceStateEnum.INFO, Message = "左侧料库请求或者调用失败,等待设备的复位信号" });
                            Thread.Sleep(1000);
                        }

                        //复位所有信号
                        //ControlDevice.ControlDeviceReset();
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

        }

        private async Task<bool> LeftMaterialFlow()
        {
            bool ret = false;

            bool S_HouseRequestFCS = false;
            ret = ControlDevice.GetHouseRequestFCS(ref S_HouseRequestFCS);
            if (ret != true) return ret;

            if (S_HouseRequestFCS == true)
            {
                Temp_S_House_RequestFCS_Last = S_HouseRequestFCS;

                //防错处理
                ret = CheckOnBegin();
                if (ret != true)
                {

                    return ret;
                }

                //料库通讯
                bool S_House_InOut = false;
                var ret_inout = ControlDevice.GetHouseInOut(ref S_House_InOut);
                if (ret_inout == false)
                {
                    return ret_inout;
                }

                if (S_House_InOut == true)
                {
                    var ret_out = await LeftMaterialOutFlow();
                    if (ret_out == false)
                    {
                        return ret_out;
                    }
                }
                else
                {
                    var ret_in = await LeftMaterialInFlow();
                    if (ret_in == false)
                    {
                        return ret_in;
                    }
                }

            }
            else
            {
                Temp_S_House_RequestFCS_Last = S_HouseRequestFCS;
            }


            return true;

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

        private async Task<bool> LeftMaterialOutFlow()
        {
            return await Task.Factory.StartNew(() =>
            {
                IWareHouseClient WareHouse = new LefModulaWareHouseClient("LEFT_MATERIAL_OUT");

                int S_House_ProductType = 0;
                var ret_prod_type = ControlDevice.GetHouseProductType(ref S_House_ProductType);
                if (ret_prod_type == false)
              {
                  return ret_prod_type;
              }

                int S_House_MaterialType = 0;
                var ret_material_type = ControlDevice.GetHouseMaterialType(ref S_House_MaterialType);
                if (ret_material_type == false)
              {
                  return ret_material_type;
              }

                var ret_moveout = WareHouse.MoveOutTray(S_House_ProductType, S_House_MaterialType);
                if (ret_moveout == false)
              {
                  return ret_moveout;
              }

                bool S_House_TrayInposition = false;
                while (S_House_TrayInposition == false)
                {

                    var ret_inposition_info = ControlDevice.GetHouseTrayInposition(ref S_House_TrayInposition);
                    if (ret_inposition_info == false)
                    {
                        return ret_inposition_info;
                    }

                    var reset = false;
                    ControlDevice.GetHouseFCSReset(ref reset);
                    if (reset == true)
                    {
                        return false;
                    }
                }

                Thread.Sleep(500);

                int S_House_ProductPosition = 0;
                int S_House_TrayPosition = 0;
                var ret_warehouse_info = WareHouse.GetPositionInfo(S_House_ProductType, S_House_MaterialType,
                    out S_House_ProductPosition, out S_House_TrayPosition);
                if (ret_warehouse_info == false)
                {
                    return ret_warehouse_info;
                }


                var ret_warehouse_product_position = ControlDevice.SetHouseProductPostion(S_House_ProductPosition);
                if (ret_warehouse_product_position == false)
                {
                    return ret_warehouse_product_position;
                }

                var ret_warehouse_tray_position = ControlDevice.SetHouseTrayPostion(S_House_TrayPosition);
                if (ret_warehouse_tray_position == false)
                {
                    return ret_warehouse_tray_position;
                }

                var ret_confirm_materialtype_position = ControlDevice.SetHouseConfirmMaterialType(S_House_MaterialType);
                if (ret_confirm_materialtype_position == false)
              {
                  return ret_confirm_materialtype_position;
              }

                var ret_req_fin = ControlDevice.SetHouseRequestFCSFin(true);
                if (ret_req_fin == false)
                {
                    return ret_req_fin;
                }

                bool S_House_RequestInfoFCS = false;

                while (S_House_RequestInfoFCS == false)
                {

                    var ret_req_info = ControlDevice.GetHouseRequestInfoFCS(ref S_House_RequestInfoFCS);
                    if (ret_req_info == false)
                  {
                      return ret_req_info;
                  }

                    var reset = false;
                    ControlDevice.GetHouseFCSReset(ref reset);
                    if (reset == true)
                  {
                      return false;
                  }
                }

                var ret_data_input = WareHouse.WriteBackData(S_House_ProductType, S_House_MaterialType, true);
                if (ret_data_input == false)
              {
                  return false;
              }

                var ret_info_fin = ControlDevice.SetHouseRequestInfoFCSFin(true);
                if (ret_info_fin == false)
              {
                  return false;
              }

                return true;
            });


        }

        private async Task<bool> LeftMaterialInFlow()
        {
            return await Task.Factory.StartNew(() =>
            {
                IWareHouseClient WareHouse = new LefModulaWareHouseClient("LEFT_MATERIAL_IN");

                int S_House_ProductType = 0;
                var ret_prod_type = ControlDevice.GetHouseProductType(ref S_House_ProductType);
                if (ret_prod_type == false)
                {
                    return ret_prod_type;
                }

                int S_House_MaterialType = 0;
                var ret_material_type = ControlDevice.GetHouseMaterialType(ref S_House_MaterialType);
                if (ret_material_type == false)
                {
                    return ret_material_type;
                }

                var ret_movein = WareHouse.MoveInTray(S_House_ProductType, S_House_MaterialType);
                if (ret_movein == false)
                {
                    return ret_movein;
                }

                int S_House_ProductPosition = 0;
                int S_House_TrayPosition = 0;
                var ret_warehouse_info = WareHouse.GetPositionInfo(S_House_ProductType, S_House_MaterialType,
                    out S_House_ProductPosition, out S_House_TrayPosition);
                if (ret_warehouse_info == false)
                {
                    return ret_warehouse_info;
                }

                var ret_warehouse_product_position = ControlDevice.SetHouseProductPostion(S_House_ProductPosition);
                if (ret_warehouse_product_position == false)
                {
                    return ret_warehouse_product_position;
                }

                var ret_warehouse_tray_position = ControlDevice.SetHouseTrayPostion(S_House_TrayPosition);
                if (ret_warehouse_tray_position == false)
                {
                    return ret_warehouse_tray_position;
                }

                var ret_confirm_materialtype_position = ControlDevice.SetHouseConfirmMaterialType(S_House_MaterialType);
                if (ret_confirm_materialtype_position == false)
                {
                    return ret_confirm_materialtype_position;
                }

                var ret_req_fin = ControlDevice.SetHouseRequestFCSFin(true);
                if (ret_req_fin == false)
                {
                    return ret_req_fin;
                }

                bool S_House_RequestInfoFCS = false;

                while (S_House_RequestInfoFCS == false)
                {

                    var ret_req_info = ControlDevice.GetHouseRequestInfoFCS(ref S_House_RequestInfoFCS);
                    if (ret_req_info == false)
                    {
                        return ret_req_info;
                    }

                    var reset = false;
                    ControlDevice.GetHouseFCSReset(ref reset);
                    if (reset == true)
                    {
                        return false;
                    }
                }

                var ret_data_input = WareHouse.WriteBackData(S_House_ProductType, S_House_MaterialType, false);
                if (ret_data_input == false)
                {
                    return false;
                }

                var ret_info_fin = ControlDevice.SetHouseRequestInfoFCSFin(true);
                if (ret_info_fin == false)
                {
                    return false;
                }

                return true;
            });
        }
    }
}
