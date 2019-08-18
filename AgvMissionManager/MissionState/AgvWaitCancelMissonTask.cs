using Agv.Common;
using Agv.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvMissionManager.MissionState
{
    /// <summary>
    /// 小车前往单元料道输送物料等待取消 RAWIN EMPTYIN (料库取空箱或者取物料送回单元料道，取消最后个等待点)
    /// </summary>
    public class AgvFromCarryToUnitWaitCancelTask : IMissionState
    {
        List<AgvOutMissonModel> missonNodes = new List<AgvOutMissonModel>();

        private MissionContext missionContext;

        public AgvFromCarryToUnitWaitCancelTask(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            missonNodes.Clear();
            var agv_outmissions_atpreplace = missionContext.undo_outmissions.Where(x => x.Process == AgvMissonProcessEnum.AGVATPREPLACE).ToList() ?? new List<AgvOutMissonModel>();
            foreach (var mission in agv_outmissions_atpreplace)
            {
                var brother_mission_type = missionContext.brotherMissionType[mission.Type];
                var brother_inmission = missionContext.MissionInNodes
                    .Where(x => x.ClientId == mission.ClientId
                        && x.Type == brother_mission_type
                        && x.Process >= AgvMissonProcessEnum.NEW && x.Process < AgvMissonProcessEnum.AGVPICKEDANDLEAVE)
                    .FirstOrDefault();

                //var feeding_signal = feedingSignals.Where(x => x.ClientId == mission.ClientId && x.Type == mission.Type).FirstOrDefault() ?? new AgvFeedingSignal { Value = true };

                //测试用 料道信号判断是否单元入料道是否有箱子--忽略
                var feeding_signal = new AgvFeedingSignal { Value = false };


                if (brother_inmission == null && feeding_signal.Value == false && mission.Process == AgvMissonProcessEnum.AGVATPREPLACE)
                {
                    missonNodes.Add(mission);

                }
            }
            return missonNodes.Count > 0;
        }

        public string Condition()
        {
            return "【小车前往单元料道输送物料，取消路线最后等待点信号】,Process=AgvAtPreplace,PreProcess=AgvPickedAndLeave";
        }

        public void Handle()
        {
            foreach (var mission in missonNodes)
            {
                missionContext.DoWork(() => missionContext.AgvPushMission(AgvSendActionEnum.SendLastWaitEndSignal.EnumToString(), mission.Id), () =>
              {
                  mission.Process = AgvMissonProcessEnum.AGVATPLACE;

              }, "通知小车等待结束失败", AgvMissionServiceErrorCodeEnum.AGVOUTPREPLACEWAIT);
            }

        }
    }


    /// <summary>
    /// 小车前往单元料道取料等待取消  EMPTYOUT FINOUT  (料库取空箱或者取物料送回单元料道，取消第一个个等待点)
    /// </summary>
    public class AgvToCarryMaterialWaitCancelTask : IMissionState
    {
        List<AgvInMissonModel> missonModels = new List<AgvInMissonModel>();

        private MissionContext missionContext;

        public AgvToCarryMaterialWaitCancelTask(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            missonModels.Clear();
            var agv_inmissions_atprepick = missionContext.undo_inmissions.Where(x => x.Process == AgvMissonProcessEnum.AGVATPREPICK).ToList() ?? new List<AgvInMissonModel>();
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


                if (feeding_signal.Value == true && mission.Process == AgvMissonProcessEnum.AGVATPREPICK)
                {
                    missonModels.Add(mission);

                }
            }
            return missonModels.Count > 0;
        }

        public string Condition()
        {
            return "【小车前往单元料道取物料，取消路线第一个等待点信号】,Process=AgvAtPreplace";
        }

        public void Handle()
        {
            foreach (var mission in missonModels)
            {
                missionContext.DoWork(() => missionContext.AgvPushMission(AgvSendActionEnum.SendFirstWaitEndSignal.EnumToString(), mission.Id), () =>
                 {
                     mission.Process = AgvMissonProcessEnum.AGVATPICK;

                 }, "通知小车等待结束失败", AgvMissionServiceErrorCodeEnum.AGVINPREPICKWAIT);
            }

        }
    }


    /// <summary>
    /// 小车前往料库出料道取物料等待取消 RAWIN EMPTYIN (取消第一个等待点)
    /// </summary>
    public class AgvToCarryTakeMaterialWaitCancelTask : IMissionState
    {
        private AgvOutMissonModel agv_outmissions_atprepick;
        private MissionContext missionContext;

        public AgvToCarryTakeMaterialWaitCancelTask(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            agv_outmissions_atprepick = missionContext.undo_outmissions.Where(x => x.Process == AgvMissonProcessEnum.AGVATPREPICK
            && x.CarryProcess == CarryMissonProcessEnum.FINISHED).FirstOrDefault();
            return agv_outmissions_atprepick != null;
        }

        public string Condition()
        {
            return "【小车前往料库道取物料，取消路线第一个等待点信号】,Process=AgvAtPreplace,CarryProcess=Finished";

        }

        public void Handle()
        {
            agv_outmissions_atprepick.Process = AgvMissonProcessEnum.AGVATPICK;
            agv_outmissions_atprepick.CarryProcess = CarryMissonProcessEnum.CLOSE;

            //TODO：添加出料道传感器信号


            missionContext.DoWork(() => missionContext.AgvPushMission(AgvSendActionEnum.SendFirstWaitEndSignal.EnumToString(), agv_outmissions_atprepick.Id), () =>
           {
                //TODO:添加小车等待信号取消失败操作

            }, "通知小车等待结束失败", AgvMissionServiceErrorCodeEnum.AGVOUTPREPICKWAIT);
        }
    }


    /// <summary>
    /// 小车前往料库入料道放物料等待取消  EMPTYOUT FINOUT  (取消最后个等待点)
    /// </summary>
    public class AgvToCarrySetMaterialWaitCancelTask : IMissionState
    {
        private AgvInMissonModel agv_inmissions_atpreplace;
        private MissionContext missionContext;

        public AgvToCarrySetMaterialWaitCancelTask(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            agv_inmissions_atpreplace = missionContext.undo_inmissions.Where(x => x.Process == AgvMissonProcessEnum.AGVATPREPLACE).FirstOrDefault();
            var agv_inmissions_atpreplace_last = missionContext.undo_inmissions.Where(x => x.CarryProcess > CarryMissonProcessEnum.NEW
                && x.CarryProcess < CarryMissonProcessEnum.CLOSE || x.Process == AgvMissonProcessEnum.AGVATPLACE).FirstOrDefault();

            return agv_inmissions_atpreplace != null && agv_inmissions_atpreplace_last == null;

        }

        public string Condition()
        {
            return "【小车前往料库道放物料，取消路线最后一个等待点信号】,Process=AgvAtPreplace,CarryProcess=NEW And CarryProcess<Close Or Process=AgvAtplace";

        }

        public void Handle()
        {
            agv_inmissions_atpreplace.Process = AgvMissonProcessEnum.AGVATPLACE;

            //TODO：添加出料道传感器信号


            missionContext.DoWork(() => missionContext.AgvPushMission(AgvSendActionEnum.SendLastWaitEndSignal.EnumToString(), agv_inmissions_atpreplace.Id), () =>
            {
                //TODO:添加小车等待信号取消失败操作

            }, "通知小车等待结束失败", AgvMissionServiceErrorCodeEnum.AGVINPREPLACEWAIT);
        }
    }
}
