using OrderDistribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var ab = new AllenBradleyDevice();

            //int mode = 0;
            //var ret = ab.SetProductType(2);
            //ret = ab.SetQuantity(10);

            //Console.WriteLine("ret:" + ret + " res:" + mode);

            BaseOrderService srv = new BaseOrderService(OrderServiceEnum.OrderDistribution);

            srv.Start();

            Console.ReadKey();
        }
    }
}
