using OrderDistribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable CS0105 // “OrderDistribution”的 using 指令以前在此命名空间中出现过
using OrderDistribution;
#pragma warning restore CS0105 // “OrderDistribution”的 using 指令以前在此命名空间中出现过
using EventBus;
using System.Diagnostics;
using LeftMaterialService;
using DeviceAsset;
//using RightMaterialService;

namespace Test
{

    class TestLeftMaterialService : BaseLeftMaterialService
    {
        public override IControlDevice ControlDevice => new AllenBradleyControlDevice();

    }

    class TestOrderService : BaseOrderService
    {
        public override IOrderDevice Device => new AllenBradleyDevice();

        public TestOrderService(OrderServiceEnum serviceEnum) : base(serviceEnum)
        {

        }

    }

    class Program
    {

        static void Main(string[] args)
        {

            //var ab = new AllenBradleyControlDevice();
            //bool in_out = false;
            //var ret = ab.GetHouseInOut(ref in_out);

            //var robot = new FanucRobotModbus("192.168.1.172");
            //var ret = robot.Read(new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.GO, DataAdr = (161).ToString() });



            //for (int i = 0; i < 16; i++)
            //{
            //    var ret = robot.Read(new FanucRobotDataConfig { DataType = FanucRobotDataTypeEnum.DO, DataAdr = (161 + i).ToString() });

            //    if (bool.Parse(ret.Content) == true)
            //    {
            //        Console.WriteLine(i);
            //    }


            //}


            //var test_in = "RVART=                        111";
            //var test = "OKRVART=1                        101";
            //var count = test.Count();


            //for(int i=0;i<100;i++)
            //{
            //    var modula = new LefModulaWareHouseClient("TEST");


            //    var ret = modula.MoveInTray(1, 2);

            //    System.Threading.Thread.Sleep(30000);
            //    int prod = 0;
            //    int tray = 0;
            //    ret = modula.GetPositionInfo(1, 2, out prod, out tray);

            //    System.Threading.Thread.Sleep(30000);
            //    ret = modula.ResetTray();
            //}


            //var ret = modula.MoveInTray(1, 2);


            //int prod = 0;
            // int tray = 0;
            //ret = modula.GetPositionInfo(1, 2, out prod, out tray);
            //    var ret = modula.ResetTray();
            //   var ret = modula.MoveOutTray(1, 1);

            //System.Threading.Thread.Sleep(2000);


            BaseOrderService srv = new TestOrderService(OrderServiceEnum.OrderDistribution);
            srv.SendOrderServiceStateMessage += PrintErrorMessage;
            srv.GetFirstOrderEvent += Srv_GetFirstOrderEvent;
            srv.Start();


            
            // eventBus.Deregister(mytest);

            //var dev = new AllenBradleyDevice();
            //bool mode = false;
            //var ret = dev.GetOrderMode(ref mode);

            // var leftSrv = new TestLeftMaterialService();
            //leftSrv.Start();



            //var orderSrv = new TestOrderService(OrderServiceEnum.OrderDistribution);
            //orderSrv.GetFirstOrderEvent += Srv_GetFirstOrderEvent;
            //orderSrv.Start();


            while (true)
            {
                Console.ReadKey();

            }
        }

        private static void PrintErrorMessage(OrderServiceState state)
        {
            Console.WriteLine($"【ORDER】【ERROR CODE】: {state.ErrorCode}     【MESSAGE】:{state.Message}");
        }

        private static OrderItem Srv_GetFirstOrderEvent(OrderServiceEnum arg)
        {
            Console.WriteLine($"==============GetFirstOrder==============Start============={DateTime.Now.ToString()}");

            var ram = new Random();
            var count = ram.Next(10, 50);
            return new OrderItem { Id = "test", CreateDateTime = DateTime.Now, Type = 1, State = OrderItemStateEnum.NEW, Quantity = count };
        }

    }
}
