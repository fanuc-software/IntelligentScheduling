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

            Redis.RedisHelper<OrderItem> redisHelper = new Redis.RedisHelper<OrderItem>();
            redisHelper.Store(new OrderItem()
            {

                Id = "2",
                CreateDateTime = DateTime.Now,
                Quantity = 12300,
                Type = 2
            });
            redisHelper.Push("OrderItem", "2");
        }


    }
}
