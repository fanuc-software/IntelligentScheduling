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

        //通知料库出库
        WHSTART = 2,

        //物料出库完成
        WHPICKED =3,

        //小车在出料道进入等待位等待
        AGVATPREPICK = 4,

        //小车在出料道等待
        AGVATPICK =5,

        //小车升降机构提升
        AGVPICKUP=6,

        //小车从出料道搬离物料并离开
        AGVPICKEDANDLEAVE = 7,

        //小车到达PLACE等待点
        AGVATPREPLACE=8,

        //进入单元入料道
        AGVATPLACE=9,

        //小车放置完成并离开
        AGVPLACEDANDLEAVE=10,

        //任务结束
        FINISHED=11,

        //任务撤销
        CANCEL = 12,

        //任务已撤销
        CANCELED = 13,

        //任务关闭
        CLOSE = 14,
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
