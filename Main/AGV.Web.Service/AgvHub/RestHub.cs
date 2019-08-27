using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AGV.Web.Service.Models;
using AGV.Web.Service.Service;
using Hangfire;
using Microsoft.AspNet.SignalR;

namespace AGV.Web.Service.AgvHub
{
    public class RestHub : Hub
    {
        public void start()
        {
            StaticData.BackGroudJobIsStart = true;


            WebStationClient webClient = new WebStationClient();
            WebAgvMissionManager webAgvMission = new WebAgvMissionManager();

            var hub = new NoticeHub();
            hub.clearAllWaitSignal();
            hub.clearAgvOrder();
            webAgvMission.Clear(null);

            var job1 = BackgroundJob.Enqueue(() => webClient.Start());
            var job2 = BackgroundJob.Enqueue(() => webAgvMission.Start());
            Startup.ListJob.Add(job1);
            Startup.ListJob.Add(job2);


            Clients.All.getBackJobState(StaticData.BackGroudJobIsStart);

        }

        public void stop()
        {
            StaticData.BackGroudJobIsStart = false;
            foreach (var item in Startup.ListJob)
            {
                BackgroundJob.Delete(item);
            }
            Clients.All.getBackJobState(StaticData.BackGroudJobIsStart);
        }

        public override Task OnConnected()
        {
            Clients.All.getBackJobState(StaticData.BackGroudJobIsStart);
            return base.OnConnected();
        }
    }
}