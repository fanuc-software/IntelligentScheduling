using Agv.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvMissionManager.MissionState
{
    /// <summary>
    /// 料库搬运入库异常处理
    /// </summary>
    public class CarryMissonInCancel : IMissionState
    {
        private AgvInMissonModel carrycancel_inmission;
        private MissionContext missionContext;

        public CarryMissonInCancel(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            carrycancel_inmission = missionContext.MissionInNodes.Where(x => x.CarryProcess == CarryMissonProcessEnum.CANCEL).FirstOrDefault();
            return carrycancel_inmission != null;

        }

        public string Condition()
        {
            return $"【料库搬运入库异常处理-判断条件】：CarryProcess=Cancel";

        }

        public void Handle()
        {
            carrycancel_inmission.CarryProcess = CarryMissonProcessEnum.CANCELED;
            //TODO:添加异常处理

        }
    }
    /// <summary>
    /// 料库搬运出库异常处理
    /// </summary>
    public class CarryMissonOutCancel : IMissionState
    {
        private AgvOutMissonModel carrycancel_outmission;
        private MissionContext missionContext;

        public CarryMissonOutCancel(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            carrycancel_outmission = missionContext.MissionOutNodes.Where(x => x.CarryProcess == CarryMissonProcessEnum.CANCEL).FirstOrDefault();
            return carrycancel_outmission != null;

        }

        public string Condition()
        {
            return $"【料库搬运出库异常处理-判断条件】：CarryProcess=Cancel";
        }

        public void Handle()
        {
            carrycancel_outmission.CarryProcess = CarryMissonProcessEnum.CANCELED;
            // 添加异常处理
        }
    }

    /// <summary>
    /// AGV入库异常处理
    /// </summary>
    public class AgvMissonInCancel : IMissionState
    {
        private AgvInMissonModel agvcancel_inmission;
        private MissionContext missionContext;

        public AgvMissonInCancel(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            agvcancel_inmission = missionContext.MissionInNodes.Where(x => x.Process == AgvMissonProcessEnum.CANCEL).FirstOrDefault();
            return agvcancel_inmission != null;

        }

        public string Condition()
        {
            return $"【AGV入库异常处理-判断条件】：CarryProcess=Cancel";

        }

        public void Handle()
        {
            agvcancel_inmission.Process = AgvMissonProcessEnum.CANCELED;
            // 添加异常处理
        }
    }

    /// <summary>
    /// AGV出库异常处理
    /// </summary>
    public class AgvMissonOutCancel : IMissionState
    {
        private AgvOutMissonModel agvcancel_outmission;
        private MissionContext missionContext;

        public AgvMissonOutCancel(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            agvcancel_outmission = missionContext.MissionOutNodes.Where(x => x.Process == AgvMissonProcessEnum.CANCEL).FirstOrDefault();
            return agvcancel_outmission != null;

        }

        public string Condition()
        {
            return $"【AGV出库异常处理-判断条件】：CarryProcess=Cancel";
        }

        public void Handle()
        {
            agvcancel_outmission.Process = AgvMissonProcessEnum.CANCELED;
            // 添加异常处理
        }
    }
}
