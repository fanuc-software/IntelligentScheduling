using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeviceAsset;

namespace RightMaterialService
{
    public class BaseRightMaterialService
    {
        private SeerRoboRoute seerRoboRoute;

        private static BaseRightMaterialService _instance = null;
        private CancellationTokenSource token = new CancellationTokenSource();

        #region 任务
        private List<RightMaterialOutMisson> OutMissions { get; set; }
        private List<RightMaterialInMisson> InMissions { get; set; }
        
        public event Action<RightMaterialInMisson> UpdateRightMaterialInMissonEvent;

        public event Action<RightMaterialOutMisson> UpdateRightMaterialOutMissonEvent;

        #endregion
        
        #region ctor
        public static BaseRightMaterialService CreateInstance()
        {
            if (_instance == null)

            {
                _instance = new BaseRightMaterialService();
            }
            return _instance;
        }

        public BaseRightMaterialService()
        {
            OutMissions = new List<RightMaterialOutMisson>();
            InMissions = new List<RightMaterialInMisson>();

            seerRoboRoute = SeerRoboRoute.CreateInstance();
            //seerRoboRoute.

            #region 初始化任务
            seerRoboRoute.OnInitial();

            OutMissions.Clear();
            InMissions.Clear();

            #endregion
        }

        #endregion

        public void PushOutMission(RightMaterialOutMisson mission)
        {
            if (OutMissions.Where(x => x.Id == mission.Id).Count() == 0)
            {
                OutMissions.Add(mission);
            }
        }

        public void PushInMission(RightMaterialInMisson mission)
        {
            if (InMissions.Where(x => x.Id == mission.Id).Count() == 0)
            {
                InMissions.Add(mission);
            }
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    var ret = CheckMissionConflict();
                    if(ret==false)
                    {
                        while (ret == false)
                        {
                            ret = SetAlarm(true);
                            SendStationClientStateMessage(
                                new StationClientState { State = StationClientStateEnum.ERROR, Message = "物料调用失败,发送错误信息至设备!" });
                            Thread.Sleep(1000);
                        }

                        bool dev_reset = false;
                        while (dev_reset == false)
                        {
                            stationDevice.GetReset(ref dev_reset);
                            SendStationClientStateMessage(
                                new StationClientState { State = StationClientStateEnum.INFO, Message = "物料调用失败,等待设备的复位信号" });
                            Thread.Sleep(1000);
                        }

                    }

                    //没有料库入库任务
                    if (InMissions.Count()==0)
                    {
                        var new_inmission = InMissions.Where(x => x.Process == RightMaterialInMissonProcessEnum.NEW).FirstOrDefault();
                        if (new_inmission != null) new_inmission.Process = RightMaterialInMissonProcessEnum.START;
                        SendInMissionToAgvRoboRoute(new_inmission);
                    }

                }
            }, token.Token);
        }

        //TODO:
        public void Stop()
        {

        }

        //TODO:
        public void Suspend()
        {

        }

        private void UpdataRightMaterialInMisson()
        {

        }

        private void AgvOrderStateEvent()
        {

        }

        private bool CheckMissionConflict()
        {
            var undo_inmissions = InMissions
                .Where(x => x.Process > RightMaterialInMissonProcessEnum.NEW && x.Process < RightMaterialInMissonProcessEnum.FINISHED);
            var undo_outmissions = OutMissions
                .Where(x => x.Process > RightMaterialOutMissonProcessEnum.NEW && x.Process < RightMaterialOutMissonProcessEnum.FINISHED);

            //存在相同ID的任务
            var max_inmission = undo_inmissions.GroupBy(x => x.Id).Select(x => new { num = x.Count() }).Max() ?? new { num = 0 };
            var max_outmission = undo_outmissions.GroupBy(x => x.Id).Select(x => new { num = x.Count() }).Max() ?? new { num = 0 };
            if (max_inmission.num > 1 || max_outmission.num > 1) return false;

            //多于2个任务正在执行
            var count_inmissions = undo_inmissions.Count();
            var count_outmissions = undo_outmissions.Count();
            if (count_inmissions + count_outmissions > 2) return false;

            //存在两个出料库任务，已经达到AGVSTART， 但是小于AGVLEAVEPICK
            var count_outmission_conflict = undo_outmissions
                .Where(x => x.Process >= RightMaterialOutMissonProcessEnum.AGVSTART && x.Process < RightMaterialOutMissonProcessEnum.AGVLEAVEPICK).Count();
            if (count_outmission_conflict > 1) return false;

            //只能有一个入料库任务
            var count_inmission_conflict = undo_inmissions.Count();
            if (count_inmission_conflict > 1) return false;

            return true;
        }

        //TODO:发送入库任务给小车调度中心
        private void SendInMissionToAgvRoboRoute(RightMaterialInMisson mission)
        {

        }

        //TODO:发送错误消息给系统
        private bool SetAlarm(bool alarm)
        {
            return true;
        }

        //TODO:从系统获得复位信号
        private bool GetReset(ref bool reset)
        {
            return true;
        }
    }
}
