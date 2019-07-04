using Agv.Common;
using AgvMissionManager;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AgvStationClient
{
    public abstract class BaseStationClient
    {
        private AgvStationEnum stationId;
        private BaseAgvMissionService materialSrv;

        public abstract IStationDevice stationDevice { get; set; }

        CancellationTokenSource token = new CancellationTokenSource();
        SignalrService signalrService;
        AutoResetEvent resetEvent = new AutoResetEvent(false);

        public BaseStationClient(AgvStationEnum id)
        {
            stationId = id;

            signalrService = new SignalrService("http://localhost/Agv", "AgvMissonHub");
            signalrService.OnMessage<AgvOutMisson>(AgvReceiveActionEnum.receiveOutMissionFinMessage.EnumToString(), (s) => {
                OnAgvOutMissonEvent(s);
            });
            signalrService.OnMessage<AgvInMisson>(AgvReceiveActionEnum.receiveInMissionFinMessage.EnumToString(), (s) => {
                OnAgvInMissonEvent(s);
            });
        }

        private void OnAgvInMissonEvent(AgvInMisson mission)
        {
            //毛坯空箱回库
            if (mission.Id.Equals(stationId + "_EMPTYOUT"))
            {
                if (mission.Process >= AgvInMissonProcessEnum.AGVPICKEDANDLEAVE)
                {
                    bool empty_out = false;
                    var ret_empty_out = stationDevice.GetEmptyOutState(ref empty_out);
                    if(ret_empty_out==true && empty_out==true)
                    {
                        stationDevice.SetEmptyOutFin(true);
                    }
                    
                }
            }
            //成品回库
            if (mission.Id.Equals(stationId + "_FINOUT"))
            {
                if (mission.Process >= AgvInMissonProcessEnum.AGVPICKEDANDLEAVE)
                {
                    bool fin_out = false;
                    var ret_fin_out = stationDevice.GetFinOutState(ref fin_out);
                    if (ret_fin_out == true && fin_out == true)
                    {
                        stationDevice.SetFinOutFin(true);
                    }
                    
                }
            }
        }

        private void OnAgvOutMissonEvent(AgvOutMisson mission)
        {
            //毛坯输入
            if (mission.Id.Equals(stationId + "_RAWIN"))
            {
                if (mission.Process >= AgvOutMissonProcessEnum.FINISHED)
                {
                    bool raw_in = false;
                    var ret_raw_in = stationDevice.GetRawInRequireState(ref raw_in);
                    if (ret_raw_in == true && raw_in == true)
                    {
                        stationDevice.SetRawInFin(true);
                    }
                }
            }
            //成品空箱输入
            if (mission.Id.Equals(stationId + "_EMPTYIN"))
            {
                if (mission.Process >= AgvOutMissonProcessEnum.FINISHED)
                {
                    bool empty_in = false;
                    var ret_empty_in = stationDevice.GetEmptyInState(ref empty_in);
                    if (ret_empty_in == true && empty_in == true)
                    {
                        stationDevice.SetEmptyInFin(true);
                    }
                }
            }
        }

        public void Start()
        {
            Task.Factory.StartNew(async() =>
            {
                await signalrService.Start();

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

                    SendFeedingSignal();
                }
            }, token.Token);
            

        }

        private void SendFeedingSignal()
        {
            var empty_in = false;
            var ret_empty_in = stationDevice.GetEmptyInFeedingSignal(ref empty_in);
            if (ret_empty_in == true)
            {
                signalrService.Send(AgvSendActionEnum.SendFeedingSignalMessage.EnumToString(),
                    new AgvFeedingSignal
                    {
                        Id = stationId + "_EMPTYIN",
                        ClientId = stationId,
                        Type = AgvMissionTypeEnum.EMPTY_IN,
                        Value = empty_in,
                    }).Wait();
            }

            var raw_in = false;
            var ret_raw_in = stationDevice.GetRawInFeedingSignal(ref raw_in);
            if(ret_raw_in==true)
            {
                signalrService.Send(AgvSendActionEnum.SendFeedingSignalMessage.EnumToString(),
                    new AgvFeedingSignal
                    {
                        Id = stationId + "_RAWIN",
                        ClientId = stationId,
                        Type = AgvMissionTypeEnum.RAW_IN,
                        Value = raw_in,
                    }).Wait();
            }
        }

        private bool StationClientFlow()
        {
            //毛坯输入
            var raw_in = false;
            var ret = stationDevice.GetRawInRequireState(ref raw_in);
            if (ret = true && raw_in == true)
            {
                //毛坯空箱回库
                {
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

                        SendInMission(new AgvInMisson
                        {
                            Id = stationId + "_EMPTYOUT",
                            TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                            ClientId = stationId,
                            Type = AgvMissionTypeEnum.EMPTY_OUT,
                            PickStationId = stationId,
                            PlaceStationId = AgvStationEnum.WareHouse,
                            Process = AgvInMissonProcessEnum.NEW,
                            Quantity = 0,
                            MaterialId = material_type,
                            ProductId = prod_type,
                            CreateDateTime = DateTime.Now,
                        });
                    }
                }

                //毛坯输入
                {
                    string prod_type = "";
                    var ret_product_type = stationDevice.GetRawInProductType(ref prod_type);

                    string material_type = "";
                    var ret_material_type = stationDevice.GetRawInMaterialType(ref material_type);

                    if (ret_product_type == false || ret_material_type == false)
                    {
                        return false;
                    }

                    var ret_rawin_fin = stationDevice.SetRawInFin(false);
                    if (ret_rawin_fin == false)
                    {
                        return false;
                    }

                    SendOutMission(new AgvOutMisson
                    {
                        Id = stationId + "_RAWIN",
                        TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                        ClientId = stationId,
                        Type = AgvMissionTypeEnum.RAW_IN,
                        PickStationId = AgvStationEnum.WareHouse,
                        PlaceStationId = stationId,
                        Process = AgvOutMissonProcessEnum.NEW,
                        Quantity = 0,
                        MaterialId = material_type,
                        ProductId = prod_type,
                        CreateDateTime = DateTime.Now,
                    });
                }

            }

            //成品空箱输入
            var empty_in = false;
            ret = stationDevice.GetEmptyInState(ref empty_in);
            if (ret = true && empty_in == true)
            {
                //成品回库
                {
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

                        SendInMission(new AgvInMisson
                        {
                            Id = stationId + "_FINOUT",
                            TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                            ClientId = stationId,
                            Type = AgvMissionTypeEnum.FIN_OUT,
                            PickStationId = stationId,
                            PlaceStationId = AgvStationEnum.WareHouse,
                            Process = AgvInMissonProcessEnum.NEW,
                            Quantity = 0,
                            MaterialId = material_type,
                            ProductId = prod_type,
                        });
                    }
                }

                //成品空箱输入
                {
                    string prod_type = "";
                    var ret_product_type = stationDevice.GetEmptyInProductType(ref prod_type);

                    string material_type = "";
                    var ret_material_type = stationDevice.GetEmptyInMaterialType(ref material_type);
                    if (ret_product_type == false || ret_material_type == false)
                    {
                        return false;
                    }

                    var ret_emptyin_fin = stationDevice.SetEmptyInFin(false);
                    if (ret_emptyin_fin == false)
                    {
                        return false;
                    }

                    SendOutMission(new AgvOutMisson
                    {
                        Id = stationId + "_EMPTYIN",
                        TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                        ClientId = stationId,
                        Type = AgvMissionTypeEnum.EMPTY_IN,
                        PickStationId = AgvStationEnum.WareHouse,
                        PlaceStationId = stationId,
                        Process = AgvOutMissonProcessEnum.NEW,
                        Quantity = 0,
                        MaterialId = material_type,
                        ProductId = prod_type,
                    });

                }

            }

            //毛坯空箱回库
            {
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

                    SendInMission(new AgvInMisson
                    {
                        Id = stationId + "_EMPTYOUT",
                        TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                        ClientId = stationId,
                        Type = AgvMissionTypeEnum.EMPTY_OUT,
                        PickStationId = stationId,
                        PlaceStationId = AgvStationEnum.WareHouse,
                        Process = AgvInMissonProcessEnum.NEW,
                        Quantity = 0,
                        MaterialId = material_type,
                        ProductId = prod_type,
                        CreateDateTime = DateTime.Now,
                    });
                }
            }

            //成品回库
            {
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

                    SendInMission(new AgvInMisson
                    {
                        Id = stationId + "_FINOUT",
                        TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                        ClientId = stationId,
                        Type = AgvMissionTypeEnum.FIN_OUT,
                        PickStationId = stationId,
                        PlaceStationId = AgvStationEnum.WareHouse,
                        Process = AgvInMissonProcessEnum.NEW,
                        Quantity = 0,
                        MaterialId = material_type,
                        ProductId = prod_type,
                    });
                }
            }

            return true;
        }

        private async void SendOutMission(AgvOutMisson mission)
        {
            await signalrService.Send(AgvSendActionEnum.SendOutMission.EnumToString(), mission);
        }

        private async void SendInMission(AgvInMisson mission)
        {
            await signalrService.Send(AgvSendActionEnum.SendInMission.EnumToString(), mission);
        }

        private void SendBrotherMission(AgvInMisson inmission, AgvOutMisson outmission)
        {

        }

        private void SendStationClientStateMessage(StationClientState state)
        {

        }
    }
}
