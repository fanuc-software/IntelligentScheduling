using Agv.Common;
using Agv.Common.Model;
using RightCarryService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgvMissionManager.MissionState
{
    public class MissionContext
    {

        public event Action<string> SendAgvMissionServiceStateMessageEvent;
        public event Action<string, object> SendSignalrEvent;

        public List<AgvInMissonModel> undo_inmissions;
        public List<AgvOutMissonModel> undo_outmissions;
        public Dictionary<AgvMissionTypeEnum, AgvMissionTypeEnum> brotherMissionType;

        public BlockingCollection<AgvInMissonModel> MissionInNodes { get; set; }
        public BlockingCollection<AgvOutMissonModel> MissionOutNodes { get; set; }
        public IControlDevice carryDevice;
        public event Action<AgvMissonModel> SendAgvMissonEvent;


        public MissionContext()
        {
            brotherMissionType = new Dictionary<AgvMissionTypeEnum, AgvMissionTypeEnum>();
            MissionInNodes = new BlockingCollection<AgvInMissonModel>();
            MissionOutNodes = new BlockingCollection<AgvOutMissonModel>();
            brotherMissionType.Add(AgvMissionTypeEnum.RAW_IN, AgvMissionTypeEnum.EMPTY_OUT);
            brotherMissionType.Add(AgvMissionTypeEnum.EMPTY_IN, AgvMissionTypeEnum.FIN_OUT);

            //测试用
            //实例化 RightCarryService\AllenBradleyControlDevice
            //carryDevice = new TestControlDevice();
            carryDevice = new AllenBradleyControlDevice();
        }

        public void ClearNodes()
        {
            MissionInNodes = new BlockingCollection<AgvInMissonModel>();
            MissionOutNodes = new BlockingCollection<AgvOutMissonModel>();
        }
        public void SendAgvMisson(AgvMissonModel obj)
        {
            SendAgvMissonEvent?.Invoke(obj);
        }
        public void Init()
        {
            undo_inmissions = MissionInNodes
                      .Where(x => (x.Process > AgvMissonProcessEnum.NEW && x.Process < AgvMissonProcessEnum.CLOSE)
                      || (x.CarryProcess > CarryMissonProcessEnum.NEW && x.CarryProcess < CarryMissonProcessEnum.CLOSE)).ToList() ?? new List<AgvInMissonModel>();
            undo_outmissions = MissionOutNodes
               .Where(x => (x.Process > AgvMissonProcessEnum.NEW && x.Process < AgvMissonProcessEnum.CLOSE)
               || (x.CarryProcess > CarryMissonProcessEnum.NEW && x.CarryProcess < CarryMissonProcessEnum.CLOSE)).ToList() ?? new List<AgvOutMissonModel>();
        }
        public void PushTask()
        {
            foreach (var undo in undo_inmissions)
            {
                SendSignalrEvent?.Invoke(AgvSendActionEnum.SendInMissionFinMessage.EnumToString(), undo);
            }

            foreach (var undo in undo_outmissions)
            {
                SendSignalrEvent?.Invoke(AgvSendActionEnum.SendOutMissionFinMessage.EnumToString(), undo);

            }
        }
        public void DoWork(Func<bool> ret_action, Action processAction, string message, AgvMissionServiceErrorCodeEnum codeEnum)
        {
            Task.Factory.StartNew(() =>
            {
                var ret_whoutmission = ret_action();
                if (ret_whoutmission == false)
                {
                    processAction();
                    int maxCount = 10;
                    while (ret_whoutmission == false && maxCount-- > 0)
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
                    maxCount = 10;
                    while (dev_reset == false && maxCount-- > 0)
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

        public bool AgvPushMission(string actionName, object mission)
        {
            // Singalr 发送订单
            //   await signalrService.Send(AgvSendActionEnum.SendMissonInOrder.EnumToString(), mission);
            SendSignalrEvent?.Invoke(actionName, mission);

            return true;


        }
        public void SendAgvMissionServiceStateMessage(AgvMissionServiceState state)
        {
            SendAgvMissionServiceStateMessageEvent?.Invoke(state.ToString());
        }
        //TODO:发送错误消息给小车任务管理系统
        public bool SetAlarm(bool alarm)
        {
            return true;
        }

        //TODO:从小车任务管理系统获得复位信号
        public bool GetReset(ref bool reset)
        {
            return true;
        }
    }
}
