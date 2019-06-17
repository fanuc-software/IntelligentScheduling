using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderDistribution
{

    //设备侧缓存一个订单
    //宁波项目--智能制造单元&工业实训单元
    //2019.06.11初始版本
    public class OrderDistributionService : BaseOrderService
    {

        public OrderDistributionService(OrderServiceEnum serviceEnum) : base(serviceEnum)
        {

        }

        //public override IOrderDevice Device => new AllenBradleyDevice();
    }
}
