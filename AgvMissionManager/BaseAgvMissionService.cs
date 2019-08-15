using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Agv.Common;
using DeviceAsset;
using RightCarryService;

namespace AgvMissionManager
{
    public class BaseAgvMissionService
    {
        private AgvMissionServiceErrorCodeEnum cur_Display_ErrorCode;
        private string cur_Display_Message;

        public TestControlDevice carryDevice;

        SignalrService signalrService;
        private static BaseAgvMissionService _instance = null;
        private CancellationTokenSource token = new CancellationTokenSource();
        private BlockingCollection<AgvFeedingSignal> feedingSignals = new BlockingCollection<AgvFeedingSignal>();

        Dictionary<AgvMissionTypeEnum, AgvMissionTypeEnum> brotherMissionType = new Dictionary<AgvMissionTypeEnum, AgvMissionTypeEnum>();
        private static string signalrHost;

        public event Action<AgvMissionServiceState> SendAgvMissionServiceStateMessageEvent;

        #region 任务
        public BlockingCollection<AgvOutMisson> OutMissions { get; set; }
        public BlockingCollection<AgvInMisson> InMissions { get; set; }


        public event Action<AgvInMisson, bool> AgvInMissChangeEvent;
        public event Action<AgvOutMisson, bool> AgvOutMissChangeEvent;

        #endregion

        #region ctor
        public static BaseAgvMissionService CreateInstance()
        {
            if (_instance == null)

            {
                _instance = new BaseAgvMissionService();
            }
            return _instance;
        }

       
        static BaseAgvMissionService()
        {
            signalrHost = System.Configuration.ConfigurationSettings.AppSettings["SignalrHost"];

        }
        public BaseAgvMissionService()
        {

            carryDevice = new TestControlDevice();

            signalrService = new SignalrService(signalrHost, "AgvMissonHub");

            signalrService.OnMessage<AgvOutMisson>(AgvReceiveActionEnum.receiveOutMissionMessage.EnumToString(), (s) =>
            {
                PushOutMission(s);
            });
            signalrService.OnMessage<AgvInMisson>(AgvReceiveActionEnum.receiveInMissionMessage.EnumToString(), (s) =>
            {
                PushInMission(s);
            });

            signalrService.OnMessage<AgvFeedingSignal>(AgvReceiveActionEnum.receiveFeedingSignalMessage.EnumToString(), (s) =>
            {
                UpdataFeedingSignal(s);
            });
            signalrService.OnMessage<string>(AgvReceiveActionEnum.agvStateChange.EnumToString(), (s) =>
            {

                try
                {
                    var data = s.Split('_');
                    var obj = OutMissions.LastOrDefault(d => d.Id == $"{data[0]}_{data[1]}");
                    if (obj != null)
                    {
                        obj.Process = (AgvOutMissonProcessEnum)Enum.Parse(typeof(AgvOutMissonProcessEnum), data[2]);
                    }
                    var objIn = InMissions.LastOrDefault(d => d.Id == $"{data[0]}_{data[1]}");
                    if (objIn != null)
                    {
                        objIn.Process = (AgvInMissonProcessEnum)Enum.Parse(typeof(AgvInMissonProcessEnum), data[2]);
                    }
                }
                catch (Exception)
                {


                }

            });
            OutMissions = new BlockingCollection<AgvOutMisson>();
            InMissions = new BlockingCollection<AgvInMisson>();

            //关联任务
            brotherMissionType.Add(AgvMissionTypeEnum.RAW_IN, AgvMissionTypeEnum.EMPTY_OUT);
            brotherMissionType.Add(AgvMissionTypeEnum.EMPTY_IN, AgvMissionTypeEnum.FIN_OUT);

        }

        #endregion

        #region 外部接口
        private void PushOutMission(AgvOutMisson mission)
        {
            if (OutMissions.Where(x => x.Id == mission.Id && x.Process != AgvOutMissonProcessEnum.CLOSE || x.CarryProcess!=CarryOutMissonProcessEnum.CLOSE).Count() == 0)
            {
                mission.AgvOutProcessChangeEvent += (obj, state) => AgvOutMissChangeEvent?.Invoke(obj, state);
                AgvOutMissChangeEvent?.Invoke(mission, true);

                OutMissions.Add(mission);
            }
        }

        private void PushInMission(AgvInMisson mission)
        {
            if (InMissions.Where(x => x.Id == mission.Id && x.Process != AgvInMissonProcessEnum.CLOSE || x.CarryProcess != CarryInMissonProcessEnum.CLOSE).Count() == 0)
            {
                mission.AgvInProcessChangeEvent += (s, e) => AgvInMissChangeEvent?.Invoke(s, e);
                AgvInMissChangeEvent?.Invoke(mission, true);
                InMissions.Add(mission);
            }
        }

        private void Mission_AgvInProcessChangeEvent(AgvInMisson arg1, bool arg2)
        {

        }

        private async void SendOutMission(AgvOutMisson mission)
        {
            await signalrService.Send(AgvSendActionEnum.SendOutMission.EnumToString(), mission);
        }

        private async void SendInMission(AgvInMisson mission)
        {
            await signalrService.Send(AgvSendActionEnum.SendInMission.EnumToString(), mission);
        }

        private void UpdataFeedingSignal(AgvFeedingSignal signal)
        {
            var sig = feedingSignals.Where(x => x.Id == signal.Id).FirstOrDefault();
            if (sig == null)
            {
                feedingSignals.Add(signal);
            }
            else
            {
                sig.Value = signal.Value;
            }
        }

        #endregion

