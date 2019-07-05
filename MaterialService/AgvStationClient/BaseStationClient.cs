using Agv.Common;
using AgvMissionManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AgvStationClient
{
    public abstract class BaseStationClient<T> where T : IStationDevice
    {
        public AgvStationEnum Station_Id { get; set; }
        
        private BaseAgvMissionService materialSrv;

        private StationClientStateEnum lastState;
        private string lastMessage;
        private List<StationClientState> lastInfos = new List<StationClientState>();
        
        public T StationDevice { get; }
        CancellationTokenSource token = new CancellationTokenSource();
        SignalrService signalrService;
        AutoResetEvent resetEvent = new AutoResetEvent(false);
        public event Action<StationClientState> SendStationClientStateMessageEvent;

        public BaseStationClient(AgvStationEnum id,T device)
        {
            StationDevice = device;
            Station_Id = id;

            signalrService = new SignalrService("http://localhost/Agv", "AgvMissonHub");
            signalrService.OnMessage<AgvOutMisson>(AgvReceiveActionEnum.receiveOutMissionFinMessage.EnumToString(), (s) =>
            {
                OnAgvOutMissonEvent(s);
            });
            signalrService.OnMessage<AgvInMisson>(AgvReceiveActionEnum.receiveInMissionFinMessage.EnumToString(), (s) =>
            {
                OnAgvInMissonEvent(s);
            });
        }

        private void OnAgvInMissonEvent(AgvInMisson mission)
        {
            //毛坯空箱回库
            if (mission.Id.Equals(Station_Id + "_EMPTYOUT"))
            {
                if (mission.Process == AgvInMissonProcessEnum.AGVPICKEDANDLEAVE)
                {
                    bool empty_out = false;
                    var ret_empty_out = StationDevice.GetEmptyOutState(ref empty_out);
                    if (ret_empty_out == true && empty_out == true)
                    {
                        StationDevice.SetEmptyOutFin(true);
                    }

                }
            }
            //成品回库
            if (mission.Id.Equals(Station_Id + "_FINOUT"))
            {
                if (mission.Process == AgvInMissonProcessEnum.AGVPICKEDANDLEAVE)
                {
                    bool fin_out = false;
                    var ret_fin_out = StationDevice.GetFinOutState(ref fin_out);
                    if (ret_fin_out == true && fin_out == true)
                    {
                        StationDevice.SetFinOutFin(true);
                    }

                }
            }
        }

        private void OnAgvOutMissonEvent(AgvOutMisson mission)
        {
            //毛坯输入
            if (mission.Id.Equals(Station_Id + "_RAWIN"))
            {
                if (mission.Process == AgvOutMissonProcessEnum.FINISHED)
                {
                    bool raw_in = false;
                    var ret_raw_in = StationDevice.GetRawInRequireState(ref raw_in);
                    if (ret_raw_in == true && raw_in == true)
                    {
                        StationDevice.SetRawInFin(true);
                    }
                }
            }
            //成品空箱输入
            if (mission.Id.Equals(Station_Id + "_EMPTYIN"))
            {
                if (mission.Process == AgvOutMissonProcessEnum.FINISHED)
                {
                    bool empty_in = false;
                    var ret_empty_in = StationDevice.GetEmptyInState(ref empty_in);
                    if (ret_empty_in == true && empty_in == true)
                    {
                        StationDevice.SetEmptyInFin(true);
                    }
                }
            }
        }

        public void Start()
        {
            SendStationClientStateMessage(
                new StationClientState { State = StationClientStateEnum.INFO, Message = "单元站点客户端开启!" ,CreateDateTime=DateTime.Now});

            Task.Factory.StartNew(async () =>
            {
                await signalrService.Start();

                while (!token.IsCancellationRequested)
                {
                    var ret = StationClientFlow();
                    if (ret == false)
                    {
                        while (ret == false)
                        {
                            ret = StationDevice.SetAlarm(true);
                            SendStationClientStateMessage(
                                new StationClientState { State = StationClientStateEnum.ERROR, Message = "物料调用失败,发送错误信息至设备!", CreateDateTime = DateTime.Now });
                            Thread.Sleep(1000);
                        }

                        bool dev_reset = false;
                        while (dev_reset == false)
                        {
                            StationDevice.GetReset(ref dev_reset);
                            SendStationClientStateMessage(
                                new StationClientState { State = StationClientStateEnum.INFO, Message = "物料调用失败,等待设备的复位信号", CreateDateTime = DateTime.Now });
                            Thread.Sleep(1000);
                        }
                    }

                    SendFeedingSignal();

                    Thread.Sleep(1000);
                }
            }, token.Token);


        }

        private void SendFeedingSignal()
        {
            var empty_in = false;
            var ret_empty_in = StationDevice.GetEmptyInFeedingSignal(ref empty_in);
            if (ret_empty_in == true)
            {
                var dd = signalrService.Send(AgvSendActionEnum.SendFeedingSignalMessage.EnumToString(),
                      new AgvFeedingSignal
                      {
                          Id = Station_Id + "_EMPTYIN",
                          ClientId = Station_Id,
                          Type = AgvMissionTypeEnum.EMPTY_IN,
                          Value = empty_in,
                      }).Result;
            }

            var raw_in = false;
            var ret_raw_in = StationDevice.GetRawInFeedingSignal(ref raw_in);
            if (ret_raw_in == true)
            {
                var ff = signalrService.Send(AgvSendActionEnum.SendFeedingSignalMessage.EnumToString(),
                     new AgvFeedingSignal
                     {
                         Id = Station_Id + "_RAWIN",
                         ClientId = Station_Id,
                         Type = AgvMissionTypeEnum.RAW_IN,
                         Value = raw_in,
                     }).Result;
            }
        }

        private bool StationClientFlow()
        {
            //毛坯输入
            var raw_in = false;
            var ret = StationDevice.GetRawInRequireState(ref raw_in);
            if (ret = true && raw_in == true)
            {
                //毛坯空箱回库
                {
                    var empty_out = false;
                    ret = StationDevice.GetEmptyOutState(ref empty_out);
                    if (ret = true && empty_out == true)
                    {
                        string prod_type = "";
                        var ret_product_type = StationDevice.GetEmptyInProductType(ref prod_type);

                        string material_type = "";
                        var ret_material_type = StationDevice.GetEmptyInMaterialType(ref material_type);

                        if (ret_product_type == false || ret_material_type == false)
                        {
                            return false;
                        }

                        var ret_emptyout_fin = StationDevice.SetEmptyOutFin(false);
                        if (ret_emptyout_fin == false)
                        {
                            return false;
                        }

                        SendInMission(new AgvInMisson
                        {
                            Id = Station_Id + "_EMPTYOUT",
                            TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                            ClientId = Station_Id,
                            Type = AgvMissionTypeEnum.EMPTY_OUT,
                            PickStationId = Station_Id,
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
                    var ret_product_type = StationDevice.GetRawInProductType(ref prod_type);

                    string material_type = "";
                    var ret_material_type = StationDevice.GetRawInMaterialType(ref material_type);

                    if (ret_product_type == false || ret_material_type == false)
                    {
                        return false;
                    }

                    var ret_rawin_fin = StationDevice.SetRawInFin(false);
                    if (ret_rawin_fin == false)
                    {
                        return false;
                    }

                    SendOutMission(new AgvOutMisson
                    {
                        Id = Station_Id + "_RAWIN",
                        TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                        ClientId = Station_Id,
                        Type = AgvMissionTypeEnum.RAW_IN,
                        PickStationId = AgvStationEnum.WareHouse,
                        PlaceStationId = Station_Id,
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
            ret = StationDevice.GetEmptyInState(ref empty_in);
            if (ret = true && empty_in == true)
            {
                //成品回库
                {
                    var fin_out = false;
                    ret = StationDevice.GetFinOutState(ref fin_out);
                    if (ret = true && fin_out == true)
                    {
                        string prod_type = "";
                        var ret_product_type = StationDevice.GetFinOutProductType(ref prod_type);

                        string material_type = "";
                        var ret_material_type = StationDevice.GetFinOutMaterialType(ref material_type);

                        if (ret_product_type == false || ret_material_type == false)
                        {
                            return false;
                        }

                        var ret_finout_fin = StationDevice.SetFinOutFin(false);
                        if (ret_finout_fin == false)
                        {
                            return false;
                        }

                        SendInMission(new AgvInMisson
                        {
                            Id = Station_Id + "_FINOUT",
                            TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                            ClientId = Station_Id,
                            Type = AgvMissionTypeEnum.FIN_OUT,
                            PickStationId = Station_Id,
                            PlaceStationId = AgvStationEnum.WareHouse,
                            Process = AgvInMissonProcessEnum.NEW,
                            Quantity = 0,
                            MaterialId = material_type,
                            ProductId = prod_type,
                            CreateDateTime = DateTime.Now,
                        });
                    }
                }

                //成品空箱输入
                {
                    string prod_type = "";
                    var ret_product_type = StationDevice.GetEmptyInProductType(ref prod_type);

                    string material_type = "";
                    var ret_material_type = StationDevice.GetEmptyInMaterialType(ref material_type);
                    if (ret_product_type == false || ret_material_type == false)
                    {
                        return false;
                    }

                    var ret_emptyin_fin = StationDevice.SetEmptyInFin(false);
                    if (ret_emptyin_fin == false)
                    {
                        return false;
                    }

                    SendOutMission(new AgvOutMisson
                    {
                        Id = Station_Id + "_EMPTYIN",
                        TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                        ClientId = Station_Id,
                        Type = AgvMissionTypeEnum.EMPTY_IN,
                        PickStationId = AgvStationEnum.WareHouse,
                        PlaceStationId = Station_Id,
                        Process = AgvOutMissonProcessEnum.NEW,
                        Quantity = 0,
                        MaterialId = material_type,
                        ProductId = prod_type,
                        CreateDateTime = DateTime.Now,
                    });

                }

            }

            //毛坯空箱回库
            {
                var empty_out = false;
                ret = StationDevice.GetEmptyOutState(ref empty_out);
                if (ret = true && empty_out == true)
                {
                    string prod_type = "";
                    var ret_product_type = StationDevice.GetEmptyInProductType(ref prod_type);

                    string material_type = "";
                    var ret_material_type = StationDevice.GetEmptyInMaterialType(ref material_type);

                    if (ret_product_type == false || ret_material_type == false)
                    {
                        return false;
                    }

                    var ret_emptyout_fin = StationDevice.SetEmptyOutFin(false);
                    if (ret_emptyout_fin == false)
                    {
                        return false;
                    }

                    SendInMission(new AgvInMisson
                    {
                        Id = Station_Id + "_EMPTYOUT",
                        TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                        ClientId = Station_Id,
                        Type = AgvMissionTypeEnum.EMPTY_OUT,
                        PickStationId = Station_Id,
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
                ret = StationDevice.GetFinOutState(ref fin_out);
                if (ret = true && fin_out == true)
                {
                    string prod_type = "";
                    var ret_product_type = StationDevice.GetFinOutProductType(ref prod_type);

                    string material_type = "";
                    var ret_material_type = StationDevice.GetFinOutMaterialType(ref material_type);

                    if (ret_product_type == false || ret_material_type == false)
                    {
                        return false;
                    }

                    var ret_finout_fin = StationDevice.SetFinOutFin(false);
                    if (ret_finout_fin == false)
                    {
                        return false;
                    }

                    SendInMission(new AgvInMisson
                    {
                        Id = Station_Id + "_FINOUT",
                        TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                        ClientId = Station_Id,
                        Type = AgvMissionTypeEnum.FIN_OUT,
                        PickStationId = Station_Id,
                        PlaceStationId = AgvStationEnum.WareHouse,
                        Process = AgvInMissonProcessEnum.NEW,
                        Quantity = 0,
                        MaterialId = material_type,
                        ProductId = prod_type,
                        CreateDateTime = DateTime.Now,
                    });
                }
            }

            return true;
        }

        private async void SendOutMission(AgvOutMisson mission)
        {
                SendStationClientStateMessage(
                    new StationClientState { State = StationClientStateEnum.INFO, Message = "出库请求:" + mission.Type.EnumToString(), CreateDateTime = DateTime.Now });

                await signalrService.Send(AgvSendActionEnum.SendOutMission.EnumToString(), mission);
        }

        private async void SendInMission(AgvInMisson mission)
        {
            SendStationClientStateMessage(
                new StationClientState { State = StationClientStateEnum.INFO, Message = "入库请求:" + mission.Type.EnumToString(), CreateDateTime = DateTime.Now });

            await signalrService.Send(AgvSendActionEnum.SendInMission.EnumToString(), mission);
        }

        private void SendStationClientStateMessage(StationClientState state)
        {
            if (lastMessage != state.Message || lastState != state.State)
            {
                if(state.State==StationClientStateEnum.INFO)
                {
                    var lastInfo = lastInfos.Where(x => x.State == state.State && x.Message == state.Message && x.CreateDateTime > state.CreateDateTime.AddSeconds(-2)).FirstOrDefault();
                    if (lastInfo != null) lastInfo.CreateDateTime = state.CreateDateTime;
                    else
                    {
                        lastInfos.Add(state);

                        SendStationClientStateMessageEvent?.Invoke(new StationClientState
                        {
                            State = state.State,
                            Message = state.Message,
                        });
                    }
                }
                else
                {
                    SendStationClientStateMessageEvent?.Invoke(new StationClientState
                    {
                        State = state.State,
                        Message = state.Message,
                    });
                }
                
                lastMessage = state.Message;
                lastState = state.State;
            }
        }
    }
}
