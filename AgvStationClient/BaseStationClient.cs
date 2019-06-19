using RightMaterialService;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AgvStationClient
{
    public abstract class BaseStationClient
    {
        private string stationId;

        public abstract IStationDevice stationDevice { get; set; }

        CancellationTokenSource token = new CancellationTokenSource();

        public BaseStationClient(string id)
        {
            stationId = id;
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

                    SendOutMission(new RightMaterialOutMisson
                    {
                        Id = stationId,
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

                    SendOutMission(new RightMaterialOutMisson
                    {
                        Id = stationId,
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

                SendInMission(new RightMaterialInMisson
                {
                    Id = stationId,
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

                SendInMission(new RightMaterialInMisson
                {
                    Id = stationId,
                    Type = RightMaterialMissionTypeEnum.EMPTY_OUT,
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
