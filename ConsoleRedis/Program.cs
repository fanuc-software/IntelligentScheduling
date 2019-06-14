using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRedis
{

    public class OrderItem
    {
        public string Id { get; set; }

        public int Type { get; set; }

        public int Quantity { get; set; }


        public DateTime CreateDateTime { get; set; }
        public override string ToString()
        {
            return $"{Id} {Type} {Quantity} {CreateDateTime}";
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var managerPool = new RedisManagerPool("123.207.159.105:6379");

            using (var client = managerPool.GetClient())
            {
                IRedisSubscription subscription = client.CreateSubscription();
                subscription.OnMessage = (channel, msg) =>
                {

                    Receive(msg);
                };

                subscription.SubscribeToChannels("OrderItem");

                Console.ReadLine();
            }

        }

        static void Receive(string id)
        {
            var managerPool = new RedisManagerPool("123.207.159.105:6379");

            using (var client = managerPool.GetClient())
            {
                var redisTodo = client.As<OrderItem>();

                var obj = redisTodo.GetById(id);
                Console.WriteLine(obj);

                Console.ReadLine();
            }
        }
    }
}
