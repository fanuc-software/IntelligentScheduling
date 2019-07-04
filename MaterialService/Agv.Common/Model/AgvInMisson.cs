using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agv.Common
{
    public enum AgvInMissonProcessEnum
    {
        //未处理
        NEW = 0,

        //开始处理
        START = 1,

        AGVSTART=2,

        //小车在料道进入等待点等待
        AGVATPREPICK = 3,

        //小车在单元料道作业
        AGVATPICK = 4,
        
        //小车从单元料道搬离物料
        AGVPICKEDANDLEAVE = 5,

        //小车在入料道进入等待位等待
        AGVATPREPLACE = 6,

        //小车在料库入料道
        AGVATPLACE=7,

        //放置完成
        AGVPLACEDANDLEAVE = 8,

        //通知入库动作
        WHSTART=9,

        //入库结束
        FINISHED = 10,

        //任务撤销
        CANCEL =11,
        
        //任务已撤销
        CANCELED = 12,

        //任务关闭
        CLOSE =13,
    }

    [System.Runtime.Remoting.Contexts.Synchronization]

    public class AgvInMisson: System.ContextBoundObject
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

        public AgvInMissonProcessEnum Process { get; set; }
    }
}
