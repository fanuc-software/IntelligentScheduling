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

        private MissionContext missionContext;
        private BlockingCollection<AgvFeedingSignal> feedingSignals = new BlockingCollection<AgvFeedingSignal>();

        public event Action<AgvMissonModel, bool> AgvMissonChangeEvent;
        private List<IMissionState> missionStates;
        public event Action<string> ShowLogEvent;
        public event Action<string, object> SendSignalrEvent;
        public event Action<AgvMissonModel> SendAgvMissonEvent;
        private CancellationTokenSource token;


        public AgvMissionManagerClient()
        {
            missionContext = new MissionContext();
            missionContext.SendAgvMissionServiceStateMessageEvent += (obj) => ShowLogEvent?.Invoke(obj);
            missionContext.SendSignalrEvent += (action, obj) => SendSignalrEvent?.Invoke(action, obj);
            missionContext.SendAgvMissonEvent += (s) => SendAgvMissonEvent?.Invoke(s);

            InitMissionState();

        }
        public void Clear()
        {
            missionContext.ClearNodes();
        }
        public void Start()
        {
            if (token != null && !token.IsCancellationRequested)
            {
                token.Cancel();
            }
            token = new CancellationTokenSource();
            var startState = new AgvMissionServiceState { State = AgvMissionServiceStateEnum.INFO, Message = "小车管理服务启动", ErrorCode = AgvMissionServiceErrorCodeEnum.NORMAL };
            ShowLogEvent?.Invoke(startState.ToString());

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
                Thread.Sleep(1000);
            }

        }
        public void Cancel()
        {
            if (token != null)
            {
                token.Cancel();

            }
        }

        private void InitMissionState()
        {
            missionStates = new List<IMissionState>()
            {
                new InMissonTask(missionContext), new OutMissonTask(missionContext),
                new CarryInMissonTask(missionContext), new CarryOutMissonTask(missionContext),
                new AgvInMissonTask(missionContext),new AgvOutMissonTask(missionContext),
                new AgvFromCarryToUnitWaitCancelTask(missionContext),new AgvToCarryMaterialWaitCancelTask(missionContext),
                new AgvToCarryTakeMaterialWaitCancelTask(missionContext),new AgvToCarrySetMaterialWaitCancelTask(missionContext),
                new CarryMissonInFinished(missionContext),new CarryMissonOutFinished(missionContext),
                new AgvMissonInFinished(missionContext),new AgvMissonOutFinished(missionContext),
                new CarryMissonInCancel(missionContext),new CarryMissonOutCancel(missionContext),
                new AgvMissonInCancel(missionContext),new AgvMissonOutCancel(missionContext)
            };
        }

        #region 外部接口

        public void AgvStateChange(string s)
        {
            try
            {
                var data = s.Split('_');
                var obj = missionContext.MissionOutNodes.LastOrDefault(d => d.Id == $"{data[0]}_{data[1]}");
                if (obj != null)
                {
                    obj.Process = (AgvMissonProcessEnum)Enum.Parse(typeof(AgvMissonProcessEnum), data[2]);
                    SendAgvMissonEvent?.Invoke(obj);

                }
                var objIn = missionContext.MissionInNodes.LastOrDefault(d => d.Id == $"{data[0]}_{data[1]}");
                if (objIn != null)
                {
                    objIn.Process = (AgvMissonProcessEnum)Enum.Parse(typeof(AgvMissonProcessEnum), data[2]);
                    SendAgvMissonEvent?.Invoke(obj);

                }
            }
            catch (Exception ex)
            {
                ShowLogEvent?.Invoke("AgvStateChange Error:" + ex.Message);

            }
        }

        public void PushOutMission(AgvOutMissonModel mission)
        {
            if (missionContext.MissionOutNodes.Where(x => x.Id == mission.Id && (x.Process != AgvMissonProcessEnum.CLOSE || x.CarryProcess != CarryMissonProcessEnum.CLOSE)).Count() == 0)
            {
                mission.AgvProcessChangeEvent += (obj, state) => AgvMissonChangeEvent?.Invoke(obj, state);
                AgvMissonChangeEvent?.Invoke(mission, true);
                missionContext.MissionOutNodes.Add(mission);
                SendAgvMissonEvent?.Invoke(mission);
            }
        }

        public void PushInMission(AgvInMissonModel mission)
        {
            if (missionContext.MissionInNodes.Where(x => x.Id == mission.Id && (x.Process != AgvMissonProcessEnum.CLOSE || x.CarryProcess != CarryMissonProcessEnum.CLOSE)).Count() == 0)
            {
                mission.AgvProcessChangeEvent += (s, e) => AgvMissonChangeEvent?.Invoke(s, e);
                AgvMissonChangeEvent?.Invoke(mission, true);
                missionContext.MissionInNodes.Add(mission);
                SendAgvMissonEvent?.Invoke(mission);

            }
        }



        public void UpdataFeedingSignal(AgvFeedingSignal signal)
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
