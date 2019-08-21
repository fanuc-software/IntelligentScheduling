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
        [System.Runtime.Serialization.EnumMember(Value = @"NEW")]
        NEW = 0,

        /// <summary>
        /// 通知小车前往PICK等待点
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"START")]
        START = 1,


        /// <summary>
        /// 小车前往PICK等待点
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"AGVSTART")]
        AGVSTART = 2,


        /// <summary>
        /// 小车在出料道进入等待位等待
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"AGVATPREPICK")]
        AGVATPREPICK = 3,


        /// <summary>
        /// 小车在出料道
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"AGVATPICK")]
        AGVATPICK = 4,


        /// <summary>
        /// 小车从出料道搬离物料并离开
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"AGVPICKEDANDLEAVE")]
        AGVPICKEDANDLEAVE = 5,


        /// <summary>
        /// 小车到达PLACE等待点
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"AGVATPREPLACE")]
        AGVATPREPLACE = 6,


        /// <summary>
        /// 进入单元入料道
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"AGVATPLACE")]
        AGVATPLACE = 7,


        /// <summary>
        /// 小车放置完成并离开
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"AGVPLACEDANDLEAVE")]
        AGVPLACEDANDLEAVE = 8,


        /// <summary>
        /// 任务结束
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"FINISHED")]
        FINISHED = 9,


        /// <summary>
        /// 任务撤销
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"CANCEL")]
        CANCEL = 10,


        /// <summary>
        /// 任务已撤销
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"CANCELED")]
        CANCELED = 11,


        /// <summary>
        /// 任务关闭
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"CLOSE")]
        CLOSE = 12,
    }

    public enum CarryMissonProcessEnum
    {

        /// <summary>
        /// 未处理
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"NEW")]
        NEW = 0,


        /// <summary>
        /// 通知料库出库
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"WHSTART")]
        WHSTART = 1,


        /// <summary>
        /// 物料出库完成
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"WHPICKED")]
        WHPICKED = 2,


        /// <summary>
        /// 出库任务结束
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"FINISHED")]
        FINISHED = 3,


        /// <summary>
        /// 出库任务撤销
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"CANCEL")]
        CANCEL = 4,


        /// <summary>
        /// 出库任务已撤销
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"CANCELED")]
        CANCELED = 5,


        /// <summary>
        /// 出库任务关闭
        /// </summary>
        [System.Runtime.Serialization.EnumMember(Value = @"CLOSE")]
        CLOSE = 6,
    }
    public abstract class AgvMissonModel
    {
        public string Id { get; set; }

        public string TimeId { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

        public AgvStationEnum ClientId { get; set; }
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

        public AgvMissionTypeEnum Type { get; set; }
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

        public AgvStationEnum PickStationId { get; set; }
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

        public AgvStationEnum PlaceStationId { get; set; }

        public string ProductId { get; set; }

        public string MaterialId { get; set; }

        public int Quantity { get; set; }

        public DateTime CreateDateTime { get; set; }


        private AgvMissonProcessEnum process;
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

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
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

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