        #region old
        //        public void Start()
        //        {
        //            Task.Factory.StartNew(async () =>
        //            {
        //                await signalrService.Start();

        //                while (!token.IsCancellationRequested)
        //                {
        //                    #region 防冲突
        //                    var ret = CheckMissionConflict();
        //                    if (ret.Item1 == false)
        //                    {
        //                        while (ret.Item1 == false)
        //                        {
        //                            //TODO:停止所有AGV小车动作


        //                            //设定报警
        //                            var ret_alarm = SetAlarm(true);
        //                            //TODO:通知界面
        //                            SendAgvMissionServiceStateMessage(
        //                                new AgvMissionServiceState { State = AgvMissionServiceStateEnum.ERROR, Message = "物料调用失败,发送错误信息至设备!", ErrorCode = ret.Item2 });
        //                            Thread.Sleep(1000);
        //                        }

        //                        bool dev_reset = false;
        //                        while (dev_reset == false)
        //                        {
        //                            GetReset(ref dev_reset);
        //                            //TODO:通知界面
        //                            SendAgvMissionServiceStateMessage(
        //                                new AgvMissionServiceState { State = AgvMissionServiceStateEnum.WARN, Message = "物料调用失败,请复位设备!", ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL });
        //                            Thread.Sleep(1000);
        //                        }

        //                        SendAgvMissionServiceStateMessage(
        //                            new AgvMissionServiceState { State = AgvMissionServiceStateEnum.WARN, Message = "物料调用失败,设备已复位!", ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL });

        //                    }

        //                    #endregion

        //                    var undo_inmissions = InMissions
        //                        .Where(x => x.Process > AgvInMissonProcessEnum.NEW && x.Process < AgvInMissonProcessEnum.CLOSE);
        //                    var undo_outmissions = OutMissions
        //                        .Where(x => x.Process > AgvOutMissonProcessEnum.NEW && x.Process < AgvOutMissonProcessEnum.CLOSE);

        //                    #region 小于两个在执行任务 添加一个入库任务
        //                    if ((undo_inmissions.Count() + undo_outmissions.Count()) < 2)
        //                    {
        //                        var new_inmission = InMissions.Where(x => x.Process == AgvInMissonProcessEnum.NEW).FirstOrDefault();
        //                        if (new_inmission != null) new_inmission.Process = AgvInMissonProcessEnum.START;
        //                    }

        //                    #endregion

        //                    #region 小于两个在执行任务 添加一个出库任务
        //                    if ((undo_inmissions.Count() + undo_outmissions.Count()) < 2)
        //                    {
        //                        var new_outmission = OutMissions.Where(x => x.Process == AgvOutMissonProcessEnum.NEW).FirstOrDefault();
        //                        if (new_outmission != null)
        //                        {
        //                            //检索出库任务之前是否有前置的入库任务
        //                            var brother_mission_type = brotherMissionType[new_outmission.Type];
        //                            var brother_inmission = OutMissions
        //                                .Where(x => x.ClientId == new_outmission.ClientId
        //                                    && x.Type == brother_mission_type
        //                                    && x.Process == AgvOutMissonProcessEnum.NEW)
        //                                .FirstOrDefault();

        //                            if (brother_inmission == null)
        //                            {
        //                                new_outmission.Process = AgvOutMissonProcessEnum.START;
        //                            }

        //                        }
        //                    }

        //                    #endregion

        //                    #region 通知料库入库动作
        //                    var warehouse_inmission = undo_inmissions.Where(x => x.Process == AgvInMissonProcessEnum.AGVPLACEDANDLEAVE).FirstOrDefault();
        //                    if (warehouse_inmission != null)
        //                    {
        //                        warehouse_inmission.Process = AgvInMissonProcessEnum.WHSTART;

        //#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        //                        Task.Factory.StartNew(() =>
        //                         {
        //                             var ret_whinmission = WareHouseInMission(warehouse_inmission);
        //                             if (ret_whinmission == false)
        //                             {
        //                                 warehouse_inmission.Process = AgvInMissonProcessEnum.CANCEL;

        //                                 while (ret_whinmission == false)
        //                                 {
        //                                     ret_whinmission = SetAlarm(true);
        //                                     SendAgvMissionServiceStateMessage(
        //                                         new AgvMissionServiceState
        //                                         {
        //                                             State = AgvMissionServiceStateEnum.ERROR,
        //                                             Message = "物料调用失败,料库入库动作错误!",
        //                                             ErrorCode = AgvMissionServiceErrorCodeEnum.WAREHOUSEIN
        //                                         });
        //                                     Thread.Sleep(1000);
        //                                 }

        //                                 bool dev_reset = false;
        //                                 while (dev_reset == false)
        //                                 {
        //                                     GetReset(ref dev_reset);
        //                                     SendAgvMissionServiceStateMessage(
        //                                         new AgvMissionServiceState
        //                                         {
        //                                             State = AgvMissionServiceStateEnum.WARN,
        //                                             Message = "物料调用失败,请复位设备!",
        //                                             ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL
        //                                         });
        //                                     Thread.Sleep(1000);
        //                                 }

        //                                 SendAgvMissionServiceStateMessage(
        //                                     new AgvMissionServiceState
        //                                     {
        //                                         State = AgvMissionServiceStateEnum.WARN,
        //                                         Message = "物料调用失败,设备已复位!",
        //                                         ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL
        //                                     });
        //                             }
        //                         });
        //#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法

        //                    }

        //                    #endregion

