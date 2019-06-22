using FANUC.Host.Service;
using NLog;
using OrderDistribution;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FANUC.Host
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            var serviceName = ConfigurationManager.AppSettings["MonitorService"];
        
            try
            {
                var monitorService = Assembly.GetExecutingAssembly().CreateInstance(serviceName) as BaseHostService;
                monitorService.ShowMessageEvent += MonitorService_ShowMessageEvent;
                monitorService.Start();

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                Console.WriteLine(ex.Message);

            }
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

        private static void MonitorService_ShowMessageEvent(string obj)
        {
            Console.WriteLine(obj);
        }
    }
}
