using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DeviceAsset;

namespace RightMaterialService
{
    public class BaseRightMaterialMissionService
    {
        private SeerRoboRoute seerRoboRoute;
        private IControlDevice controlDevice;

        private static BaseRightMaterialMissionService _instance = null;
        private CancellationTokenSource token = new CancellationTokenSource();

        #region 任务
        private List<RightMaterialOutMisson> OutMissions { get; set; }
        private List<RightMaterialInMisson> InMissions { get; set; }
        
        #endregion

        #region ctor
        public static BaseRightMaterialMissionService CreateInstance()
        {
            if (_instance == null)

            {
                _instance = new BaseRightMaterialMissionService();
            }
            return _instance;
        }

        public BaseRightMaterialMissionService()
        {
            OutMissions = new List<RightMaterialOutMisson>();
            InMissions = new List<RightMaterialInMisson>();

            seerRoboRoute = SeerRoboRoute.CreateInstance();
            controlDevice = new AllenBradleyControlDevice();

            //seerRoboRoute.

            #region 初始化任务
            seerRoboRoute.OnInitial();

            OutMissions.Clear();
            InMissions.Clear();

            #endregion
        }

        #endregion

        #region 外部接口
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

#pragma warning disable CS0067 // 从不使用事件“BaseRightMaterialMissionService.UpdateRightMaterialInMissonEvent”
        public event Action<RightMaterialInMisson> UpdateRightMaterialInMissonEvent;
#pragma warning restore CS0067 // 从不使用事件“BaseRightMaterialMissionService.UpdateRightMaterialInMissonEvent”

#pragma warning disable CS0067 // 从不使用事件“BaseRightMaterialMissionService.UpdateRightMaterialOutMissonEvent”
        public event Action<RightMaterialOutMisson> UpdateRightMaterialOutMissonEvent;
#pragma warning restore CS0067 // 从不使用事件“BaseRightMaterialMissionService.UpdateRightMaterialOutMissonEvent”

        #endregion

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    //bool ret=false;
                    ////防冲突
                    ////var ret = CheckMissionConflict();
                    //if (ret == false)
                    //{
                    //    while (ret == false)
                    //    {
                    //        //TODO:停止所有AGV小车动作


                    //        //设定报警
                    //        ret = SetAlarm(true);
                    //        //TODO:通知界面
                    //        //SendStationClientStateMessage(
                    //        //    new StationClientState { State = StationClientStateEnum.ERROR, Message = "物料调用失败,发送错误信息至设备!" });
                    //        Thread.Sleep(1000);
                    //    }

                    //    bool dev_reset = false;
                    //    while (dev_reset == false)
                    //    {
                    //        GetReset(ref dev_reset);
                    //        //TODO:通知界面
                    //        //SendStationClientStateMessage(
                    //        //    new StationClientState { State = StationClientStateEnum.INFO, Message = "物料调用失败,等待设备的复位信号" });
                    //        Thread.Sleep(1000);
                    //    }

                    //}

                    //var undo_inmissions = InMissions
                    //    .Where(x => x.Process > RightMaterialInMissonProcessEnum.NEW && x.Process < RightMaterialInMissonProcessEnum.CLOSE);
                    //var undo_outmissions = OutMissions
                    //    .Where(x => x.Process > RightMaterialOutMissonProcessEnum.NEW && x.Process < RightMaterialOutMissonProcessEnum.CLOSE);

                    ////小于两个料库入库任务 添加一个入库任务
                    //if (undo_inmissions.Count() < 2)
                    //{
                    //    var new_inmission = InMissions.Where(x => x.Process == RightMaterialInMissonProcessEnum.NEW).FirstOrDefault();
                    //    if (new_inmission != null) new_inmission.Process = RightMaterialInMissonProcessEnum.START;
                    //}

