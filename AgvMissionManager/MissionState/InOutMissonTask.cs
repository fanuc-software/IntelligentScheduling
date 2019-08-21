using Agv.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvMissionManager.MissionState
{
    /// <summary>
    /// 添加一个入库任务
    /// </summary>
    public class InMissonTask : IMissionState
    {
        private AgvInMissonModel new_inmission;
        private MissionContext missionContext;

        
        public InMissonTask(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            if ((missionContext.undo_inmissions.Count + missionContext.undo_outmissions.Count) < 2)
            {
                new_inmission = missionContext.MissionInNodes.Where(x => x.Process == AgvMissonProcessEnum.NEW && x.CarryProcess == CarryMissonProcessEnum.NEW).FirstOrDefault();
                return new_inmission != null;

            }
            return false;
        }

        public string Condition()
        {
            return $"【添加入库任务-判断条件】： undo_inmissions.Count+undo_outmissions<2 And CarryProcess=New And Process=New";
        }

        public void Handle()
        {
            new_inmission.Process = AgvMissonProcessEnum.START;
            missionContext.SendAgvMisson(new_inmission);
        }
    }

    /// <summary>
    /// 添加一个出库任务
    /// </summary>
    public class OutMissonTask :  IMissionState
    {
        private AgvOutMissonModel new_outmission;
        private MissionContext missionContext;

        public OutMissonTask(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            if ((missionContext.undo_inmissions.Count() + missionContext.undo_outmissions.Count()) < 2)
            {
                new_outmission = missionContext.MissionOutNodes.Where(x => x.Process == AgvMissonProcessEnum.NEW && x.CarryProcess == CarryMissonProcessEnum.NEW).FirstOrDefault();
                if (new_outmission != null)
                {
                    //检索出库任务之前是否有前置的入库任务
                    var brother_mission_type = missionContext.brotherMissionType[new_outmission.Type];
                    var brother_inmission = missionContext.MissionOutNodes
                        .Where(x => x.ClientId == new_outmission.ClientId
                            && x.Type == brother_mission_type
                            && x.Process == AgvMissonProcessEnum.NEW
                            && x.CarryProcess == CarryMissonProcessEnum.NEW)
                        .FirstOrDefault();
                    return brother_inmission == null;
                }
            }
            return false;
        }

        public string Condition()
        {
            return $"【添加出库任务-判断条件】:undo_inmissions.Count+undo_outmissions<2 And CarryProcess=New And Process=New And 【NoPreTask】";
        }

        public void Handle()
        {
            new_outmission.Process = AgvMissonProcessEnum.START;
            missionContext.SendAgvMisson(new_outmission);

        }
    }
}
