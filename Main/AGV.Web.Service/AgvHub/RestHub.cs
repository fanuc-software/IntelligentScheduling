using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AGV.Web.Service.Models;
using AGV.Web.Service.Service;
using EventBus;
using Hangfire;
using Microsoft.AspNet.SignalR;

namespace AGV.Web.Service.AgvHub
{
    public class RestHub : Hub
    {
        public static List<string> ListAgvJob = new List<string>();
        static WebStationClient webClient = new WebStationClient();

        public static string WebLeftMaterial = "";
        public static string WebRightMaterial = "";
        private SimpleEventBus eventBus;

        public RestHub()
        {
            eventBus = SimpleEventBus.GetDefaultEventBus();

        }
        public void start()
        {
            if (StaticData.BackGroudJobIsStart)
            {
                return;
            }
            StaticData.BackGroudJobIsStart = true;


            WebAgvMissionManager webAgvMission = new WebAgvMissionManager();

            var hub = new NoticeHub();
            hub.clearAllWaitSignal();
            hub.clearAgvOrder();

            eventBus.Post(new ClearMissionNode(), TimeSpan.FromSeconds(1));
            StaticData.CurrentMissionOrder = new System.Collections.Concurrent.BlockingCollection<Agv.Common.Model.AgvMissonModel>();
            try
            {

                var job2 = BackgroundJob.Enqueue(() => webAgvMission.Start());
                //   ListAgvJob.Add(job1);
                ListAgvJob.Add(job2);


                Clients.All.getBackJobState(StaticData.BackGroudJobIsStart);
            }
            catch (Exception ex)
            {

                Clients.All.getStartLog(ex.Message);

            }


        }

        public void startUnit(string unit)
        {
            string job = "";
            if (unit == "RX07")
            {
                if (StaticData.RX07BackGroudJobState)
                {
                    return;
                }
                StaticData.RX07BackGroudJobState = true;
                job = BackgroundJob.Enqueue(() => webClient.RX07Start());
                Clients.All.getRX07BackJobState(StaticData.RX07BackGroudJobState);

            }
            else if (unit == "RX08")
            {
                if (StaticData.RX08BackGroudJobState)
                {
                    return;
                }
                StaticData.RX08BackGroudJobState = true;
                job = BackgroundJob.Enqueue(() => webClient.RX08Start());
                Clients.All.getRX08BackJobState(StaticData.RX08BackGroudJobState);
            }
            else if (unit == "RX09")
            {
                if (StaticData.RX09BackGroudJobState)
                {
                    return;
                }
                StaticData.RX09BackGroudJobState = true;
                job = BackgroundJob.Enqueue(() => webClient.RX09Start());
                Clients.All.getRX09BackJobState(StaticData.RX09BackGroudJobState);
            }
            ListAgvJob.Add($"{unit}_{job}");


        }

        public void stopUnit(string unit)
        {
            string item = ListAgvJob.FirstOrDefault(d => d.Contains(unit));
            if (unit == "RX07" && StaticData.RX07BackGroudJobState)
            {

                StaticData.RX07BackGroudJobState = false;
                string id = item.Contains("_") ? item.Split('_')[1] : item;
                BackgroundJob.Delete(id);
                Clients.All.getRX07BackJobState(StaticData.RX07BackGroudJobState);
                return;

            }
            if (unit == "RX08" && StaticData.RX08BackGroudJobState)
            {

                StaticData.RX08BackGroudJobState = false;
                string id = item.Contains("_") ? item.Split('_')[1] : item;
                BackgroundJob.Delete(id);
                Clients.All.getRX08BackJobState(StaticData.RX08BackGroudJobState);
                return;

            }
            if (unit == "RX09" && StaticData.RX09BackGroudJobState)
            {

                StaticData.RX09BackGroudJobState = false;
                string id = item.Contains("_") ? item.Split('_')[1] : item;
                BackgroundJob.Delete(id);
                Clients.All.getRX09BackJobState(StaticData.RX09BackGroudJobState);

            }
        }
        public void stop()
        {
            StaticData.BackGroudJobIsStart = false;
            StaticData.RX07BackGroudJobState = false;
            StaticData.RX08BackGroudJobState = false;
            StaticData.RX09BackGroudJobState = false;
            foreach (var item in ListAgvJob)
            {
                string id = item.Contains("_") ? item.Split('_')[1] : item;
                BackgroundJob.Delete(id);
            }
            ListAgvJob.Clear();
            Clients.All.getBackJobState(StaticData.BackGroudJobIsStart);
            Clients.All.getRX07BackJobState(StaticData.RX07BackGroudJobState);
            Clients.All.getRX08BackJobState(StaticData.RX08BackGroudJobState);
            Clients.All.getRX09BackJobState(StaticData.RX09BackGroudJobState);

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
            Clients.All.getRX07BackJobState(StaticData.RX07BackGroudJobState);
            Clients.All.getRX08BackJobState(StaticData.RX08BackGroudJobState);
            Clients.All.getRX09BackJobState(StaticData.RX09BackGroudJobState);

            Clients.All.getleftMaterialState(StaticData.LeftMaterialState);
            Clients.All.getRightMaterialState(StaticData.RightMaterialState);
            return base.OnConnected();
        }
    }
}