                    ////小于两个出料库任务 添加一个出库任务
                    //if (undo_outmissions.Count() < 2)
                    //{
                    //    var new_inmission = InMissions.Where(x => x.Process == RightMaterialInMissonProcessEnum.NEW).FirstOrDefault();
                    //    if (new_inmission != null) new_inmission.Process = RightMaterialInMissonProcessEnum.START;
                    //}

                    ////通知料库入库动作
                    //var warehouse_inmission = undo_inmissions.Where(x => x.Process == RightMaterialInMissonProcessEnum.PLACEANDLEAVE).FirstOrDefault();
                    //if (warehouse_inmission != null)
                    //{
                    //    warehouse_inmission.Process = RightMaterialInMissonProcessEnum.WHSTART;

                    //    Task.Factory.StartNew(async () =>
                    //    {
                    //        ret = await WareHouseInMission(warehouse_inmission);
                    //        if (ret == false)
                    //        {
                    //            warehouse_inmission.Process = RightMaterialInMissonProcessEnum.CANCEL;

                    //            while (ret == false)
                    //            {
                    //                ret = SetAlarm(true);
                    //                //TODO:
                    //                //SendStationClientStateMessage(
                    //                //    new StationClientState { State = StationClientStateEnum.ERROR, Message = "物料调用失败,发送错误信息至设备!" });
                    //                Thread.Sleep(1000);
                    //            }

                    //            bool dev_reset = false;
                    //            while (dev_reset == false)
                    //            {
                    //                GetReset(ref dev_reset);
                    //                //TODO:
                    //                //SendStationClientStateMessage(
                    //                //    new StationClientState { State = StationClientStateEnum.INFO, Message = "物料调用失败,等待设备的复位信号" });
                    //                Thread.Sleep(1000);
                    //            }

                    //        }
                    //    });

                    //}

                    ////通知料库出库动作
                    //var warehouse_outmission = undo_outmissions.Where(x => x.Process == RightMaterialOutMissonProcessEnum.START).FirstOrDefault();
                    //if (warehouse_outmission != null)
                    //{
                    //    //warehouse_outmission.Process = RightMaterialOutMissonProcessEnum.WHSTART;

                    //    Task.Factory.StartNew(async () =>
                    //    {
                    //        ret = await WareHouseOutMission(warehouse_outmission);

                    //        if (ret == false)
                    //        {
                    //            warehouse_outmission.Process = RightMaterialOutMissonProcessEnum.CANCEL;

                    //            while (ret == false)
                    //            {
                    //                ret = SetAlarm(true);
                    //                //TODO:
                    //                //SendStationClientStateMessage(
                    //                //    new StationClientState { State = StationClientStateEnum.ERROR, Message = "物料调用失败,发送错误信息至设备!" });
                    //                Thread.Sleep(1000);
                    //            }

                    //            bool dev_reset = false;
                    //            while (dev_reset == false)
                    //            {
                    //                GetReset(ref dev_reset);
                    //                //TODO:
                    //                //SendStationClientStateMessage(
                    //                //    new StationClientState { State = StationClientStateEnum.INFO, Message = "物料调用失败,等待设备的复位信号" });
                    //                Thread.Sleep(1000);
                    //            }

                    //        }
                    //    });

                    //}

                    ////通知小车搬运入库
                    //var agv_inmission = undo_inmissions.Where(x => x.Process == RightMaterialInMissonProcessEnum.START).FirstOrDefault();
                    //if (agv_inmission != null)
                    //{
                    //    agv_inmission.Process = RightMaterialInMissonProcessEnum.AGVSTART;

                    //    Task.Factory.StartNew(async () =>
                    //    {
                    //        ret = await AgvInMission(agv_inmission);
                    //        if (ret == false)
                    //        {
                    //            agv_inmission.Process = RightMaterialInMissonProcessEnum.CANCEL;

