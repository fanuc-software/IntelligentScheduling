using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMaterialService
{
    public enum RightMaterialInMissonProcessEnum
    {
        //未处理
        NEW = 0,

        //开始处理
        START = 1,

        //通知小车前往PICK
        AGVSTART = 2,
        
        //小车从单元出料道搬离物料
        AGVLEAVEPICK = 3,

        //小车准备进入料库入料道
        BEFOREPLACE = 4,

        //小车进入料库入料道
        INPLACE=5,

        //放置完成
        PLACEANDLEAVE = 6,

        //通知入库动作
        WHSTART=7,

        //入库结束
        FINISHED = 8,

        //任务撤销
        CANCEL =9,
        
        //任务已撤销
        CANCELED = 10,

        //任务关闭
        CLOSE =11,
    }

    [System.Runtime.Remoting.Contexts.Synchronization]

    public class RightMaterialInMisson: System.ContextBoundObject
    {
        public string Id { get; set; }

        public string TimeId { get; set; }

        public RightMaterialMissionTypeEnum Type { get; set; }

        public string PickStationId { get; set; }

        public string PlaceStationId { get; set; }

        public string ProductId { get; set; }

        public string MaterialId { get; set; }

        public int Quantity { get; set; }

        public RightMaterialInMissonProcessEnum Process { get; set; }
    }
}
