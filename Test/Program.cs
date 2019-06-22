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


namespace Test
{

    class TestLeftMaterialService : BaseLeftMaterialService
    {
        public override IControlDevice ControlDevice => new AllenBradleyControlDevice();

    }

    class Program
    {
        static void Main(string[] args)
        {
            //var test_in = "RVART=                        111";
            //var test = "OKRVART=1                        101";
            //var count = test.Count();


            // var modula = new NewModulaWareHouseClient("TEST");
            //modula.ResetTray();

            //modula.MoveInTray(1, 2);

            // int prod = 0;
            // int tray = 0;
            //var ret = modula.GetPositionInfo(1, 2, out prod, out tray);
            //    var ret = modula.ResetTray();
            //   var ret = modula.MoveOutTray(1, 1);

            //System.Threading.Thread.Sleep(2000);


            //BaseOrderService srv = new BaseOrderService(OrderServiceEnum.OrderDistribution);
            //srv.Start();

            // eventBus.Deregister(mytest);

            //var dev = new AllenBradleyDevice();
            //bool mode = false;
            //var ret = dev.GetOrderMode(ref mode);

            var leftSrv = new TestLeftMaterialService();
            leftSrv.Start();

            while (true)
            {
                Console.ReadKey();

            }
        }
    }
}
