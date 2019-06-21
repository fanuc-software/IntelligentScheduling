using OrderDistribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using OrderDistribution;
using EventBus;
using System.Diagnostics;
using LeftMaterialService;
using DeviceAsset;


namespace Test
{
   
    class TestLeftMaterialService : BaseLeftMaterialService
    {
        public override IControlDevice ControlDevice => new AllenBradleyControlDevice();

    }

    class TestOrderService : BaseOrderService
    {
        public override IOrderDevice Device => new FanucRobotDevice();

        public TestOrderService(OrderServiceEnum serviceEnum) : base(serviceEnum)
        {

        }

    }

    class Program
    {
        static void Main(string[] args)
        {

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


            //var modula = new NewModulaWareHouseClient("TEST");
            //int prod = 0;
            //int tray = 0;
            //var ret = modula.GetPositionInfo(1, 1, out prod, out tray);
            //var ret = modula.ResetTray();
            //var ret = modula.MoveOutTray(1, 1);

            //System.Threading.Thread.Sleep(2000);


            //BaseOrderService srv = new BaseOrderService(OrderServiceEnum.OrderDistribution);
            //srv.Start();

            // eventBus.Deregister(mytest);

            //var dev = new AllenBradleyDevice();
            //bool mode = false;
            //var ret = dev.GetOrderMode(ref mode);

            //var leftSrv = new TestLeftMaterialService();
            //leftSrv.Start();



            var orderSrv = new TestOrderService(OrderServiceEnum.OrderDistribution);
            orderSrv.GetFirstOrderEvent += Srv_GetFirstOrderEvent;
            orderSrv.Start();



            Console.ReadKey();
        }

        private static OrderItem Srv_GetFirstOrderEvent(OrderServiceEnum arg)
        {
            Console.WriteLine("==============GetFirstOrder==============Start");
            return new OrderItem { Id = "test", CreateDateTime = DateTime.Now, Type = 1, State = OrderItemStateEnum.NEW, Quantity = 1 };
        }

    }
}
