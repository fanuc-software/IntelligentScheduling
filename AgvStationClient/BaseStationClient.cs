using RightMaterialService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AgvStationClient
{
    public abstract class BaseStationClient
    {
        private string stationId;
        private BaseRightMaterialService materialSrv;

        public abstract IStationDevice stationDevice { get; set; }
        
        CancellationTokenSource token = new CancellationTokenSource();

        public BaseStationClient(string id)
        {
            stationId = id;
            materialSrv = BaseRightMaterialService.CreateInstance();

            materialSrv.UpdateRightMaterialInMissonEvent += OnRightMaterialInMissonEvent;
            materialSrv.UpdateRightMaterialOutMissonEvent += OnRightMaterialOutMissonEvent;
        }

        private void OnRightMaterialInMissonEvent(RightMaterialInMisson mission)
        {
            //毛坯空箱回库
            if (mission.Id.Equals(stationId + "_EMPTYOUT"))
            {
                if(mission.Process>=RightMaterialInMissonProcessEnum.AGVLEAVEPICK)
                {
                    stationDevice.SetEmptyOutFin(true);
                }
            }
            //成品回库
            if (mission.Id.Equals(stationId + "_FINOUT"))
            {
                if (mission.Process >= RightMaterialInMissonProcessEnum.AGVLEAVEPICK)
                {
                    stationDevice.SetFinOutFin(true);
                }
            }
        }

        private void OnRightMaterialOutMissonEvent(RightMaterialOutMisson mission)
        {
            //毛坯输入
            if (mission.Id.Equals(stationId + "_RAWIN"))
            {
                if (mission.Process >= RightMaterialOutMissonProcessEnum.FINISHED)
                {
                    stationDevice.SetRawInFin(true);
                }
            }
            //成品空箱输入
            if (mission.Id.Equals(stationId + "_EMPTYIN"))
            {
                if (mission.Process >= RightMaterialOutMissonProcessEnum.FINISHED)
                {
                    stationDevice.SetEmptyInFin(true);
                }
            }
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {

                while (!token.IsCancellationRequested)
                {

                    var ret = StationClientFlow();
                    if (ret == false)
                    {
                        while (ret == false)
                        {
                            ret = stationDevice.SetAlarm(true);
                            SendStationClientStateMessage(
                                new StationClientState { State = StationClientStateEnum.ERROR, Message = "物料调用失败,发送错误信息至设备!" });
                            Thread.Sleep(1000);
                        }

                        bool dev_reset = false;
                        while (dev_reset == false)
                        {
                            stationDevice.GetReset(ref dev_reset);
                            SendStationClientStateMessage(
                                new StationClientState { State = StationClientStateEnum.INFO, Message = "物料调用失败,等待设备的复位信号" });
                            Thread.Sleep(1000);
                        }
                    }

                }
            }, token.Token);



        }
        
        private bool StationClientFlow()
        {
            //毛坯输入
            var raw_in = false;
            var ret = stationDevice.GetRawInRequireState(ref raw_in);
            if (ret = true && raw_in == true)
            {
                var raw_in_allow = false;
                ret = stationDevice.CheckRawInRequireAllow(ref raw_in_allow);
                if (ret == true && raw_in_allow == true)
                {
                    string prod_type = "";
                    var ret_product_type = stationDevice.GetRawInProductType(ref prod_type);

                    string material_type = "";
                    var ret_material_type = stationDevice.GetRawInMaterialType(ref material_type);

                    if(ret_product_type==false || ret_material_type==false)
                    {
                        return false;
                    }

                    var ret_rawin_fin = stationDevice.SetRawInFin(false);
                    if (ret_rawin_fin == false)
                    {
                        return false;
                    }

                    SendOutMission(new RightMaterialOutMisson
                    {
                        Id = stationId+"_RAWIN",
                        Type = RightMaterialMissionTypeEnum.RAW_IN,
                        PickStationId = null,
                        PlaceStationId = stationId,
                        Process = RightMaterialOutMissonProcessEnum.NEW,
                        Quantity = 0,
                        MaterialId = material_type,
                        ProductId = prod_type,
                    });
                }

            }

            //成品空箱输入
            var empty_in = false;
            ret = stationDevice.GetEmptyInState(ref empty_in);
            if (ret = true && empty_in == true)
            {
                var empty_in_allow = false;
                ret = stationDevice.CheckEmptyInAllow(ref empty_in_allow);
                if (ret == true && empty_in_allow == true)
                {
                    string prod_type = "";
                    var ret_product_type = stationDevice.GetEmptyInProductType(ref prod_type);

                    string material_type = "";
                    var ret_material_type = stationDevice.GetEmptyInMaterialType(ref material_type);
                    if (ret_product_type == false || ret_material_type == false)
                    {
                        return false;
                    }

                    var ret_emptyin_fin =  stationDevice.SetEmptyInFin(false);
                    if (ret_emptyin_fin == false)
                    {
                        return false;
                    }

                    SendOutMission(new RightMaterialOutMisson
                    {
                        Id = stationId+"_EMPTYIN",
                        Type = RightMaterialMissionTypeEnum.EMPTY_IN,
                        PickStationId = null,
                        PlaceStationId = stationId,
                        Process = RightMaterialOutMissonProcessEnum.NEW,
                        Quantity = 0,
                        MaterialId = material_type,
                        ProductId = prod_type,
                    });
                }

            }

            //毛坯空箱回库
            var empty_out = false;
            ret = stationDevice.GetEmptyOutState(ref empty_out);
            if (ret = true && empty_out == true)
            {
                string prod_type = "";
                var ret_product_type = stationDevice.GetEmptyInProductType(ref prod_type);

                string material_type = "";
                var ret_material_type = stationDevice.GetEmptyInMaterialType(ref material_type);

                if (ret_product_type == false || ret_material_type == false)
                {
                    return false;
                }

                var ret_emptyout_fin = stationDevice.SetEmptyOutFin(false);
                if (ret_emptyout_fin == false)
                {
                    return false;
                }

                SendInMission(new RightMaterialInMisson
                {
                    Id = stationId+"_EMPTYOUT",
                    Type = RightMaterialMissionTypeEnum.EMPTY_OUT,
                    PickStationId = stationId,
                    PlaceStationId = null,
                    Process = RightMaterialInMissonProcessEnum.NEW,
                    Quantity = 0,
                    MaterialId = material_type,
                    ProductId = prod_type,
                });
            }

            //成品回库
            var fin_out = false;
            ret = stationDevice.GetFinOutState(ref fin_out);
            if (ret = true && fin_out == true)
            {
                string prod_type = "";
                var ret_product_type = stationDevice.GetFinOutProductType(ref prod_type);

                string material_type = "";
                var ret_material_type = stationDevice.GetFinOutMaterialType(ref material_type);

                if (ret_product_type == false || ret_material_type == false)
                {
                    return false;
                }

                var ret_finout_fin = stationDevice.SetFinOutFin(false);
                if (ret_finout_fin == false)
                {
                    return false;
                }

                SendInMission(new RightMaterialInMisson
                {
                    Id = stationId+"_FINOUT",
                    Type = RightMaterialMissionTypeEnum.FIN_OUT,
                    PickStationId = stationId,
                    PlaceStationId = null,
                    Process = RightMaterialInMissonProcessEnum.NEW,
                    Quantity = 0,
                    MaterialId = material_type,
                    ProductId = prod_type,
                });
            }

            return true;
        }

        private void SendOutMission(RightMaterialOutMisson mission)
        {

        }
        
        private void SendInMission(RightMaterialInMisson mission)
        {

        }

        private void SendStationClientStateMessage(StationClientState state)
        {

        }
    }
}
