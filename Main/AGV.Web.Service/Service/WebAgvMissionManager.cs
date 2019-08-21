using Agv.Common;
using Agv.Common.Model;
using AGV.Web.Service.AgvHub;
using AGV.Web.Service.Models;
using AgvMissionManager;
using EventBus;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace AGV.Web.Service.Service
{
    public class WebAgvMissionManager : IDisposable
    {
        AgvMissionManagerClient agvMissionManagerClient;
        private SimpleEventBus eventBus;

        public WebAgvMissionManager()
        {
            eventBus = SimpleEventBus.GetDefaultEventBus();
            eventBus.Register(this);
            agvMissionManagerClient = new AgvMissionManagerClient();
            agvMissionManagerClient.SendAgvMissonEvent += AgvMissionManagerClient_SendAgvMissonEvent;
            agvMissionManagerClient.SendSignalrEvent += AgvMissionManagerClient_SendSignalrEvent;
            agvMissionManagerClient.ShowLogEvent += AgvMissionManagerClient_ShowLogEvent;
        }

        private void AgvMissionManagerClient_SendSignalrEvent(string arg1, object arg2)
        {

            var obj = Type.GetType($"AGV.Web.Service.Models.{arg1}");
            var instance = Activator.CreateInstance(obj, new object[] { arg2 });
            eventBus.Post(instance, TimeSpan.FromSeconds(1));
        }

        public void Start()
        {

            agvMissionManagerClient.Start();
        }

        private void AgvMissionManagerClient_ShowLogEvent(string obj)
        {
            var hubContext2 = GlobalHost.ConnectionManager.GetHubContext<StationHub>();
            if (hubContext2 != null)
            {
                hubContext2.Clients.All.getAgvLog(obj);

            }
        }

        private void AgvMissionManagerClient_SendAgvMissonEvent(AgvMissonModel obj)
        {
            var hubContext2 = GlobalHost.ConnectionManager.GetHubContext<StationHub>();
            if (hubContext2 != null)
            {
                hubContext2.Clients.All.getAgvStateLog(obj);

            }

        }
        [EventSubscriber]
        public void Clear(ClearMissionNode clear)
        {
            agvMissionManagerClient.Clear();
        }

        [EventSubscriber]
        public void receiveOutMissionMessage(SendOutMission s)
        {
            StaticData.CurrentMissionOrder.Add(s.Model);
            agvMissionManagerClient.PushOutMission(s.Model);

        }

        [EventSubscriber]
        public void receiveInMissionMessage(SendInMission s)
        {
            StaticData.CurrentMissionOrder.Add(s.Model);
            agvMissionManagerClient.PushInMission(s.Model);

        }
        [EventSubscriber]
        public void receiveFeedingSignalMessage(SendFeedingSignalMessage s)
        {
            agvMissionManagerClient.UpdataFeedingSignal(s.Model);

        }
        [EventSubscriber]
        public void agvStateChange(agvStateChange s)
        {
            agvMissionManagerClient.AgvStateChange(s.Model);

        }
        public void Dispose()
        {
            agvMissionManagerClient.Cancel();
            eventBus.Deregister(this);

        }
    }
}