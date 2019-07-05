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
        NEW=0,

        //通知小车前往PICK等待点
        START = 1,

        AGVSTART = 2,

        //通知料库出库
        WHSTART = 3,

        //物料出库完成
        WHPICKED =4,

        //小车在出料道进入等待位等待
        AGVATPREPICK = 5,

        //小车在出料道等待
        AGVATPICK =6,

        //小车升降机构提升
        AGVPICKUP=7,

        //小车从出料道搬离物料并离开
        AGVPICKEDANDLEAVE = 8,

        //小车到达PLACE等待点
        AGVATPREPLACE=9,

        //进入单元入料道
        AGVATPLACE=10,

        //小车放置完成并离开
        AGVPLACEDANDLEAVE=11,

        //任务结束
        FINISHED=12,

        //任务撤销
        CANCEL = 13,

        //任务已撤销
        CANCELED = 14,

        //任务关闭
        CLOSE = 15,
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

        public AgvOutMissonProcessEnum Process { get; set; }
    }
}