                    //            while (ret == false)
                    //            {
                    //                ret = SetAlarm(true);
                    //                //TODO:
                    //                //SendStationClientStateMessage(
                    //                //    new StationClientState { State = StationClientStateEnum.ERROR, Message = "物料调用失败,发送错误信息至设备!" });
                    //                Thread.Sleep(1000);
                    //            }

                    //            bool dev_reset = false;
                    //            while (dev_reset == false)
                    //            {
                    //                GetReset(ref dev_reset);
                    //                //TODO:
                    //                //SendStationClientStateMessage(
                    //                //    new StationClientState { State = StationClientStateEnum.INFO, Message = "物料调用失败,等待设备的复位信号" });
                    //                Thread.Sleep(1000);
                    //            }

                    //        }
                    //    });


                    //}

                    ////通知小车出库搬运
                    //var agv_outmission = undo_outmissions.Where(x => x.Process == RightMaterialOutMissonProcessEnum.PICKED).FirstOrDefault();
                    //if (agv_outmission != null)
                    //{
                    //    agv_outmission.Process = RightMaterialOutMissonProcessEnum.AGVSTART;

                    //    Task.Factory.StartNew(async () =>
                    //    {
                    //        ret = await AgvOutMission(agv_outmission);
                    //        if (ret == false)
                    //        {
                    //            agv_outmission.Process = RightMaterialOutMissonProcessEnum.CANCEL;

                    //            while (ret == false)
                    //            {
                    //                ret = SetAlarm(true);
                    //                //TODO:
                    //                //SendStationClientStateMessage(
                    //                //    new StationClientState { State = StationClientStateEnum.ERROR, Message = "物料调用失败,发送错误信息至设备!" });
                    //                Thread.Sleep(1000);
                    //            }

                    //            bool dev_reset = false;
                    //            while (dev_reset == false)
                    //            {
                    //                GetReset(ref dev_reset);
                    //                //TODO:
                    //                //SendStationClientStateMessage(
                    //                //    new StationClientState { State = StationClientStateEnum.INFO, Message = "物料调用失败,等待设备的复位信号" });
                    //                Thread.Sleep(1000);
                    //            }

                    //        }
                    //    });
                    //}

                    ////入库完工处理
                    //var finished_inmission = InMissions.Where(x => x.Process == RightMaterialInMissonProcessEnum.FINISHED).FirstOrDefault();
                    //if (finished_inmission != null)
                    //{
                    //    finished_inmission.Process = RightMaterialInMissonProcessEnum.CLOSE;
                    //}

                    ////出库完工处理
                    //var finished_outmission = OutMissions.Where(x => x.Process == RightMaterialOutMissonProcessEnum.FINISHED).FirstOrDefault();
                    //if (finished_outmission != null)
                    //{
                    //    finished_outmission.Process = RightMaterialOutMissonProcessEnum.CLOSE;
                    //}

                    ////入库异常处理
                    //var cancel_inmission = InMissions.Where(x => x.Process == RightMaterialInMissonProcessEnum.CANCEL).FirstOrDefault();
                    //if (cancel_inmission != null)
                    //{
                    //    cancel_inmission.Process = RightMaterialInMissonProcessEnum.CANCELED;
                    //    AgvInMissionCancel(cancel_inmission);
                    //}

