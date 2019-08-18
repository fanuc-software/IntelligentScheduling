using Agv.Common;
using Agv.Common.Model;
using AgvMissionManager.MissionState;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgvMissionManager
{
    public class AgvMissionManagerClient
    {
        private SignalrService signalrService;

        private static string signalrHost;
        private MissionContext missionContext;
        private BlockingCollection<AgvFeedingSignal> feedingSignals = new BlockingCollection<AgvFeedingSignal>();

        public event Action<AgvMissonModel, bool> AgvMissonChangeEvent;
        private List<IMissionState> missionStates;
        public event Action<string> ShowLogEvent;
        private CancellationTokenSource token;

        [Obsolete]
        static AgvMissionManagerClient()
        {
            signalrHost = System.Configuration.ConfigurationSettings.AppSettings["SignalrHost"];

        }

        public AgvMissionManagerClient()
        {
            missionContext = new MissionContext();
            missionContext.SendAgvMissionServiceStateMessageEvent += (obj) => ShowLogEvent?.Invoke(obj);
            missionContext.SendSignalrEvent += (action, obj) => signalrService.Send(action, obj.ToString()).Wait();
            InitMissionState();
            InitSignlar();
        }

        public async Task Start()
        {
            if (token != null && !token.IsCancellationRequested)
            {
                token.Cancel();
            }
            token = new CancellationTokenSource();
            var startState = new AgvMissionServiceState { State = AgvMissionServiceStateEnum.INFO, Message = "小车管理服务启动", ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL };
            ShowLogEvent?.Invoke(startState.ToString());

            await Task.Factory.StartNew(async d =>
             {
                 while (!token.IsCancellationRequested)
                 {
                     missionContext.Init();
                     foreach (var item in missionStates)
                     {
                         if (item.CanRequest())
                         {
                             ShowLogEvent?.Invoke($"【Time:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}】{item.Condition()}");
                             item.Handle();
                         }

                     }

                     missionContext.PushTask();
                     await Task.Delay(1000);
                 }


             }, token.Token);

        }


        private void InitMissionState()
        {
            missionStates = new List<IMissionState>()
            {
                new InMissonTask(missionContext), new OutMissonTask(missionContext),
                new CarryInMissonTask(missionContext), new CarryOutMissonTask(missionContext),
                new AgvFromCarryToUnitWaitCancelTask(missionContext),new AgvToCarryMaterialWaitCancelTask(missionContext),
                new AgvToCarryTakeMaterialWaitCancelTask(missionContext),new AgvToCarrySetMaterialWaitCancelTask(missionContext),
                new CarryMissonInFinished(missionContext),new CarryMissonOutFinished(missionContext),
                new AgvMissonInFinished(missionContext),new AgvMissonOutFinished(missionContext),
                new AgvInMissonTask(missionContext),new AgvOutMissonTask(missionContext),
                new CarryMissonInCancel(missionContext),new CarryMissonOutCancel(missionContext),
                new AgvMissonInCancel(missionContext),new AgvMissonOutCancel(missionContext)
            };
        }
        private async void InitSignlar()
        {
            signalrService = new SignalrService(signalrHost, "AgvMissonHub");
            try
            {
                await signalrService.Start();

            }
            catch (Exception ex)
            {
                ShowLogEvent?.Invoke("Signlar Error:" + ex.Message);


            }
            signalrService.OnMessage<AgvOutMissonModel>(AgvReceiveActionEnum.receiveOutMissionMessage.EnumToString(), (s) =>
            {
                PushOutMission(s);
            });
            signalrService.OnMessage<AgvInMissonModel>(AgvReceiveActionEnum.receiveInMissionMessage.EnumToString(), (s) =>
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
                    var obj = missionContext.MissionOutNodes.LastOrDefault(d => d.Id == $"{data[0]}_{data[1]}");
                    if (obj != null)
                    {
                        obj.Process = (AgvMissonProcessEnum)Enum.Parse(typeof(AgvMissonProcessEnum), data[2]);
                    }
                    var objIn = missionContext.MissionInNodes.LastOrDefault(d => d.Id == $"{data[0]}_{data[1]}");
                    if (objIn != null)
                    {
                        objIn.Process = (AgvMissonProcessEnum)Enum.Parse(typeof(AgvMissonProcessEnum), data[2]);
                    }
                }
                catch (Exception)
                {


                }

            });
        }

        #region 外部接口
        private void PushOutMission(AgvOutMissonModel mission)
        {
            if (missionContext.MissionOutNodes.Where(x => x.Id == mission.Id && (x.Process != AgvMissonProcessEnum.CLOSE || x.CarryProcess != CarryMissonProcessEnum.CLOSE)).Count() == 0)
            {
                mission.AgvProcessChangeEvent += (obj, state) => AgvMissonChangeEvent?.Invoke(obj, state);
                AgvMissonChangeEvent?.Invoke(mission, true);
                missionContext.MissionOutNodes.Add(mission);
            }
        }

        private void PushInMission(AgvInMissonModel mission)
        {
            if (missionContext.MissionInNodes.Where(x => x.Id == mission.Id && (x.Process != AgvMissonProcessEnum.CLOSE || x.CarryProcess != CarryMissonProcessEnum.CLOSE)).Count() == 0)
            {
                mission.AgvProcessChangeEvent += (s, e) => AgvMissonChangeEvent?.Invoke(s, e);
                AgvMissonChangeEvent?.Invoke(mission, true);
                missionContext.MissionInNodes.Add(mission);
            }
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
    }
}
