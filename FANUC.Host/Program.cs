using OrderDistribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FANUC.Host
{
    class Program
    {

        static void Main(string[] args)
        {

            //  Device();
            MoitorTest();

            Console.ReadLine();
        }
        static void MoitorTest()
        {
            Task.Factory.StartNew(() =>
            {

                var host = new MonitorHost("Task1");
                host.ConsoleInfoEvent += (s) => Console.WriteLine(s);
                host.StepA();
                host.StepB();
                host.StepC();
             
            });

            Task.Factory.StartNew(() =>
            {

                var host = new MonitorHost("Task2");

                host.ConsoleInfoEvent += (s) => Console.WriteLine(s);
                host.StepA();
                host.StepB();
                host.StepC();
            });

            Task.Factory.StartNew(() =>
            {

                var host = new MonitorHost("Task3");

                host.ConsoleInfoEvent += (s) => Console.WriteLine(s);
                host.StepA();
                host.StepB();
                host.StepC();
                //    Thread.Sleep(1000);
            });

        }

        static void Device()
        {
            OrderDistributionService srv = new OrderDistributionService(OrderServiceEnum.OrderDistribution);
            srv.GetFirstOrderEvent += Srv_GetFirstOrderEvent;
            srv.OrderStateChangeEvent += Srv_OrderStateChangeEvent;
            srv.UpdateOrderActualQuantityEvent += Srv_UpdateOrderActualQuantityEvent;
            srv.SendOrderServiceStateMessage += (s) => Console.WriteLine(s);
            srv.Start();

            Console.WriteLine("OrderDistributionService 运行中........");
            Console.WriteLine("输入 【exit】 退出........");

            while (true)
            {
                var key = Console.ReadLine();
                if (key == "exit")
                {
                    return;
                }
                Console.WriteLine("输入 【exit】 退出");
            }
        }

        private static void Srv_UpdateOrderActualQuantityEvent(OrderServiceEnum arg1, int arg2)
        {
            Redis.RedisHelper<OrderItem> redisHelper = new Redis.RedisHelper<OrderItem>();
            var obj = redisHelper.GetAll().Where(d => d.State == OrderItemStateEnum.DOWORK).OrderBy(d => d.CreateDateTime).FirstOrDefault();
            if (obj != null)
            {
                obj.ActualQuantity = arg2;
                redisHelper.Update(obj);
                redisHelper.Push(obj.Id);
                Console.WriteLine();
                Console.WriteLine("=================UpdateOrderActualQuantity=====================Start");
                Console.WriteLine(obj);
                Console.WriteLine("=================UpdateOrderActualQuantity=====================End");

            }

        }

        private static void Srv_OrderStateChangeEvent(OrderItem arg1, OrderServiceEnum arg2, int arg3)
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
                        Console.WriteLine();
                        Console.WriteLine("==============OrderStateChange==DoWork[2]=>DONE============Start");
                        Console.WriteLine(obj);
                        Console.WriteLine("==============OrderStateChange==DoWork[2]=>DONE====End");
                    }
                }
                return;
            }
            redisHelper.Update(arg1);
            redisHelper.Push(arg1.Id);
            Console.WriteLine();
            Console.WriteLine("==============OrderStateChange==============Start");
            Console.WriteLine(arg1);
            Console.WriteLine("==============OrderStateChange======End");
        }

        private static OrderItem Srv_GetFirstOrderEvent(OrderServiceEnum arg)
        {
            Redis.RedisHelper<OrderItem> redisHelper = new Redis.RedisHelper<OrderItem>();
            var obj = redisHelper.GetAll().Where(d => d.State == OrderItemStateEnum.NEW).OrderBy(d => d.CreateDateTime).FirstOrDefault();
            if (obj != null)
            {
                Console.WriteLine();
                Console.WriteLine("==============GetFirstOrder==============Start");
                Console.WriteLine(obj);
                Console.WriteLine("==============GetFirstOrder======End");
            }

            return obj;
        }
    }
}
