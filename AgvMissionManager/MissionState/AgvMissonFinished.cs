using Agv.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvMissionManager.MissionState
{
    /// <summary>
    ///  料库搬运入库完工处理=> 状态处理
    /// </summary>
    public class CarryMissonInFinished : IMissionState
    {
        private AgvInMissonModel currentMisson;
        private MissionContext missionContext;

        public CarryMissonInFinished(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            currentMisson = missionContext.MissionInNodes.FirstOrDefault(x => x.CarryProcess == CarryMissonProcessEnum.FINISHED && x.Process > AgvMissonProcessEnum.AGVPLACEDANDLEAVE);
            return currentMisson != null;

        }

        public string Condition()
        {
            return $"【料库搬运入库完成-判断条件】：CarryProcess=Finshed AND Process=AgvPlacedAndLeave";
        }

        public void Handle()
        {
            currentMisson.CarryProcess = CarryMissonProcessEnum.CLOSE;
            missionContext.SendAgvMisson(currentMisson);

        }
    }

    /// <summary>
    ///  料库搬运出库完工处理=> 状态处理
    /// </summary>
    public class CarryMissonOutFinished : IMissionState
    {
        private AgvOutMissonModel currentMisson;
        private MissionContext missionContext;

        public CarryMissonOutFinished(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            currentMisson = missionContext.MissionOutNodes.FirstOrDefault(x => x.CarryProcess == CarryMissonProcessEnum.FINISHED && x.Process > AgvMissonProcessEnum.AGVPICKEDANDLEAVE);
            return currentMisson != null;

        }

        public string Condition()
        {
            return $"【料库搬运出库完成-判断条件】：CarryProcess=Finshed AND Process=AgvPickedAndLeave";

        }

        public void Handle()
        {
            currentMisson.CarryProcess = CarryMissonProcessEnum.CLOSE;
            missionContext.SendAgvMisson(currentMisson);

        }
    }

    /// <summary>
    /// AGV入库完工处理=>状态处理
    /// </summary>
    public class AgvMissonInFinished : IMissionState
    {
        private AgvInMissonModel agvfinished_inmission;
        private MissionContext missionContext;

        public AgvMissonInFinished(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            agvfinished_inmission = missionContext.MissionInNodes.Where(x => x.Process == AgvMissonProcessEnum.FINISHED && x.CarryProcess > CarryMissonProcessEnum.NEW).FirstOrDefault();
            return agvfinished_inmission != null;

        }

        public string Condition()
        {
            return $"【AGV入库完成-判断条件】：CarryProcess=New AND Process=Finished";
        }

        public void Handle()
        {
            agvfinished_inmission.Process = AgvMissonProcessEnum.CLOSE;
            missionContext.SendAgvMisson(agvfinished_inmission);

        }
    }
    /// <summary>
    /// AGV出库完工处理=>状态处理
    /// </summary>
    public class AgvMissonOutFinished : IMissionState
    {
        private AgvOutMissonModel agvfinished_outmission;
        private MissionContext missionContext;

        public AgvMissonOutFinished(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            agvfinished_outmission = missionContext.MissionOutNodes.Where(x => x.Process == AgvMissonProcessEnum.FINISHED).FirstOrDefault();
            return agvfinished_outmission != null;

        }

        public string Condition()
        {
            return $"【AGV出库完成-判断条件】：Process=Finished";
        }

        public void Handle()
        {
            agvfinished_outmission.Process = AgvMissonProcessEnum.CLOSE;
            missionContext.SendAgvMisson(agvfinished_outmission);

        }
    }


}
