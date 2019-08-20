using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Storage;
using Microsoft.Owin;
using Hangfire.MySql.Core;
using Owin;
using System.Linq;
using System.Web;
using AGV.Web.Service.Service;

[assembly: OwinStartup(typeof(AGV.Web.Service.Startup))]

namespace AGV.Web.Service
{
    public class Startup
    {
        public static List<string> ListJob = new List<string>();
        [Obsolete]
        public void Configuration(IAppBuilder app)
        {

            // 有关如何配置应用程序的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkID=316888
            app.MapSignalR();

            GlobalConfiguration.Configuration
                .UseStorage(new MySqlStorage(ConfigurationManager.ConnectionStrings["DB_Service"].ConnectionString, new MySqlStorageOptions
                {
                    QueuePollInterval = TimeSpan.FromSeconds(15),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                    CountersAggregateInterval = TimeSpan.FromMinutes(5),
                    PrepareSchemaIfNecessary = true,
                    DashboardJobListLimit = 50000,
                    TransactionTimeout = TimeSpan.FromMinutes(1),
                    TablePrefix = "Hangfire"
                }));


            var filter = new BasicAuthAuthorizationFilter(
              new BasicAuthAuthorizationFilterOptions
              {
                  SslRedirect = false,
                  // Require secure connection for dashboard
                  RequireSsl = false,
                  // Case sensitive login checking
                  LoginCaseSensitive = false,
                  // Users
                  Users = new[]
                  {

                        new BasicAuthAuthorizationUser
                        {
                            Login = "admin",//用户名
                            // Password as SHA1 hash
                            Password = new byte[] { 0x58,0x1d,0xb3,0x1d,0x94,0x0f,0xe9,0x62,0x25,0xfa,0xf4,0x5b,0x1a,0x14,0x82,0x50,0x35,0x0d,0x35,0x3b }
                        }
                  }
              });
            var options = new DashboardOptions
            {
                AppPath = VirtualPathUtility.ToAbsolute("~"),
                AuthorizationFilters = new IAuthorizationFilter[] { filter }

            };
            app.UseHangfireDashboard("/hangfire", options);
            app.UseHangfireServer();

            InitBackgroundJob();
        }
        void InitBackgroundJob()
        {
            WebStationClient webClient = new WebStationClient();

            var job1 = BackgroundJob.Enqueue(() => webClient.Start());
           
            ListJob.Add(job1);
        }

    }
}
