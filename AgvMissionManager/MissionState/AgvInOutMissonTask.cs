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
    /// AGV小车搬运入库任务
    /// </summary>
    public class AgvInMissonTask : IMissionState
    {
        private MissionContext missionContext;
        public AgvInMissonTask(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        private AgvInMissonModel agv_inmission;

        public bool CanRequest()
        {
            agv_inmission = missionContext.undo_inmissions.Where(x => x.Process == AgvMissonProcessEnum.START).FirstOrDefault();
            return agv_inmission != null;

        }

        public string Condition()
        {
            return $"【AGV小车搬运入库任务-判断条件】：Process=Start";
        }

        public void Handle()
        {
            agv_inmission.Process = AgvMissonProcessEnum.AGVSTART;
            missionContext.SendAgvMisson(agv_inmission);

            missionContext.DoWork(() => missionContext.AgvPushMission(AgvSendActionEnum.SendMissonInOrder.EnumToString(), agv_inmission), () =>
            {
                agv_inmission.Process = AgvMissonProcessEnum.CANCEL;
                missionContext.SendAgvMisson(agv_inmission);


            }, "小车搬运入库任务失败", AgvMissionServiceErrorCodeEnum.AGVIN);

        }
    }

    /// <summary>
    /// AGV小车搬运出库任务
    /// </summary>
    public class AgvOutMissonTask : IMissionState
    {

        private AgvOutMissonModel agv_outmission;
        private MissionContext missionContext;
        public AgvOutMissonTask(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            agv_outmission = missionContext.undo_outmissions.Where(x => x.Process == AgvMissonProcessEnum.START).FirstOrDefault();
            return agv_outmission != null;

        }

        public string Condition()
        {
            return $"【AGV小车搬运出库任务-判断条件】：Process=Start";
        }

        public void Handle()
        {
            agv_outmission.Process = AgvMissonProcessEnum.AGVSTART;
            missionContext.SendAgvMisson(agv_outmission);

            missionContext.DoWork(() => missionContext.AgvPushMission(AgvSendActionEnum.SendMissonOutOrder.EnumToString(), agv_outmission), () =>
            {
                agv_outmission.Process = AgvMissonProcessEnum.CANCEL;
                missionContext.SendAgvMisson(agv_outmission);

            }, "小车搬运出库任务失败", AgvMissionServiceErrorCodeEnum.AGVOUT);
        }
    }
}
