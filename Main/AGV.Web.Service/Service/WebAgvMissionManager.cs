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

            var className = StaticData.AppHostConfig.GetType().GetProperty($"{StaticData.AppHostConfig.Environment}CarryDevice").GetValue(StaticData.AppHostConfig).ToString();
            //   RightCarryService.IControlDevice dObj = Activator.CreateInstance(Type.GetType(className)) as RightCarryService.IControlDevice;



            agvMissionManagerClient = new AgvMissionManagerClient(new TestStationDevice() as RightCarryService.IControlDevice);
            agvMissionManagerClient.SendAgvMissonEvent += AgvMissionManagerClient_SendAgvMissonEvent;
            agvMissionManagerClient.SendSignalrEvent += AgvMissionManagerClient_SendSignalrEvent;
            agvMissionManagerClient.ShowLogEvent += AgvMissionManagerClient_ShowLogEvent;
        }

        private void AgvMissionManagerClient_SendSignalrEvent(string arg1, object arg2)
        {
            if (arg1 == "SendMissonInOrder" || arg1 == "SendMissonOutOrder")
            {
                SendMissonOrder(arg2 as AgvMissonModel);
                return;
            }
            if (arg1 == "SendFirstWaitEndSignal")
            {
                new AgvMissonHub().SendFirstWaitEndSignal(arg2.ToString());
                return;
            }
            if (arg1 == "SendLastWaitEndSignal")
            {
                new AgvMissonHub().SendLastWaitEndSignal(arg2.ToString());
                return;
            }
            var obj = Type.GetType($"AGV.Web.Service.Models.{arg1}");
            var instance = Activator.CreateInstance(obj, new object[] { arg2 });
            eventBus.Post(instance, TimeSpan.FromSeconds(1));
           
        }
        private void SendMissonOrder(AgvMissonModel message)
        {
            var hubContext2 = GlobalHost.ConnectionManager.GetHubContext<NoticeHub>();
            try
            {
                var client = new Client(StaticData.AppHostConfig.AgvServiceUrl);
                string id = $"{message.Id}_{ message.TimeId}";
                StaticData.OrderName.Add(id);
                var order = new AgvInMissonModel() { Id = message.Id }.AgvMissonToTransportOrder();

                client.TransportOrders2(id, order);


                hubContext2.Clients.All.queryOrder(id);
            }
            catch (Exception ex)
            {
                hubContext2.Clients.All.pushSystemMessage($"AGV调度服务连接失败,异常信息:{ex.Message}", new { state = false });

            }

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
        public void carryJobFinish(CarryJobFinish carryJobFinish)
        {
            agvMissionManagerClient.CarryJobFinish();
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