        //                    #region 通知料库出库动作
        //                    var warehouse_outmission = undo_outmissions.Where(x => x.Process == AgvOutMissonProcessEnum.START).FirstOrDefault();
        //                    if (warehouse_outmission != null)
        //                    {
        //                        warehouse_outmission.Process = AgvOutMissonProcessEnum.WHSTART;

        //#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        //                        Task.Factory.StartNew(() =>
        //                        {
        //                            var ret_whoutmission = WareHouseOutMission(warehouse_outmission);

        //                            if (ret_whoutmission == false)
        //                            {
        //                                warehouse_outmission.Process = AgvOutMissonProcessEnum.CANCEL;

        //                                while (ret_whoutmission == false)
        //                                {
        //                                    ret_whoutmission = SetAlarm(true);
        //                                    SendAgvMissionServiceStateMessage(
        //                                          new AgvMissionServiceState
        //                                          {
        //                                              State = AgvMissionServiceStateEnum.ERROR,
        //                                              Message = "物料调用失败,料库出库动作错误!",
        //                                              ErrorCode = AgvMissionServiceErrorCodeEnum.WAREHOUSEOUT
        //                                          });
        //                                    Thread.Sleep(1000);
        //                                }

        //                                bool dev_reset = false;
        //                                while (dev_reset == false)
        //                                {
        //                                    GetReset(ref dev_reset);
        //                                    SendAgvMissionServiceStateMessage(
        //                                        new AgvMissionServiceState
        //                                        {
        //                                            State = AgvMissionServiceStateEnum.WARN,
        //                                            Message = "物料调用失败,请复位设备!",
        //                                            ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL
        //                                        });
        //                                    Thread.Sleep(1000);
        //                                }

        //                                SendAgvMissionServiceStateMessage(
        //                                    new AgvMissionServiceState
        //                                    {
        //                                        State = AgvMissionServiceStateEnum.WARN,
        //                                        Message = "物料调用失败,设备已复位!",
        //                                        ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL
        //                                    });
        //                            }
        //                        });
        //#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法

        //                    }

        //                    #endregion

        //                    #region 发布小车搬运入库任务
        //                    var agv_inmission = undo_inmissions.Where(x => x.Process == AgvInMissonProcessEnum.START).FirstOrDefault();
        //                    if (agv_inmission != null)
        //                    {
        //#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        //                        Task.Factory.StartNew(async () =>
        //                        {
        //                            var ret_agvinmission = await AgvInMission(agv_inmission);
        //                            if (ret_agvinmission == false)
        //                            {
        //                                agv_inmission.Process = AgvInMissonProcessEnum.CANCEL;

        //                                while (ret_agvinmission == false)
        //                                {
        //                                    ret_agvinmission = SetAlarm(true);
        //                                    SendAgvMissionServiceStateMessage(
        //                                          new AgvMissionServiceState
        //                                          {
        //                                              State = AgvMissionServiceStateEnum.ERROR,
        //                                              Message = "物料调用失败,小车搬运入库错误!",
        //                                              ErrorCode = AgvMissionServiceErrorCodeEnum.WAREHOUSEOUT
        //                                          });
        //                                    Thread.Sleep(1000);
        //                                }

        //                                bool dev_reset = false;
        //                                while (dev_reset == false)
        //                                {
        //                                    GetReset(ref dev_reset);
        //                                    SendAgvMissionServiceStateMessage(
        //                                        new AgvMissionServiceState
        //                                        {
        //                                            State = AgvMissionServiceStateEnum.WARN,
        //                                            Message = "物料调用失败,请复位设备!",
        //                                            ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL
        //                                        });
        //                                    Thread.Sleep(1000);
        //                                }

        //                                SendAgvMissionServiceStateMessage(
        //                                    new AgvMissionServiceState
        //                                    {
        //                                        State = AgvMissionServiceStateEnum.WARN,
        //                                        Message = "物料调用失败,设备已复位!",
        //                                        ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL
        //                                    });
        //                            }
        //                        });
        //#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        //                    }

        //                    #endregion

        //                    #region 发布小车搬运出库任务
        //                    var agv_outmission = undo_outmissions.Where(x => x.Process == AgvOutMissonProcessEnum.START).FirstOrDefault();
        //                    if (agv_outmission != null)
        //                    {
        //#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        //                        Task.Factory.StartNew(async () =>
        //                        {
        //                            var ret_agvoutmission = await AgvOutMission(agv_outmission);
        //                            if (ret_agvoutmission == false)
        //                            {
        //                                agv_outmission.Process = AgvOutMissonProcessEnum.CANCEL;

        //                                while (ret_agvoutmission == false)
        //                                {
        //                                    ret_agvoutmission = SetAlarm(true);
        //                                    SendAgvMissionServiceStateMessage(
        //                                          new AgvMissionServiceState
        //                                          {
        //                                              State = AgvMissionServiceStateEnum.ERROR,
        //                                              Message = "物料调用失败,小车搬运出库错误!",
        //                                              ErrorCode = AgvMissionServiceErrorCodeEnum.WAREHOUSEOUT
        //                                          });
        //                                    Thread.Sleep(1000);
        //                                }

        //                                bool dev_reset = false;
        //                                while (dev_reset == false)
        //                                {
        //                                    GetReset(ref dev_reset);
        //                                    SendAgvMissionServiceStateMessage(
        //                                        new AgvMissionServiceState
        //                                        {
        //                                            State = AgvMissionServiceStateEnum.WARN,
        //                                            Message = "物料调用失败,请复位设备!",
        //                                            ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL
        //                                        });
        //                                    Thread.Sleep(1000);
        //                                }