                    ////出库异常处理
                    //var cancel_outmission = OutMissions.Where(x => x.Process == RightMaterialOutMissonProcessEnum.CANCELED).FirstOrDefault();
                    //if (cancel_outmission != null)
                    //{
                    //    cancel_outmission.Process = RightMaterialOutMissonProcessEnum.CLOSE;
                    //    AgvOutMissionCancel(cancel_outmission);
                    //}
                }
            }, token.Token);
        }
        
        public bool CloseOutMission(string missionId)
        {
            var finished_outmission = OutMissions.Where(x => x.Id==missionId && x.Process == RightMaterialOutMissonProcessEnum.FINISHED).FirstOrDefault();
            if (finished_outmission != null)
            {
                finished_outmission.Process = RightMaterialOutMissonProcessEnum.CLOSE;
            }

            return true;
        }

        public bool CloseInMission(string missionId)
        {
            var finished_inmission = InMissions.Where(x => x.Id == missionId && x.Process == RightMaterialInMissonProcessEnum.FINISHED).FirstOrDefault();
            if (finished_inmission != null)
            {
                finished_inmission.Process = RightMaterialInMissonProcessEnum.CLOSE;
            }

            return true;
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

        private Tuple<bool, RightMaterialMissionServiceErrorCodeEnum> CheckMissionConflict()
        {
            var undo_inmissions = InMissions
                .Where(x => x.Process > RightMaterialInMissonProcessEnum.NEW && x.Process < RightMaterialInMissonProcessEnum.CLOSE);
            var undo_outmissions = OutMissions
                .Where(x => x.Process > RightMaterialOutMissonProcessEnum.NEW && x.Process < RightMaterialOutMissonProcessEnum.CLOSE);

            //存在相同ID的任务
            var max_inmission = undo_inmissions.GroupBy(x => x.Id).Select(x => new { num = x.Count() }).Max() ?? new { num = 0 };
            var max_outmission = undo_outmissions.GroupBy(x => x.Id).Select(x => new { num = x.Count() }).Max() ?? new { num = 0 };
            if (max_inmission.num > 1 || max_outmission.num > 1)
            {
                return new Tuple<bool, RightMaterialMissionServiceErrorCodeEnum>
                    (false, RightMaterialMissionServiceErrorCodeEnum.IDCONFLICT);
            }

            //多于2个任务正在执行
            var count_inmissions = undo_inmissions.Count();
            var count_outmissions = undo_outmissions.Count();
            if (count_inmissions + count_outmissions > 2)
            {
                return new Tuple<bool, RightMaterialMissionServiceErrorCodeEnum>
                    (false, RightMaterialMissionServiceErrorCodeEnum.QUANTITYLIMIT);
            }

            //存在两个出料库任务，已经达到AGVSTART， 但是小于AGVLEAVEPICK
            var count_outmission_conflict = undo_outmissions
                .Where(x => x.Process >= RightMaterialOutMissonProcessEnum.AGVSTART && x.Process < RightMaterialOutMissonProcessEnum.AGVLEAVEPICK).Count();
            if (count_outmission_conflict > 1)
            {
                return new Tuple<bool, RightMaterialMissionServiceErrorCodeEnum>
                    (false, RightMaterialMissionServiceErrorCodeEnum.IDCONFLICT);
            }

            //只能有一个入料库任务
            var count_inmission_conflict = undo_inmissions.Count();
            if (count_inmission_conflict > 1)
            {
                return new Tuple<bool, RightMaterialMissionServiceErrorCodeEnum>
    (false, RightMaterialMissionServiceErrorCodeEnum.IDCONFLICT);
            }

            return new Tuple<bool, RightMaterialMissionServiceErrorCodeEnum>
    (false, RightMaterialMissionServiceErrorCodeEnum.IDCONFLICT);
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

        //TODO:异步料库执行入库
#pragma warning disable CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        private async Task<bool> WareHouseInMission(RightMaterialInMisson mission)
#pragma warning restore CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        {
            int temp_type = 0;
            var ret = int.TryParse(mission.ProductId, out temp_type);
            if (ret == false) return ret;

            ret = controlDevice.SetRHouseFin(false);
            if (ret == false) return ret;

            ret = controlDevice.SetRHouseProductType(temp_type);
            if (ret == false) return ret;

            int temp_material = 0;
            ret = int.TryParse(mission.MaterialId, out temp_material);
            if (ret == false) return ret;

            ret = controlDevice.SetRHouseMaterialType(temp_material);
            if (ret == false) return ret;

            ret = controlDevice.SetRHouseInOut(false);
            if (ret == false) return ret;

            ret = controlDevice.SetRHouseRequest(true);
            if (ret == false) return ret;

            var in_fin = false;
            while (in_fin == false || ret == false)
            {
                ret = controlDevice.GetRHouseFin(ref in_fin);

                var in_reset = false;
                controlDevice.GetRHouseReset(ref in_reset);
                if (in_reset == true)
                {
                    break;
                }
            }

            mission.Process = RightMaterialInMissonProcessEnum.FINISHED;

            ret = controlDevice.SetRHouseRequest(false);
            if (ret == false) return ret;

            ret = controlDevice.SetRHouseFin(false);
            if (ret == false) return ret;


            return true;
        }

        //TODO:异步料库执行出库
#pragma warning disable CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        private async Task<bool> WareHouseOutMission(RightMaterialOutMisson mission)
#pragma warning restore CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        {
            int temp_type = 0;
            var ret = int.TryParse(mission.ProductId, out temp_type);
            if (ret == false) return ret;

            ret = controlDevice.SetRHouseFin(false);
            if (ret == false) return ret;

            ret = controlDevice.SetRHouseProductType(temp_type);
            if (ret == false) return ret;

            int temp_material = 0;
            ret = int.TryParse(mission.MaterialId, out temp_material);
            if (ret == false) return ret;

            ret = controlDevice.SetRHouseMaterialType(temp_material);
            if (ret == false) return ret;

            ret = controlDevice.SetRHouseInOut(true);
            if (ret == false) return ret;

            ret = controlDevice.SetRHouseRequest(true);
            if (ret == false) return ret;

            var in_fin = false;
            while (in_fin == false || ret == false)
            {
                ret = controlDevice.GetRHouseFin(ref in_fin);

                var in_reset = false;
                controlDevice.GetRHouseReset(ref in_reset);
                if (in_reset == true)
                {
                    break;
                }
            }

            mission.Process = RightMaterialOutMissonProcessEnum.PICKED;

            ret = controlDevice.SetRHouseRequest(false);
            if (ret == false) return ret;

            ret = controlDevice.SetRHouseFin(false);
            if (ret == false) return ret;

            return true;
        }

        //TODO:异步小车入库搬运
        private async Task<bool> AgvInMission(RightMaterialInMisson mission)
        {
            var agv_order_id = mission.Id + "_" + mission.TimeId;
            IClient client = new Client();
            await client.TransportOrders2Async(agv_order_id, new TransportOrder()
            {
                Deadline = DateTime.Now.AddMinutes(20),
                Destinations = new List<DestinationOrder>()
                {
                    new DestinationOrder(){ LocationName=mission.PickStationId,Operation="JackLoad"}
                }
            });

            await Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Task.Delay(2000);

                    var order = await client.TransportOrdersAsync(agv_order_id);
                    if (order.State == TransportOrderStateState.FINISHED)
                    {
                        mission.Process = RightMaterialInMissonProcessEnum.FINISHED;
                        break;
                    }
                    else if (order.State == TransportOrderStateState.WITHDRAWN || order.State == TransportOrderStateState.FAILED)
                    {
                        mission.Process = RightMaterialInMissonProcessEnum.CANCEL;
                        break;
                    }
                }
            });
            return true;
        }

        //TODO:异步小车出库搬运
#pragma warning disable CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        private async Task<bool> AgvOutMission(RightMaterialOutMisson mission)
#pragma warning restore CS1998 // 此异步方法缺少 "await" 运算符，将以同步方式运行。请考虑使用 "await" 运算符等待非阻止的 API 调用，或者使用 "await Task.Run(...)" 在后台线程上执行占用大量 CPU 的工作。
        {
            return true;
        }

        //TODO:小车入库任务取消
        private bool AgvInMissionCancel(RightMaterialInMisson mission)
        {

            return true;
        }

        //TODO:小车出库任务取消
        private bool AgvOutMissionCancel(RightMaterialOutMisson mission)
        {

            return true;
        }
    }
}
