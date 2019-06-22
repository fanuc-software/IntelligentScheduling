using NLog;
using OrderDistribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FANUC.Host.Service
{
    public class DeviceService : IMonitorService
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        OrderDistributionService srv;

        public event Action<string> ShowMessageEvent;

        public DeviceService()
        {
            Redis.RedisHelper<OrderItem> redisHelper = new Redis.RedisHelper<OrderItem>();
            redisHelper.DeleteAll();
            logger.Info("Redis DeleteAll Is Done!");

            srv = new OrderDistributionService(OrderServiceEnum.OrderDistribution);
            srv.GetFirstOrderEvent += Srv_GetFirstOrderEvent;
            srv.OrderStateChangeEvent += Srv_OrderStateChangeEvent;
            srv.UpdateOrderActualQuantityEvent += Srv_UpdateOrderActualQuantityEvent;
            srv.SendOrderServiceStateMessage += (s) => Console.WriteLine(s);


        }

        public void Start()
        {
            srv.Start();
            logger.Info("OrderDistributionService is Start");

            ShowMessageEvent?.Invoke("OrderDistributionService 运行中........");
            ShowMessageEvent?.Invoke("输入 【exit】 退出........");

        }



        private void Srv_UpdateOrderActualQuantityEvent(OrderServiceEnum arg1, int arg2)
        {
            Redis.RedisHelper<OrderItem> redisHelper = new Redis.RedisHelper<OrderItem>();
            var obj = redisHelper.GetAll().Where(d => d.State == OrderItemStateEnum.DOWORK).OrderBy(d => d.CreateDateTime).FirstOrDefault();
            if (obj != null && obj.ActualQuantity != arg2)
            {
                obj.ActualQuantity = arg2;
                redisHelper.Update(obj);
                redisHelper.Push(obj.Id);
                logger.Info("【UpdateOrderActualQuantity】" + obj.ToString());
                ShowMessageEvent?.Invoke("【UpdateOrderActualQuantity】" + obj.ToString());
            }

        }

        private void Srv_OrderStateChangeEvent(OrderItem arg1, OrderServiceEnum arg2, int arg3)
        {
            Redis.RedisHelper<OrderItem> redisHelper = new Redis.RedisHelper<OrderItem>();

            if (arg1 == null)
            {
                var count = redisHelper.GetAll().Count(d => d.State == OrderItemStateEnum.DOWORK);
                if (count >= 2)
                {
                    var obj = redisHelper.GetAll().Where(d => d.State == OrderItemStateEnum.DOWORK).OrderBy(d => d.CreateDateTime).FirstOrDefault();

                    if (obj != null)
                    {
                        obj.State = OrderItemStateEnum.DONE;
                        redisHelper.Update(obj);
                        redisHelper.Push(obj.Id);
                        logger.Info("【OrderStateChange】=>DoWork=>DONE" + obj.ToString());
                        ShowMessageEvent?.Invoke("【OrderStateChange】=>DoWork=>DONE" + obj.ToString());

                    }
                }
                return;
            }
            redisHelper.Update(arg1);
            redisHelper.Push(arg1.Id);
            logger.Info("【OrderStateChange】=" + arg1.ToString());
            ShowMessageEvent?.Invoke("【OrderStateChange】" + arg1.ToString());

        }

        private OrderItem Srv_GetFirstOrderEvent(OrderServiceEnum arg)
        {
            Redis.RedisHelper<OrderItem> redisHelper = new Redis.RedisHelper<OrderItem>();
            var obj = redisHelper.GetAll().Where(d => d.State == OrderItemStateEnum.NEW).OrderBy(d => d.CreateDateTime).FirstOrDefault();
            if (obj != null)
            {
                logger.Info("【GetFirstOrder】" + obj.ToString());
                ShowMessageEvent?.Invoke("【GetFirstOrder】" + obj.ToString());

            }

            return obj;
        }
    }
}