        //                                SendAgvMissionServiceStateMessage(
        //                                    new AgvMissionServiceState
        //                                    {
        //                                        State = AgvMissionServiceStateEnum.WARN,
        //                                        Message = "物料调用失败,设备已复位!",
        //                                        ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL
        //                                    });
        //                            }
        //                        });
        //#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        //                    }

        //                    #endregion

        //                    #region 入库完工处理
        //                    var finished_inmission = InMissions.Where(x => x.Process == AgvInMissonProcessEnum.FINISHED).FirstOrDefault();
        //                    if (finished_inmission != null)
        //                    {
        //                        finished_inmission.Process = AgvInMissonProcessEnum.CLOSE;
        //                    }

        //                    #endregion

        //                    #region 出库完工处理
        //                    var finished_outmission = OutMissions.Where(x => x.Process == AgvOutMissonProcessEnum.FINISHED).FirstOrDefault();
        //                    if (finished_outmission != null)
        //                    {
        //                        finished_outmission.Process = AgvOutMissonProcessEnum.CLOSE;
        //                    }

        //                    #endregion

        //                    #region 入库异常处理
        //                    var cancel_inmission = InMissions.Where(x => x.Process == AgvInMissonProcessEnum.CANCEL).FirstOrDefault();
        //                    if (cancel_inmission != null)
        //                    {
        //                        cancel_inmission.Process = AgvInMissonProcessEnum.CANCELED;
        //                        AgvInMissionCancel(cancel_inmission);
        //                    }

        //                    #endregion

        //                    #region 出库异常处理
        //                    var cancel_outmission = OutMissions.Where(x => x.Process == AgvOutMissonProcessEnum.CANCELED).FirstOrDefault();
        //                    if (cancel_outmission != null)
        //                    {
        //                        cancel_outmission.Process = AgvOutMissonProcessEnum.CANCELED;
        //                        AgvOutMissionCancel(cancel_outmission);
        //                    }

        //                    #endregion

        //                    #region 发送任务状态


        //                    #endregion
        //                }
        //            }, token.Token);
        //        }

        #endregion

