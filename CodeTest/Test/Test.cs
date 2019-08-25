using OrderDistribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using LeftMaterialService;
using RightMaterialService;
using RightCarryService;
using Agv.Common;
using System.Threading;
using AgvStationClient;

namespace Test
{

    class TestRightMaterialService : BaseRightMaterialService
    {
        public override RightMaterialService.IControlDevice ControlDevice => new RightMaterialService.AllenBradleyControlDevice();

    }

    class TestLeftMaterialService : BaseLeftMaterialService
    {
        public override LeftMaterialService.IControlDevice ControlDevice => new LeftMaterialService.AllenBradleyControlDevice();

    }

    class TestOrderService : BaseOrderService
    {
        public override IOrderDevice Device => new AllenBradleyDevice();

        public TestOrderService(OrderServiceEnum serviceEnum) : base(serviceEnum)
        {

        }

    }

    class Test
    {

        static void Main(string[] args)
        {
            Task.Factory.StartNew(()=> {
                RX08FanucRobotClientDevice rx08 = new RX08FanucRobotClientDevice();

                bool req = false;
                rx08.GetRawInRequireState(ref req);

                if(req==true)
                {
                    rx08.SetRawInFin(true);

                    while(req==true)
                    {
                        rx08.GetRawInRequireState(ref req);

                        if (req==false)
                        {
                            rx08.SetRawInFin(false);
                            break;
                        }
                    }
                }

            });

            Task.Factory.StartNew(() => {
                RX08FanucRobotClientDevice rx08 = new RX08FanucRobotClientDevice();

                bool req = false;
                rx08.GetRawInRequireState(ref req);

                if (req == true)
                {
                    rx08.SetRawInFin(true);

                    while (req == true)
                    {
                        rx08.GetRawInRequireState(ref req);

                        if (req == false)
                        {
                            rx08.SetRawInFin(false);
                            break;
                        }
                    }
                }

            });

            Task.Factory.StartNew(() => {
                RX08FanucRobotClientDevice rx08 = new RX08FanucRobotClientDevice();

                bool req = false;
                rx08.GetRawInRequireState(ref req);

                if (req == true)
                {
                    rx08.SetRawInFin(true);

                    while (req == true)
                    {
                        rx08.GetRawInRequireState(ref req);

                        if (req == false)
                        {
                            rx08.SetRawInFin(false);
                            break;
                        }
                    }
                }

            });

            Task.Factory.StartNew(() => {
                RX08FanucRobotClientDevice rx08 = new RX08FanucRobotClientDevice();

                bool req = false;
                rx08.GetRawInRequireState(ref req);

                if (req == true)
                {
                    rx08.SetRawInFin(true);

                    while (req == true)
                    {
                        rx08.GetRawInRequireState(ref req);

                        if (req == false)
                        {
                            rx08.SetRawInFin(false);
                            break;
                        }
                    }
                }

            });

            //Task.Factory.StartNew(() => {
            //    RX08FanucRobotClientDevice rx08 = new RX08FanucRobotClientDevice();

            //    bool req = false;
            //    rx08.GetRawInRequireState(ref req);

            //    if (req == true)
            //    {
            //        rx08.SetFinOutFin(true);

            //        while (req == true)
            //        {
            //            rx08.GetRawInRequireState(ref req);

            //            if (req == false)
            //            {
            //                rx08.SetFinOutFin(false);
            //                break;
            //            }
            //        }
            //    }

            //});

            //Task.Factory.StartNew(() => {
            //    RX08FanucRobotClientDevice rx08 = new RX08FanucRobotClientDevice();

            //    bool req = false;
            //    rx08.GetRawInRequireState(ref req);

            //    if (req == true)
            //    {
            //        rx08.SetFinOutFin(true);

            //        while (req == true)
            //        {
            //            rx08.GetRawInRequireState(ref req);

            //            if (req == false)
            //            {
            //                rx08.SetFinOutFin(false);
            //                break;
            //            }
            //        }
            //    }

            //});

            //Task.Factory.StartNew(() => {
            //    RX08FanucRobotClientDevice rx08 = new RX08FanucRobotClientDevice();

            //    bool req = false;
            //    rx08.GetRawInRequireState(ref req);

            //    if (req == true)
            //    {
            //        rx08.SetFinOutFin(true);

            //        while (req == true)
            //        {
            //            rx08.GetRawInRequireState(ref req);

            //            if (req == false)
            //            {
            //                rx08.SetFinOutFin(false);
            //                break;
            //            }
            //        }
            //    }

            //});

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


            //for (int i = 0; i < 100; i++)
            //{
            //bool ret1,ret2;
            ////  SignalrTest.MainTest();

            //bool ret1;
            //var modula = new RightModulaWareHouseClient("TESTR");
            //ret1 = modula.ResetTray();
            //

            //if(true){
            //    ret1 = modula.MoveInTray(1, 7);

            //    int prod = 0;
            //    int tray = 0;
            //    int qty = 0;
            //    ret1 = modula.GetPositionInfo(1, 7, out prod, out tray, out qty);
            //}


            //bool ret2;
            //var modula2 = new LefModulaWareHouseClient("TESTL");
            //ret2 = modula2.ResetTray();
            //ret2 = modula2.MoveInTray(1, 1);
            //modula2.ResetTray();
            //    System.Threading.Thread.Sleep(30000);
            //int prod = 0;
            //int tray = 0;
            //int qty = 0;
            //ret = modula.GetPositionInfo(1, 1, out prod, out tray,out qty);

            //var device = new AllenBradleyControlDevice();
            //device.SetHouseProductPostion(prod);
            //device.SetHouseTrayPostion(tray);
            //device.SetHouseQuantity(qty);

            //    System.Threading.Thread.Sleep(30000);
            //modula.ResetTray();
            //}


            //modula.WriteBackData(1, 1, false);

            //var ret = modula.MoveInTray(1, 2);


            //int prod = 0;
            // int tray = 0;
            //ret = modula.GetPositionInfo(1, 2, out prod, out tray);
            //    var ret = modula.ResetTray();
            //   var ret = modula.MoveOutTray(1, 1);

            //System.Threading.Thread.Sleep(2000);


            //BaseOrderService srv = new TestOrderService(OrderServiceEnum.OrderDistribution);
            //srv.SendOrderServiceStateMessage += PrintErrorMessage;
            //srv.GetFirstOrderEvent += Srv_GetFirstOrderEvent;
            //srv.Start();

            // eventBus.Deregister(mytest);

            //var dev = new AllenBradleyDevice();
            //bool mode = false;
            //var ret = dev.GetOrderMode(ref mode);

            //var leftSrv = new TestRightMaterialService();
            //leftSrv.SendRightMaterialServiceStateMessageEvent += PrintRightMaterialServiceError;
            //leftSrv.Start();
            //Console.WriteLine("服务启动中");

            //var leftSrv = new TestLeftMaterialService();
            //leftSrv.SendLeftMaterialServiceStateMessageEvent += PrintLeftMaterialServiceError;
            //leftSrv.Start();
            //Console.WriteLine("服务启动中");

            //var orderSrv = new TestOrderService(OrderServiceEnum.OrderDistribution);
            //orderSrv.GetFirstOrderEvent += Srv_GetFirstOrderEvent;
            //orderSrv.SendOrderServiceStateMessage += PrintErrorMessage;
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

        private static void PrintLeftMaterialServiceError(LeftMaterialServiceState state)
        {
            Console.WriteLine($"【NEW】【ERROR CODE】: {state.ErrorCode}     【MESSAGE】:{state.Message}");
        }

        private static void PrintRightMaterialServiceError(RightMaterialServiceState state)
        {
            Console.WriteLine($"【NEW】【ERROR CODE】: {state.ErrorCode}     【MESSAGE】:{state.Message}");
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
