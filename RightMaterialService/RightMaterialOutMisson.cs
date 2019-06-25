using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMaterialService
{
    public enum RightMaterialOutMissonProcessEnum
    {
        //未处理
        NEW=0,

        //开始处理
        START=1,

        //通知小车前往PICK等待点
        AGVTOPREPICK = 2,

        //通知料库出库
        WHPICK = 3,

        //物料出库完成
        WHPICKED =4,

        //通知小车进入出料道
        AGVTOPICK=5,

        //小车从出料道搬离物料并离开
        AGVPICKEDANDLEAVE = 6,

        //小车前往PLACE等待点
        AGVTOPREPLACE=7,

        //放置完成离开
        FINISHED=7,

        //任务撤销
        CANCEL = 8,

        //任务已撤销
        CANCELED = 9,

        //任务关闭
        CLOSE = 10,
    }

    public class RightMaterialOutMisson
    {
        public string Id { get; set; }

        public string TimeId { get; set; }

        public RightMaterialMissionTypeEnum Type { get; set; }

        public string PickStationId { get; set; }

        public string PlaceStationId { get; set; }

        public string ProductId { get; set; }

        public string MaterialId { get; set; }

        public int Quantity { get; set; }

        public RightMaterialOutMissonProcessEnum Process { get; set; }
    }
}