        public void Start(bool isNew)
        {
            Task.Factory.StartNew(async () =>
            {
                await signalrService.Start();

                SendAgvMissionServiceStateMessage(new AgvMissionServiceState { State = AgvMissionServiceStateEnum.INFO, Message = "小车管理服务启动", ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL });

                while (!token.IsCancellationRequested)
                {
                    #region 防冲突

                    //var ret = CheckMissionConflict();
                    //if (ret.Item1 == false)
                    //{
                    //    while (ret.Item1 == false)
                    //    {
                    //        //TODO:停止所有AGV小车动作


                    //        //设定报警
                    //        var ret_alarm = SetAlarm(true);
                    //        //TODO:通知界面
                    //        SendAgvMissionServiceStateMessage(
                    //            new AgvMissionServiceState { State = AgvMissionServiceStateEnum.ERROR, Message = "物料调用失败,发送错误信息至设备!", ErrorCode = ret.Item2 });
                    //        Thread.Sleep(1000);
                    //    }

                    //    bool dev_reset = false;
                    //    while (dev_reset == false)
                    //    {
                    //        GetReset(ref dev_reset);
                    //        //TODO:通知界面
                    //        SendAgvMissionServiceStateMessage(
                    //            new AgvMissionServiceState { State = AgvMissionServiceStateEnum.WARN, Message = "物料调用失败,请复位设备!", ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL });
                    //        Thread.Sleep(1000);
                    //    }

                    //    SendAgvMissionServiceStateMessage(
                    //        new AgvMissionServiceState { State = AgvMissionServiceStateEnum.WARN, Message = "物料调用失败,设备已复位!", ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL });

                    //}

                    #endregion

                    var undo_inmissions = InMissions
                        .Where(x => (x.Process > AgvInMissonProcessEnum.NEW && x.Process < AgvInMissonProcessEnum.CLOSE)
                        || (x.CarryProcess > CarryInMissonProcessEnum.NEW && x.CarryProcess < CarryInMissonProcessEnum.CLOSE)) ?? new List<AgvInMisson>();
                    var undo_outmissions = OutMissions
                        .Where(x => (x.Process > AgvOutMissonProcessEnum.NEW && x.Process < AgvOutMissonProcessEnum.CLOSE)
                        || (x.CarryProcess > CarryOutMissonProcessEnum.NEW && x.CarryProcess < CarryOutMissonProcessEnum.CLOSE)) ?? new List<AgvOutMisson>();

                    #region 小于两个在执行任务 添加一个入库任务
                    if ((undo_inmissions.Count() + undo_outmissions.Count()) < 2)
                    {
                        var new_inmission = InMissions.Where(x => x.Process == AgvInMissonProcessEnum.NEW && x.CarryProcess==CarryInMissonProcessEnum.NEW).FirstOrDefault();
                        if (new_inmission != null)
                        {
                            new_inmission.Process = AgvInMissonProcessEnum.START;
                        }
                    }

                    #endregion

                    #region 小于两个在执行任务 添加一个出库任务
                    if ((undo_inmissions.Count() + undo_outmissions.Count()) < 2)
                    {
                        var new_outmission = OutMissions.Where(x => x.Process == AgvOutMissonProcessEnum.NEW && x.CarryProcess == CarryOutMissonProcessEnum.NEW).FirstOrDefault();
                        if (new_outmission != null)
                        {
                            //检索出库任务之前是否有前置的入库任务
                            var brother_mission_type = brotherMissionType[new_outmission.Type];
                            var brother_inmission = OutMissions
                                .Where(x => x.ClientId == new_outmission.ClientId
                                    && x.Type == brother_mission_type
                                    && x.Process == AgvOutMissonProcessEnum.NEW
                                    && x.CarryProcess==CarryOutMissonProcessEnum.NEW)
                                .FirstOrDefault();

                            if (brother_inmission == null)
                            {
                                new_outmission.Process = AgvOutMissonProcessEnum.START;
                            }

                        }
                    }

                    #endregion

                    #region 通知料库入库动作
                    var warehouse_inmission = undo_inmissions.Where(x => x.Process == AgvInMissonProcessEnum.AGVPLACEDANDLEAVE
                        && x.CarryProcess == CarryInMissonProcessEnum.NEW).FirstOrDefault();
                    if (warehouse_inmission != null)
                    {
                        warehouse_inmission.CarryProcess = CarryInMissonProcessEnum.WHSTART;
                        DoWork(() => WareHouseInMission(warehouse_inmission), () =>
                          {
                              //TODO:添加料库入库错误后的动作


                          }, "料库入库动作失败", AgvMissionServiceErrorCodeEnum.WAREHOUSEIN);


                    }

                    #endregion

                    #region 通知料库出库动作
                    var warehouse_outmission = undo_outmissions.Where(x => x.CarryProcess == CarryOutMissonProcessEnum.NEW).FirstOrDefault();
                    var warehouse_outmission_last = undo_outmissions.Where(x => x.CarryProcess > CarryOutMissonProcessEnum.NEW
                        && x.Process < AgvOutMissonProcessEnum.AGVPICKEDANDLEAVE).FirstOrDefault();
                    //TODO:增加料道信号检查

                    if (warehouse_outmission != null && warehouse_outmission_last == null)
                    {
                        warehouse_outmission.CarryProcess = CarryOutMissonProcessEnum.WHSTART;

                        DoWork(() => WareHouseOutMission(warehouse_outmission), () =>
                        {
                            //TODO:添加料库出库失败动作

                        }, "料库出库动作失败", AgvMissionServiceErrorCodeEnum.WAREHOUSEOUT);


                    }

                    #endregion

                    #region 发布小车搬运入库任务
                    var agv_inmission = undo_inmissions.Where(x => x.Process == AgvInMissonProcessEnum.START).FirstOrDefault();
                    if (agv_inmission != null)
                    {

                        agv_inmission.Process = AgvInMissonProcessEnum.AGVSTART;

                        DoWork(() => AgvInMission(agv_inmission).Result, () =>
                       {
                           agv_inmission.Process = AgvInMissonProcessEnum.CANCEL;

                       }, "小车搬运入库任务失败", AgvMissionServiceErrorCodeEnum.AGVIN);



                    }

                    #endregion

                    #region 发布小车搬运出库任务
                    var agv_outmission = undo_outmissions.Where(x => x.Process == AgvOutMissonProcessEnum.START).FirstOrDefault();
                    if (agv_outmission != null)
                    {
                        agv_outmission.Process = AgvOutMissonProcessEnum.AGVSTART;

                        DoWork(() => AgvOutMission(agv_outmission).Result, () =>
                        {
                            agv_outmission.Process = AgvOutMissonProcessEnum.CANCEL;

                        }, "小车搬运出库任务失败", AgvMissionServiceErrorCodeEnum.AGVOUT);


                    }

                    #endregion

                    #region 通知小车前往单元料道输送物料等待取消
                    var agv_outmissions_atpreplace = undo_outmissions.Where(x => x.Process == AgvOutMissonProcessEnum.AGVATPREPLACE).ToList() ?? new List<AgvOutMisson>();
                    foreach (var mission in agv_outmissions_atpreplace)
                    {
                        var brother_mission_type = brotherMissionType[mission.Type];
                        var brother_inmission = OutMissions
                            .Where(x => x.ClientId == mission.ClientId
                                && x.Type == brother_mission_type
                                && x.Process >= AgvOutMissonProcessEnum.NEW && x.Process < AgvOutMissonProcessEnum.AGVPICKEDANDLEAVE)
                            .FirstOrDefault();

                        //var feeding_signal = feedingSignals.Where(x => x.ClientId == mission.ClientId && x.Type == mission.Type).FirstOrDefault() ?? new AgvFeedingSignal { Value = true };

                        //测试用 料道信号判断是否单元入料道是否有箱子--忽略
                        var feeding_signal = new AgvFeedingSignal { Value = false };


                        if (brother_inmission == null && feeding_signal.Value == false && mission.Process == AgvOutMissonProcessEnum.AGVATPREPLACE)
                        {
                            DoWork(() => AgvOutMissionPrePlaceWait(mission).Result, () =>
                            {
                                mission.Process = AgvOutMissonProcessEnum.AGVATPLACE;

                            }, "通知小车等待结束失败", AgvMissionServiceErrorCodeEnum.AGVOUTPREPLACEWAIT);
                        }
                    }

                    #endregion

                    #region 通知小车前往单元料道取料等待取消
                    var agv_inmissions_atprepick = undo_inmissions.Where(x => x.Process == AgvInMissonProcessEnum.AGVATPREPICK).ToList() ?? new List<AgvInMisson>();
                    foreach (var mission in agv_inmissions_atprepick)
                    {
                        //var brother_mission_type = brotherMissionType[mission.Type];
                        //var brother_inmission = OutMissions
                        //    .Where(x => x.ClientId == mission.ClientId
                        //        && x.Type == brother_mission_type
                        //        && x.Process >= AgvOutMissonProcessEnum.NEW && x.Process < AgvOutMissonProcessEnum.AGVPICKEDANDLEAVE)
                        //    .FirstOrDefault();

                        //var feeding_signal = feedingSignals.Where(x => x.ClientId == mission.ClientId && x.Type == mission.Type).FirstOrDefault() ?? new AgvFeedingSignal { Value = true };

                        //测试用 料道信号判断是否单元入料道是否有箱子--忽略
                        var feeding_signal = new AgvFeedingSignal { Value = true };


                        if (feeding_signal.Value == true && mission.Process == AgvInMissonProcessEnum.AGVATPREPICK)
                        {
                            DoWork(() => AgvInMissionPrePickWait(mission).Result, () =>
                            {
                                mission.Process = AgvInMissonProcessEnum.AGVATPICK;

                            }, "通知小车等待结束失败", AgvMissionServiceErrorCodeEnum.AGVINPREPICKWAIT);
                        }
                    }

                    #endregion

                    #region 通知小车前往料库出料道取物料等待取消
                    var agv_outmissions_atprepick = undo_outmissions.Where(x => x.Process == AgvOutMissonProcessEnum.AGVATPREPICK
                        &&  x.CarryProcess==CarryOutMissonProcessEnum.FINISHED).FirstOrDefault();
                    if (agv_outmissions_atprepick != null)
                    {
                        agv_outmissions_atprepick.Process = AgvOutMissonProcessEnum.AGVATPICK;
                        agv_outmissions_atprepick.CarryProcess = CarryOutMissonProcessEnum.CLOSE;

                        //TODO：添加出料道传感器信号


                        DoWork(() => AgvOutMissionPrePickWait(agv_outmissions_atprepick).Result, () =>
                        {
                            //TODO:添加小车等待信号取消失败操作

                        }, "通知小车等待结束失败", AgvMissionServiceErrorCodeEnum.AGVOUTPREPICKWAIT);

                    }



                    #endregion

                    #region 通知小车前往料库入料道放物料等待取消
                    var agv_inmissions_atpreplace = undo_inmissions.Where(x => x.Process == AgvInMissonProcessEnum.AGVATPREPLACE).FirstOrDefault();
                    var agv_inmissions_atpreplace_last = undo_inmissions.Where(x => x.CarryProcess > CarryInMissonProcessEnum.NEW
                        && x.CarryProcess < CarryInMissonProcessEnum.CLOSE || x.Process == AgvInMissonProcessEnum.AGVATPLACE).FirstOrDefault();
                    if (agv_inmissions_atpreplace != null && agv_inmissions_atpreplace_last == null)
                    {
                        agv_inmissions_atpreplace.Process = AgvInMissonProcessEnum.AGVATPLACE;

                        //TODO：添加出料道传感器信号


                        DoWork(() => AgvInMissionPrePlaceWait(agv_inmissions_atpreplace).Result, () =>
                        {
                            //TODO:添加小车等待信号取消失败操作

                        }, "通知小车等待结束失败", AgvMissionServiceErrorCodeEnum.AGVINPREPLACEWAIT);

                    }



                    #endregion

                    #region 搬运入库完工处理
                    var carryfinished_inmission = InMissions.Where(x => x.CarryProcess == CarryInMissonProcessEnum.FINISHED && x.Process > AgvInMissonProcessEnum.AGVPLACEDANDLEAVE).FirstOrDefault();
                    if (carryfinished_inmission != null)
                    {
                        carryfinished_inmission.CarryProcess = CarryInMissonProcessEnum.CLOSE;
                    }

                    #endregion

                    #region 搬运出库完工处理
                    var carryfinished_outmission = OutMissions.Where(x => x.CarryProcess == CarryOutMissonProcessEnum.FINISHED && x.Process>AgvOutMissonProcessEnum.AGVPICKEDANDLEAVE).FirstOrDefault();
                    if (carryfinished_outmission != null)
                    {
                        carryfinished_outmission.CarryProcess = CarryOutMissonProcessEnum.CLOSE;
                    }

                    #endregion

                    #region AGV入库完工处理
                    var agvfinished_inmission = InMissions.Where(x => x.Process == AgvInMissonProcessEnum.FINISHED).FirstOrDefault();
                    if (agvfinished_inmission != null)
                    {

                        agvfinished_inmission.Process = AgvInMissonProcessEnum.CLOSE;
                    }

                    #endregion

                    #region AGV出库完工处理
                    var agvfinished_outmission = OutMissions.Where(x => x.Process == AgvOutMissonProcessEnum.FINISHED).FirstOrDefault();
                    if (agvfinished_outmission != null)
                    {

                        agvfinished_outmission.Process = AgvOutMissonProcessEnum.CLOSE;
                    }

                    #endregion

                    #region 搬运入库异常处理
                    var carrycancel_inmission = InMissions.Where(x => x.CarryProcess == CarryInMissonProcessEnum.CANCEL).FirstOrDefault();
                    if (carrycancel_inmission != null)
                    {
                        carrycancel_inmission.CarryProcess = CarryInMissonProcessEnum.CANCELED;
                        CarryInMissionCancel(carrycancel_inmission);
                    }

                    #endregion

                    #region 搬运出库异常处理
                    var carrycancel_outmission = OutMissions.Where(x => x.CarryProcess == CarryOutMissonProcessEnum.CANCELED).FirstOrDefault();
                    if (carrycancel_outmission != null)
                    {
                        carrycancel_outmission.CarryProcess = CarryOutMissonProcessEnum.CANCELED;
                        CarryOutMissionCancel(carrycancel_outmission);
                    }

                    #endregion

                    #region AGV入库异常处理
                    var agvcancel_inmission = InMissions.Where(x => x.Process == AgvInMissonProcessEnum.CANCEL).FirstOrDefault();
                    if (agvcancel_inmission != null)
                    {
                        agvcancel_inmission.Process = AgvInMissonProcessEnum.CANCELED;
                        AgvInMissionCancel(agvcancel_inmission);
                    }

                    #endregion

                    #region AGV出库异常处理
                    var agvcancel_outmission = OutMissions.Where(x => x.Process == AgvOutMissonProcessEnum.CANCELED).FirstOrDefault();
                    if (agvcancel_outmission != null)
                    {
                        agvcancel_outmission.Process = AgvOutMissonProcessEnum.CANCELED;
                        AgvOutMissionCancel(agvcancel_outmission);
                    }

                    #endregion

                    #region 发送任务状态
                    foreach (var undo in undo_inmissions)
                    {
                        signalrService.Send(AgvSendActionEnum.SendInMissionFinMessage.EnumToString(), undo).Wait();
                    }

                    foreach (var undo in undo_outmissions)
                    {
                        signalrService.Send(AgvSendActionEnum.SendOutMissionFinMessage.EnumToString(), undo).Wait();
                    }

                    #endregion

                    Thread.Sleep(1000);
                }
            }, token.Token);
        }


