using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ServiceStack.Redis;
using ServiceStack.Configuration;
using OrderDistribution;

namespace FANUC.Host.Redis
{
    public class RedisHelper<T> where T : BaseItem
    {
        RedisManagerPool managerPool;
        static string host;
        static string channel;

        static RedisHelper()
        {
            host = ConfigurationManager.AppSettings["RedisHost"];
            channel = ConfigurationManager.AppSettings["RedisChannel"];
        }
        public RedisHelper()
        {
            managerPool = new RedisManagerPool(host);
        }
        public void Store(T model)
        {

            using (var redisClient = managerPool.GetClient())
            {
                var redisTodo = redisClient.As<T>();

                redisTodo.Store(model);
            }
        }

        public void Push(string message)
        {
            using (var redisClient = managerPool.GetClient())
            {
                redisClient.PublishMessage(channel, message);
            }
        }
        public void Update(T model)
        {
            using (var redisClient = managerPool.GetClient())
            {
                var redisTodo = redisClient.As<T>();
                var obj = redisTodo.GetById(model.Id);
                if (obj != null)
                {
                    redisTodo.Store(model);

                }
            }
        }

        public T Get(object id)
        {
            using (var redisClient = managerPool.GetClient())
            {
                var redisTodo = redisClient.As<T>();
                return redisTodo.GetById(id);

            }
        }
        public IEnumerable<T> GetAll()
        {
            using (var redisClient = managerPool.GetClient())
            {
                var redisTodo = redisClient.As<T>();
                return redisTodo.GetAll();

            }
        }
        public void DeleteAll()
        {
            using (var redisClient = managerPool.GetClient())
            {
                var redisTodo = redisClient.As<T>();
                redisTodo.DeleteAll();
            }
        }
    }
}
