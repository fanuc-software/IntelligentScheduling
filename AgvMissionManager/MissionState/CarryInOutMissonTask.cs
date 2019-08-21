using Agv.Common.Model;
using RightCarryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvMissionManager.MissionState
{
    /// <summary>
    /// 料库入库动作
    /// </summary>
    public class CarryInMissonTask : IMissionState
    {
        private AgvInMissonModel warehouse_inmission;
        private MissionContext missionContext;

        public CarryInMissonTask(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            warehouse_inmission = missionContext.undo_inmissions.Where(x => x.Process == AgvMissonProcessEnum.FINISHED && x.CarryProcess == CarryMissonProcessEnum.NEW).FirstOrDefault();
            return warehouse_inmission != null;

        }

        public string Condition()
        {
            return $"【料库入库动作-判断条件】：CarryProcess=New And Process=Finished";
        }

        public void Handle()
        {
            warehouse_inmission.CarryProcess = CarryMissonProcessEnum.WHSTART;
            missionContext.SendAgvMisson(warehouse_inmission);

            missionContext.DoWork(() => WareHouseInMission(warehouse_inmission), () =>
            {
                //TODO:添加料库入库错误后的动作


            }, "料库入库动作失败", AgvMissionServiceErrorCodeEnum.WAREHOUSEIN);
        }

        //料库执行入库
        private bool WareHouseInMission(AgvInMissonModel mission)
        {
            TestRightCarryService<IControlDevice> carry = new TestRightCarryService<IControlDevice>(missionContext.carryDevice);

            var ret = carry.CarryIn(mission.ProductId, mission.MaterialId);

            if (ret == false)
            {
                mission.Process = AgvMissonProcessEnum.CANCEL;
                mission.CarryProcess = CarryMissonProcessEnum.CANCEL;
                carry.ReleaseLock();
                return false;
            }

            mission.CarryProcess = CarryMissonProcessEnum.FINISHED;
            carry.ReleaseLock();
            return true;
        }
    }

    /// <summary>
    /// 料库出库动作
    /// </summary>
    public class CarryOutMissonTask : IMissionState
    {
        private AgvOutMissonModel warehouse_outmission;
        private AgvOutMissonModel warehouse_outmission_last;
        private MissionContext missionContext;

        public CarryOutMissonTask(MissionContext _missionContext)
        {
            missionContext = _missionContext;
        }
        public bool CanRequest()
        {
            warehouse_outmission = missionContext.undo_outmissions.Where(x => x.CarryProcess == CarryMissonProcessEnum.NEW).FirstOrDefault();
            warehouse_outmission_last = missionContext.undo_outmissions.Where(x => x.CarryProcess > CarryMissonProcessEnum.NEW
              && x.Process < AgvMissonProcessEnum.AGVPICKEDANDLEAVE).FirstOrDefault();
            //TODO:增加料道信号检查
            return warehouse_outmission != null && warehouse_outmission_last == null;

        }

        public string Condition()
        {
            return $"【料库出库动作-判断条件】：CarryProcess[1]=New And CarryProcess[2]>New AND Process[2]<AgvPickedAndLeave";
        }

        public void Handle()
        {
            warehouse_outmission.CarryProcess = CarryMissonProcessEnum.WHSTART;
            missionContext.SendAgvMisson(warehouse_outmission);

            missionContext.DoWork(() => WareHouseOutMission(warehouse_outmission), () =>
            {
                //TODO:添加料库出库失败动作

            }, "料库出库动作失败", AgvMissionServiceErrorCodeEnum.WAREHOUSEOUT);
        }

        //料库执行出库
        private bool WareHouseOutMission(AgvOutMissonModel mission)
        {

            TestRightCarryService<IControlDevice> carry = new TestRightCarryService<IControlDevice>(missionContext.carryDevice);

            int quantity = 0;
            var ret = carry.CarryOut(mission.ProductId, mission.MaterialId, ref quantity);

            if (ret == false)
            {
                mission.Process = AgvMissonProcessEnum.CANCEL;
                mission.CarryProcess = CarryMissonProcessEnum.CANCEL;
                carry.ReleaseLock();
                return false;
            }

            mission.CarryProcess = CarryMissonProcessEnum.FINISHED;
            carry.ReleaseLock();
            return true;
        }
    }
}