        private void DoWork(Func<bool> ret_action, Action processAction, string message, AgvMissionServiceErrorCodeEnum codeEnum)
        {
            Task.Factory.StartNew(() =>
            {
                var ret_whoutmission = ret_action();
                if (ret_whoutmission == false)
                {
                    processAction();

                    while (ret_whoutmission == false)
                    {
                        ret_whoutmission = SetAlarm(true);
                        SendAgvMissionServiceStateMessage(
                              new AgvMissionServiceState
                              {
                                  State = AgvMissionServiceStateEnum.ERROR,
                                  Message = $"{message},动作错误!",
                                  ErrorCode = codeEnum
                              });
                        Thread.Sleep(1000);
                    }

                    bool dev_reset = false;
                    while (dev_reset == false)
                    {
                        GetReset(ref dev_reset);
                        SendAgvMissionServiceStateMessage(
                            new AgvMissionServiceState
                            {
                                State = AgvMissionServiceStateEnum.WARN,
                                Message = $"{message},请复位设备",

                                ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL
                            });
                        Thread.Sleep(1000);
                    }

                    SendAgvMissionServiceStateMessage(
                        new AgvMissionServiceState
                        {
                            State = AgvMissionServiceStateEnum.WARN,
                            Message = $"{message},设备已复位",
                            ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL
                        });
                }
            });
        }

