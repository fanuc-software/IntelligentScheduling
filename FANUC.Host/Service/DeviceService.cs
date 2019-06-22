using NLog;
using OrderDistribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FANUC.Host.Service
{
    public class DeviceService : BaseHostService
    {
        public override BaseOrderService GetOrderServce => new OrderDistributionService(OrderServiceEnum.OrderDistribution);
    }

    public class FanucDeviceService : BaseHostService
    {
        public override BaseOrderService GetOrderServce => new OrderRobotDistributionService(OrderServiceEnum.OrderDistribution);
    }
}
