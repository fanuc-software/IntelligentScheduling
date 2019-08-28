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
        public static List<string> ListAgvJob = new List<string>();

        public static string WebLeftMaterial = "";
        public static string WebRightMaterial = "";
        public void start()
        {
            StaticData.BackGroudJobIsStart = true;


            WebStationClient webClient = new WebStationClient();
            WebAgvMissionManager webAgvMission = new WebAgvMissionManager();

            var hub = new NoticeHub();
            hub.clearAllWaitSignal();
            hub.clearAgvOrder();
            webAgvMission.Clear(null);
            StaticData.CurrentMissionOrder = new System.Collections.Concurrent.BlockingCollection<Agv.Common.Model.AgvMissonModel>();
            var job1 = BackgroundJob.Enqueue(() => webClient.Start());
            var job2 = BackgroundJob.Enqueue(() => webAgvMission.Start());
            ListAgvJob.Add(job1);
            ListAgvJob.Add(job2);


            Clients.All.getBackJobState(StaticData.BackGroudJobIsStart);

        }

        public void stop()
        {
            StaticData.BackGroudJobIsStart = false;
            foreach (var item in ListAgvJob)
            {
                BackgroundJob.Delete(item);
            }
            ListAgvJob.Clear();
            Clients.All.getBackJobState(StaticData.BackGroudJobIsStart);
        }


        #region 左侧料库服务
        public void leftMaterialStart()
        {

            WebLeftMaterial = BackgroundJob.Enqueue(() => startLeft());
            StaticData.LeftMaterialState = true;
            Clients.All.getleftMaterialState(true);
        }
        public void startLeft()
        {
            var leftSrv = new WebLeftMaterialService();
            leftSrv.SendLeftMaterialServiceStateMessageEvent += LeftSrv_SendLeftMaterialServiceStateMessageEvent;
            leftSrv.Start();
        }
        public void leftMaterialStop()
        {
            StaticData.LeftMaterialState = false;
            if (!string.IsNullOrEmpty(WebLeftMaterial))
            {
                BackgroundJob.Delete(WebLeftMaterial);
                WebLeftMaterial = "";
            }
            Clients.All.getleftMaterialState(false);
        }

        private void LeftSrv_SendLeftMaterialServiceStateMessageEvent(LeftMaterialService.LeftMaterialServiceState obj)
        {
            var hubContext2 = GlobalHost.ConnectionManager.GetHubContext<RestHub>();
            hubContext2.Clients.All.getLeftMaterialLog(obj.ToString());
        }

        #endregion


        #region 右侧料库服务
        public void rightMaterialStart()
        {

            WebRightMaterial = BackgroundJob.Enqueue(() => startRight());
            StaticData.RightMaterialState = true;
            Clients.All.getRightMaterialState(true);
        }

        public void startRight()
        {
            var right = new WebRightMaterialService();
            right.SendRightMaterialServiceStateMessageEvent += LeftSrv_SendRightMaterialServiceStateMessageEvent;
            right.Start();
        }
        private void LeftSrv_SendRightMaterialServiceStateMessageEvent(RightMaterialService.RightMaterialServiceState obj)
        {
            var hubContext2 = GlobalHost.ConnectionManager.GetHubContext<RestHub>();
            hubContext2.Clients.All.getRightMaterialLog(obj.ToString());

        }

        public void rightMaterialStop()
        {
            StaticData.RightMaterialState = false;
            if (!string.IsNullOrEmpty(WebRightMaterial))
            {
                BackgroundJob.Delete(WebRightMaterial);
                WebRightMaterial = "";
            }
            Clients.All.getRightMaterialState(false);
        }

        #endregion
        public override Task OnConnected()
        {
            Clients.All.getBackJobState(StaticData.BackGroudJobIsStart);
            Clients.All.getleftMaterialState(StaticData.LeftMaterialState);
            Clients.All.getRightMaterialState(StaticData.RightMaterialState);
            return base.OnConnected();
        }
    }
}