        //TODO:
        public void Stop()
        {

        }

        //TODO:
        public void Suspend()
        {

        }

        public bool CloseOutMission(string missionId)
        {
            var finished_outmission = OutMissions.Where(x => x.Id == missionId && x.Process == AgvOutMissonProcessEnum.FINISHED).FirstOrDefault();
            if (finished_outmission != null)
            {
                finished_outmission.Process = AgvOutMissonProcessEnum.CLOSE;

            }

            return true;
        }

        public bool CloseInMission(string missionId)
        {
            var finished_inmission = InMissions.Where(x => x.Id == missionId && x.Process == AgvInMissonProcessEnum.FINISHED).FirstOrDefault();
            if (finished_inmission != null)
            {
                finished_inmission.Process = AgvInMissonProcessEnum.CLOSE;

            }

            return true;
        }

        //TODO:小车入库任务取消
        private bool AgvInMissionCancel(AgvInMisson mission)
        {
            //TODO:添加异常处理

            //mission.Process = AgvInMissonProcessEnum.CLOSE;
            return true;
        }

        //TODO:小车出库任务取消
        private bool AgvOutMissionCancel(AgvOutMisson mission)
        {
            //TODO:添加异常处理

            //mission.Process = AgvOutMissonProcessEnum.CLOSE;
            return true;
        }

        //TODO:搬运入库任务取消
        private bool CarryInMissionCancel(AgvInMisson mission)
        {
            //TODO:添加异常处理

            //mission.Process = AgvInMissonProcessEnum.CLOSE;
            return true;
        }

        //TODO:搬运出库任务取消
        private bool CarryOutMissionCancel(AgvOutMisson mission)
        {
            //TODO:添加异常处理

            //mission.Process = AgvOutMissonProcessEnum.CLOSE;
            return true;
        }

        private void SendAgvMissionServiceStateMessage(AgvMissionServiceState state)
        {
            if (cur_Display_ErrorCode != state.ErrorCode || state.Message != cur_Display_Message)
            {
                SendAgvMissionServiceStateMessageEvent?.Invoke(state);

                cur_Display_ErrorCode = state.ErrorCode;
                cur_Display_Message = state.Message;
            }

            if (cur_Display_ErrorCode != state.ErrorCode)
            {
                Console.WriteLine($"【RIGHT MATERIAL MISSION】【ERROR CODE】: {state.ErrorCode}     【MESSAGE】:{state.Message}");

                cur_Display_ErrorCode = state.ErrorCode;
            }
        }

        private void UpdataAgvInMisson()
        {

        }

        private void AgvOrderStateEvent()
        {

        }

