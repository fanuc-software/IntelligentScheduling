using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agv.Common
{
    public enum AgvOutMissonProcessEnum
    {
        //未处理
        NEW = 0,

        //通知小车前往PICK等待点
        START = 1,

        //小车前往PICK等待点
        AGVSTART = 2,
        
        //小车在出料道进入等待位等待
        AGVATPREPICK = 3,

        //小车在出料道
        AGVATPICK = 4,

        //小车从出料道搬离物料并离开
        AGVPICKEDANDLEAVE = 5,

        //小车到达PLACE等待点
        AGVATPREPLACE = 6,

        //进入单元入料道
        AGVATPLACE = 7,

        //小车放置完成并离开
        AGVPLACEDANDLEAVE = 8,

        //任务结束
        FINISHED = 9,

        //任务撤销
        CANCEL = 10,

        //任务已撤销
        CANCELED = 11,

        //任务关闭
        CLOSE = 12,
    }

    public enum CarryOutMissonProcessEnum
    {
        //未处理
        NEW = 0,
        
        //通知料库出库
        WHSTART = 1,

        //物料出库完成
        WHPICKED = 2,

        //出库任务结束
        FINISHED = 3,

        //出库任务撤销
        CANCEL = 4,

        //出库任务已撤销
        CANCELED = 5,

        //出库任务关闭
        CLOSE = 6,
    }

    public class AgvOutMisson
    {
        public string Id { get; set; }

        public string TimeId { get; set; }

        public AgvStationEnum ClientId { get; set; }

        public AgvMissionTypeEnum Type { get; set; }

        public AgvStationEnum PickStationId { get; set; }

        public AgvStationEnum PlaceStationId { get; set; }

        public string ProductId { get; set; }

        public string MaterialId { get; set; }

        public int Quantity { get; set; }

        public DateTime CreateDateTime { get; set; }


        private AgvOutMissonProcessEnum process;

        public AgvOutMissonProcessEnum Process
        {
            get { return process; }
            set
            {
                if (process != value)
                {
                    process = value;
                    AgvOutProcessChangeEvent?.Invoke(this, false);
                }

            }
        }
        
        private CarryOutMissonProcessEnum carryProcess;

        public CarryOutMissonProcessEnum CarryProcess
        {
            get { return carryProcess; }
            set
            {
                if (carryProcess != value)
                {
                    carryProcess = value;
                    AgvOutProcessChangeEvent?.Invoke(this, false);
                }

            }
        }

        public event Action<AgvOutMisson, bool> AgvOutProcessChangeEvent;
    }
}
