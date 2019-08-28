using Agv.Common;
using Agv.Common.Model;
using AgvStationClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AGV.Web.Service.Service
{
    public class StationProxyService : IDisposable
    {
        public AgvStationEnum Station_Id { private set; get; }
        public IStationDevice StationDevice { private set; get; }
        private bool last_rawin_state = false;
        private bool last_emptyout_state = false;
        private bool last_finout_state = false;
        private bool last_emptyin_state = false;
        public event Action<string, object> SendSingnalrEvent;

        public event Action<string> SendLogEvent;

        CancellationTokenSource token = new CancellationTokenSource();

        public StationProxyService(AgvStationEnum id, IStationDevice stationDevice)
        {
            Station_Id = id;
            this.StationDevice = stationDevice;
        }

        public void StartStationClentFlow()
        {
            while (!token.IsCancellationRequested)
            {

                var ret = StationClientFlow();
                if (ret == false)
                {
                    while (ret == false)
                    {
                        ret = StationDevice.SetAlarm(true);
                        SendLogEvent?.Invoke(
                            new StationClientState { State = StationClientStateEnum.ERROR, Message = "物料调用失败,发送错误信息至设备!" }.ToString());
                        Thread.Sleep(1000);
                    }

                    bool dev_reset = false;
                    while (dev_reset == false)
                    {
                        StationDevice.GetReset(ref dev_reset);
                        SendLogEvent?.Invoke(
                            new StationClientState { State = StationClientStateEnum.INFO, Message = "物料调用失败,等待设备的复位信号" }.ToString());
                        Thread.Sleep(1000);
                    }
                }

                Thread.Sleep(1000);
            }


        }

        public void FeedingSignalStart()
        {
            while (!token.IsCancellationRequested)
            {
                SendFeedingSignal();
                Thread.Sleep(1000);

            }


        }

        public void OnAgvInMissonEvent(AgvInMissonModel mission)
        {
            //毛坯空箱回库
            if (mission.Id.Equals(Station_Id + "_EMPTYOUT"))
            {
                if (mission.Process == AgvMissonProcessEnum.AGVPICKEDANDLEAVE && mission.Process != AgvMissonProcessEnum.CANCEL & mission.Process != AgvMissonProcessEnum.CANCELED)
                {
                    bool empty_out = false;
                    var ret_empty_out = StationDevice.GetEmptyOutState(ref empty_out);
                    if (ret_empty_out == true && empty_out == true)
                    {
                        StationDevice.SetEmptyOutFin(true);
                        bool empty_out_confirm = true;

                        while (empty_out_confirm)
                        {
                            ret_empty_out = StationDevice.GetEmptyOutState(ref empty_out_confirm);
                            if (ret_empty_out == true && empty_out_confirm == false)
                            {
                                StationDevice.SetEmptyOutFin(false);
                            }
                            Thread.Sleep(1000);
                        }
                    }

                }
            }
            //成品回库
            if (mission.Id.Equals(Station_Id + "_FINOUT"))
            {
                if (mission.Process == AgvMissonProcessEnum.AGVPICKEDANDLEAVE && mission.Process != AgvMissonProcessEnum.CANCEL & mission.Process != AgvMissonProcessEnum.CANCELED)
                {
                    bool fin_out = false;
                    var ret_fin_out = StationDevice.GetFinOutState(ref fin_out);
                    if (ret_fin_out == true && fin_out == true)
                    {
                        StationDevice.SetFinOutFin(true);

                        bool fin_out_confirm = true;

                        while (fin_out_confirm)
                        {
                            ret_fin_out = StationDevice.GetFinOutState(ref fin_out_confirm);
                            if (ret_fin_out == true && fin_out_confirm == false)
                            {
                                StationDevice.SetFinOutFin(false);
                            }
                            Thread.Sleep(1000);
                        }
                    }

                }
            }
        }

        public void OnAgvOutMissonEvent(AgvOutMissonModel mission)
        {
            //毛坯输入
            if (mission.Id.Equals(Station_Id + "_RAWIN"))
            {
                if ((mission.Process == AgvMissonProcessEnum.FINISHED || mission.Process == AgvMissonProcessEnum.CLOSE) && mission.Process != AgvMissonProcessEnum.CANCEL && mission.Process != AgvMissonProcessEnum.CANCELED)
                {
                    bool raw_in = false;
                    var ret_raw_in = StationDevice.GetRawInRequireState(ref raw_in);
                    if (ret_raw_in == true && raw_in == true)
                    {
                        Console.WriteLine("RAWIN_" + DateTime.Now);

                        StationDevice.SetRawInFin(true);
                        bool raw_in_confirm = true;

                        while (raw_in_confirm)
                        {
                            ret_raw_in = StationDevice.GetRawInRequireState(ref raw_in_confirm);
                            if (ret_raw_in == true && raw_in_confirm == false)
                            {
                                StationDevice.SetRawInFin(false);
                            }
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
            //成品空箱输入
            if (mission.Id.Equals(Station_Id + "_EMPTYIN"))
            {
                if ((mission.Process == AgvMissonProcessEnum.FINISHED || mission.Process == AgvMissonProcessEnum.CLOSE) && mission.Process != AgvMissonProcessEnum.CANCEL && mission.Process != AgvMissonProcessEnum.CANCELED)
                {
                    bool empty_in = false;
                    var ret_empty_in = StationDevice.GetEmptyInState(ref empty_in);
                    if (ret_empty_in == true && empty_in == true)
                    {
                        StationDevice.SetEmptyInFin(true);
                        bool empty_in_confirm = true;

                        while (empty_in_confirm)
                        {
                            ret_empty_in = StationDevice.GetEmptyInState(ref empty_in_confirm);
                            if (ret_empty_in == true && empty_in_confirm == false)
                            {
                                StationDevice.SetEmptyInFin(false);
                            }
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
        }


        private void SendFeedingSignal()
        {
            var empty_in = false;
            var ret_empty_in = StationDevice.GetEmptyInFeedingSignal(ref empty_in);
            if (ret_empty_in)
            {
                SendSingnalrEvent?.Invoke(AgvSendActionEnum.SendFeedingSignalMessage.EnumToString(), new AgvFeedingSignal
                {
                    Id = Station_Id + "_EMPTYIN",
                    ClientId = Station_Id,
                    Type = AgvMissionTypeEnum.EMPTY_IN,
                    Value = empty_in,
                });

            }

            var raw_in = false;
            var ret_raw_in = StationDevice.GetRawInFeedingSignal(ref raw_in);
            if (ret_raw_in == true)
            {
                SendSingnalrEvent?.Invoke(AgvSendActionEnum.SendFeedingSignalMessage.EnumToString(), new AgvFeedingSignal
                {
                    Id = Station_Id + "_RAWIN",
                    ClientId = Station_Id,
                    Type = AgvMissionTypeEnum.RAW_IN,
                    Value = raw_in,
                });
            }

        }

        private bool StationClientFlow()
        {
            //毛坯输入
            var raw_in = false;
            var ret = StationDevice.GetRawInRequireState(ref raw_in);
            if (ret && !raw_in)
            {
                last_rawin_state = false;
            }
            if (ret = true && raw_in == true && last_rawin_state == false)
            {
                System.Threading.Thread.Sleep(1000);

                //毛坯空箱回库
                {
                    var empty_out = false;
                    ret = StationDevice.GetEmptyOutState(ref empty_out);
                    if (ret && !empty_out)
                    {
                        last_emptyin_state = false;
                    }
                    if (ret = true && empty_out == true && last_emptyout_state == false)
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

                        SendInMission(new AgvInMissonModel
                        {
                            Id = Station_Id + "_EMPTYOUT",
                            TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                            ClientId = Station_Id,
                            Type = AgvMissionTypeEnum.EMPTY_OUT,
                            PickStationId = Station_Id,
                            PlaceStationId = AgvStationEnum.WareHouse,
                            Process = AgvMissonProcessEnum.NEW,
                            CarryProcess = CarryMissonProcessEnum.NEW,
                            Quantity = 0,
                            MaterialId = material_type,
                            ProductId = prod_type,
                            CreateDateTime = DateTime.Now,
                        });

                        last_emptyout_state = empty_out;
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

                    SendOutMission(new AgvOutMissonModel
                    {
                        Id = Station_Id + "_RAWIN",
                        TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                        ClientId = Station_Id,
                        Type = AgvMissionTypeEnum.RAW_IN,
                        PickStationId = AgvStationEnum.WareHouse,
                        PlaceStationId = Station_Id,
                        Process = AgvMissonProcessEnum.NEW,
                        CarryProcess = CarryMissonProcessEnum.NEW,
                        Quantity = 0,
                        MaterialId = material_type,
                        ProductId = prod_type,
                        CreateDateTime = DateTime.Now,
                    });

                    last_rawin_state = raw_in;
                }



            }

            //成品空箱输入
            var empty_in = false;
            ret = StationDevice.GetEmptyInState(ref empty_in);
            if (ret && !empty_in)
            {
                last_emptyin_state = false;
            }
            if (ret = true && empty_in == true && last_emptyin_state == false)
            {
                System.Threading.Thread.Sleep(1000);

                //成品回库
                {
                    var fin_out = false;
                    ret = StationDevice.GetFinOutState(ref fin_out);
                    if (ret && !fin_out)
                    {
                        last_finout_state = false;
                    }
                    if (ret = true && fin_out == true && last_finout_state == false)
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

                        SendInMission(new AgvInMissonModel
                        {
                            Id = Station_Id + "_FINOUT",
                            TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                            ClientId = Station_Id,
                            Type = AgvMissionTypeEnum.FIN_OUT,
                            PickStationId = Station_Id,
                            PlaceStationId = AgvStationEnum.WareHouse,
                            Process = AgvMissonProcessEnum.NEW,
                            CarryProcess = CarryMissonProcessEnum.NEW,
                            Quantity = 0,
                            MaterialId = material_type,
                            ProductId = prod_type,
                            CreateDateTime = DateTime.Now,
                        });

                        last_finout_state = fin_out;
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

                    SendOutMission(new AgvOutMissonModel
                    {
                        Id = Station_Id + "_EMPTYIN",
                        TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                        ClientId = Station_Id,
                        Type = AgvMissionTypeEnum.EMPTY_IN,
                        PickStationId = AgvStationEnum.WareHouse,
                        PlaceStationId = Station_Id,
                        Process = AgvMissonProcessEnum.NEW,
                        CarryProcess = CarryMissonProcessEnum.NEW,
                        Quantity = 0,
                        MaterialId = material_type,
                        ProductId = prod_type,
                        CreateDateTime = DateTime.Now,
                    });

                    last_emptyin_state = empty_in;
                }

            }

            //毛坯空箱回库
            {
                var empty_out = false;
                ret = StationDevice.GetEmptyOutState(ref empty_out);
                if (ret && !empty_out)
                {
                    last_emptyout_state = false;
                }
                if (ret = true && empty_out == true && last_emptyout_state == false)
                {
                    string prod_type = "";
                    var ret_product_type = StationDevice.GetEmptyOutProductType(ref prod_type);

                    string material_type = "";
                    var ret_material_type = StationDevice.GetEmptyOutMaterialType(ref material_type);

                    if (ret_product_type == false || ret_material_type == false)
                    {
                        return false;
                    }

                    var ret_emptyout_fin = StationDevice.SetEmptyOutFin(false);
                    if (ret_emptyout_fin == false)
                    {
                        return false;
                    }

                    SendInMission(new AgvInMissonModel
                    {
                        Id = Station_Id + "_EMPTYOUT",
                        TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                        ClientId = Station_Id,
                        Type = AgvMissionTypeEnum.EMPTY_OUT,
                        PickStationId = Station_Id,
                        PlaceStationId = AgvStationEnum.WareHouse,
                        Process = AgvMissonProcessEnum.NEW,
                        CarryProcess = CarryMissonProcessEnum.NEW,
                        Quantity = 0,
                        MaterialId = material_type,
                        ProductId = prod_type,
                        CreateDateTime = DateTime.Now,
                    });

                    last_emptyout_state = empty_out;
                }


            }

            //成品回库
            {
                var fin_out = false;
                ret = StationDevice.GetFinOutState(ref fin_out);
                if (ret && !fin_out)
                {
                    last_finout_state = false;
                }
                if (ret = true && fin_out == true && last_finout_state == false)
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

                    SendInMission(new AgvInMissonModel
                    {
                        Id = Station_Id + "_FINOUT",
                        TimeId = DateTime.Now.ToString("yyMMddmmssff"),
                        ClientId = Station_Id,
                        Type = AgvMissionTypeEnum.FIN_OUT,
                        PickStationId = Station_Id,
                        PlaceStationId = AgvStationEnum.WareHouse,
                        Process = AgvMissonProcessEnum.NEW,
                        CarryProcess = CarryMissonProcessEnum.NEW,
                        Quantity = 0,
                        MaterialId = material_type,
                        ProductId = prod_type,
                        CreateDateTime = DateTime.Now,
                    });

                    last_finout_state = fin_out;
                }


            }

            return true;
        }

        private void SendOutMission(AgvOutMissonModel mission)
        {
            SendLogEvent?.Invoke(new StationClientState { State = StationClientStateEnum.INFO, Message = "出库请求:" + mission.Type.EnumToString() }.ToString());
            SendSingnalrEvent?.Invoke(AgvSendActionEnum.SendOutMission.EnumToString(), mission);
        }

        private void SendInMission(AgvInMissonModel mission)
        {
            SendLogEvent?.Invoke(new StationClientState { State = StationClientStateEnum.INFO, Message = "入库请求:" + mission.Type.EnumToString() }.ToString());
            SendSingnalrEvent?.Invoke(AgvSendActionEnum.SendInMission.EnumToString(), mission);

        }

        public void Dispose()
        {
            if (token != null)
            {
                token.Cancel();
            }
        }
    }
}