        private Tuple<bool, AgvMissionServiceErrorCodeEnum> CheckMissionConflict()
        {
            var undo_inmissions = InMissions
                .Where(x => x.Process > AgvInMissonProcessEnum.NEW && x.Process < AgvInMissonProcessEnum.CLOSE);
            var undo_outmissions = OutMissions
                .Where(x => x.Process > AgvOutMissonProcessEnum.NEW && x.Process < AgvOutMissonProcessEnum.CLOSE);

            //存在相同ID的任务
            var max_inmission = undo_inmissions.GroupBy(x => x.Id).Select(x => new { num = x.Count() }).Max() ?? new { num = 0 };
            var max_outmission = undo_outmissions.GroupBy(x => x.Id).Select(x => new { num = x.Count() }).Max() ?? new { num = 0 };
            if (max_inmission.num > 1 || max_outmission.num > 1)
            {
                return new Tuple<bool, AgvMissionServiceErrorCodeEnum>
                    (false, AgvMissionServiceErrorCodeEnum.IDCONFLICT);
            }

            //多于2个任务正在执行
            var count_inmissions = undo_inmissions.Count();
            var count_outmissions = undo_outmissions.Count();
            if (count_inmissions + count_outmissions > 2)
            {
                return new Tuple<bool, AgvMissionServiceErrorCodeEnum>
                    (false, AgvMissionServiceErrorCodeEnum.QUANTITYLIMIT);
            }

            //存在两个出料库任务，已经达到AGVSTART， 但是小于AGVLEAVEPICK
            //var count_outmission_conflict = undo_outmissions
            //    .Where(x => x.Process >= AgvOutMissonProcessEnum.AGVSTART && x.Process < AgvOutMissonProcessEnum.AGVLEAVEPICK).Count();
            //if (count_outmission_conflict > 1)
            //{
            //    return new Tuple<bool, AgvMissionServiceErrorCodeEnum>
            //        (false, AgvMissionServiceErrorCodeEnum.IDCONFLICT);
            //}

            //        //只能有一个入料库任务
            //        var count_inmission_conflict = undo_inmissions.Count();
            //        if (count_inmission_conflict > 1)
            //        {
            //            return new Tuple<bool, AgvMissionServiceErrorCodeEnum>
            //(false, AgvMissionServiceErrorCodeEnum.IDCONFLICT);
            //}

            return new Tuple<bool, AgvMissionServiceErrorCodeEnum>
                (true, AgvMissionServiceErrorCodeEnum.NORMAL);
        }

        //TODO:发送入库任务给小车调度中心
        private void SendInMissionToAgvRoboRoute(AgvInMisson mission)
        {


        }

        //TODO:发送错误消息给小车任务管理系统
        private bool SetAlarm(bool alarm)
        {
            return true;
        }

        //TODO:从小车任务管理系统获得复位信号
        private bool GetReset(ref bool reset)
        {
            return true;
        }

        //料库执行入库
        private bool WareHouseInMission(AgvInMisson mission)
        {
            TestRightCarryService<TestControlDevice> carry = new TestRightCarryService<TestControlDevice>(carryDevice);

            var ret = carry.CarryIn(mission.ProductId, mission.MaterialId);

            if (ret == false)
            {
                mission.Process = AgvInMissonProcessEnum.CANCEL;
                mission.CarryProcess = CarryInMissonProcessEnum.CANCEL;
                carry.ReleaseLock();
                return false;
            }

            mission.CarryProcess = CarryInMissonProcessEnum.FINISHED;
            mission.Process = AgvInMissonProcessEnum.FINISHED;
            carry.ReleaseLock();
            return true;
        }

        //料库执行出库
        private bool WareHouseOutMission(AgvOutMisson mission)
        {

            TestRightCarryService<TestControlDevice> carry = new TestRightCarryService<TestControlDevice>(carryDevice);

            int quantity = 0;
            var ret = carry.CarryOut(mission.ProductId, mission.MaterialId, ref quantity);

            if (ret == false)
            {
                mission.Process = AgvOutMissonProcessEnum.CANCEL;
                mission.CarryProcess = CarryOutMissonProcessEnum.CANCEL;
                carry.ReleaseLock();
                return false;
            }

            mission.CarryProcess = CarryOutMissonProcessEnum.FINISHED;
            carry.ReleaseLock();
            return true;
        }

        //TODO:异步小车入库搬运
        private async Task<bool> AgvInMission(AgvInMisson mission)
        {
            try
            {
                await signalrService.Send(AgvSendActionEnum.SendMissonInOrder.EnumToString(), mission);

            }
            catch (Exception ex)
            {


            }

            return true;


        }

        //TODO:异步小车出库搬运
        private async Task<bool> AgvOutMission(AgvOutMisson mission)
        {
            try
            {

                await signalrService.Send(AgvSendActionEnum.SendMissonOutOrder.EnumToString(), mission);

            }
            catch (Exception ex)
            {


            }

            return true;
        }

        private async Task<bool> AgvOutMissionPrePlaceWait(AgvOutMisson AgvOutMisson)
        {
            await signalrService.Send<string>(AgvSendActionEnum.SendLastWaitEndSignal.EnumToString(), AgvOutMisson.Id);
            return true;
        }

        private async Task<bool> AgvOutMissionPrePickWait(AgvOutMisson AgvOutMisson)
        {
            await signalrService.Send<string>(AgvSendActionEnum.SendFirstWaitEndSignal.EnumToString(), AgvOutMisson.Id);
            
            return true;
        }

        private async Task<bool> AgvInMissionPrePlaceWait(AgvInMisson AgvInMisson)
        {
            await signalrService.Send<string>(AgvSendActionEnum.SendLastWaitEndSignal.EnumToString(), AgvInMisson.Id);

            return true;
        }

        private async Task<bool> AgvInMissionPrePickWait(AgvInMisson AgvInMisson)
        {
            await signalrService.Send<string>(AgvSendActionEnum.SendFirstWaitEndSignal.EnumToString(), AgvInMisson.Id);

            return true;
        }

    }
}
