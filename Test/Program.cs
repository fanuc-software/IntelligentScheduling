using OrderDistribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WareHouse;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var test_in = "RVART=                        111";
            var test = "OKRVART=1                        101";
            var count = test.Count();


            var modula = new ModulaHouse();
            var ret = modula.MoveInHouseTray(0, 1);

            //System.Threading.Thread.Sleep(2000);

            //var ret = modula.ResetHouseTray();
            //BaseOrderService srv = new BaseOrderService(OrderServiceEnum.OrderDistribution);
            //srv.Start();

            Console.ReadKey();
        }
    }
}
