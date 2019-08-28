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
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;    //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
                p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;//不显示程序窗口
                p.Start();//启动程序

                //向cmd窗口发送输入信息
                var startcmd = @"net start redis";

                p.StandardInput.WriteLine(startcmd);

                p.StandardInput.AutoFlush = true;
                p.WaitForExit(1000);//等待程序执行完退出进程
                p.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
         

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
                logger.Info("输入 【exit】 退出");
                Console.WriteLine("输入 【exit】 退出");
            }
        }

        private static void MonitorService_ShowMessageEvent(string obj)
        {
            logger.Info(obj);
            Console.WriteLine(obj);
        }
    }
}
