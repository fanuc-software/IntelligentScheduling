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

        //放置完成离开
        PLACED = 5,

        //入库结束
        FINISHED = 6,
    }

    public class RightMaterialInMisson
    {
        public string Id { get; set; }

        public RightMaterialMissionTypeEnum Type { get; set; }

        public string PickStationId { get; set; }

        public string PlaceStationId { get; set; }

        public string ProductId { get; set; }

        public string MaterialId { get; set; }

        public int Quantity { get; set; }

        public RightMaterialInMissonProcessEnum Process { get; set; }
    }
}
