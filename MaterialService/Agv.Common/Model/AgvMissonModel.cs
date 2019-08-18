using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agv.Common.Model
{
    public enum AgvMissonProcessEnum
    {
        
        /// <summary>
        /// 未处理
        /// </summary>
        NEW = 0,

        /// <summary>
        /// 通知小车前往PICK等待点
        /// </summary>
        START = 1,

       
        /// <summary>
        /// 小车前往PICK等待点
        /// </summary>
        AGVSTART = 2,

       
        /// <summary>
        /// 小车在出料道进入等待位等待
        /// </summary>
        AGVATPREPICK = 3,

       
        /// <summary>
        /// 小车在出料道
        /// </summary>
        AGVATPICK = 4,

       
        /// <summary>
        /// 小车从出料道搬离物料并离开
        /// </summary>
        /// 
        AGVPICKEDANDLEAVE = 5,

     
        /// <summary>
        /// 小车到达PLACE等待点
        /// </summary>
        AGVATPREPLACE = 6,

      
        /// <summary>
        /// 进入单元入料道
        /// </summary>
        AGVATPLACE = 7,

        
        /// <summary>
        /// 小车放置完成并离开
        /// </summary>
        AGVPLACEDANDLEAVE = 8,

       
        /// <summary>
        /// 任务结束
        /// </summary>
        FINISHED = 9,

      
        /// <summary>
        /// 任务撤销
        /// </summary>
        CANCEL = 10,

        
        /// <summary>
        /// 任务已撤销
        /// </summary>
        CANCELED = 11,

       
        /// <summary>
        /// 任务关闭
        /// </summary>
        CLOSE = 12,
    }

    public enum CarryMissonProcessEnum
    {
        
        /// <summary>
        /// 未处理
        /// </summary>
        NEW = 0,

       
        /// <summary>
        /// 通知料库出库
        /// </summary>
        WHSTART = 1,

      
        /// <summary>
        /// 物料出库完成
        /// </summary>
        WHPICKED = 2,

        
        /// <summary>
        /// 出库任务结束
        /// </summary>
        FINISHED = 3,

       
        /// <summary>
        /// 出库任务撤销
        /// </summary>
        CANCEL = 4,

       
        /// <summary>
        /// 出库任务已撤销
        /// </summary>
        CANCELED = 5,

        
        /// <summary>
        /// 出库任务关闭
        /// </summary>
        CLOSE = 6,
    }
    public abstract class AgvMissonModel
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


        private AgvMissonProcessEnum process;

        public AgvMissonProcessEnum Process
        {
            get { return process; }
            set
            {
                if (process != value)
                {
                    process = value;
                    AgvProcessChangeEvent?.Invoke(this, false);
                }

            }
        }

        private CarryMissonProcessEnum carryProcess;

        public CarryMissonProcessEnum CarryProcess
        {
            get { return carryProcess; }
            set
            {
                if (carryProcess != value)
                {
                    carryProcess = value;
                    AgvProcessChangeEvent?.Invoke(this, false);
                }

            }
        }

        public event Action<AgvMissonModel, bool> AgvProcessChangeEvent;
    }

    public class AgvInMissonModel : AgvMissonModel
    { }

    public class AgvOutMissonModel : AgvMissonModel
    { }
}
