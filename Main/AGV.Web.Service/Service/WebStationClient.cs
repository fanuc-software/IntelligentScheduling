using Agv.Common;
using Agv.Common.Model;
using AGV.Web.Service.AgvHub;
using AGV.Web.Service.Models;
using AgvStationClient;
using EventBus;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AGV.Web.Service.Service
{
    public class WebStationClient : IDisposable
    {
        AutoResetEvent resetEvent = new AutoResetEvent(false);
        public List<StationProxyService> stationProxyServices { set; get; } = new List<StationProxyService>();
        SimpleEventBus eventBus;
        public WebStationClient()
        {
            eventBus = SimpleEventBus.GetDefaultEventBus();
            eventBus.Register(this);
            foreach (var item in StaticData.AppHostConfig.StationNodes)
            {
                IStationDevice dObj = null;
                string className = item.GetType().GetProperty($"{StaticData.AppHostConfig.Environment}Service").GetValue(item).ToString();
                if (Type.GetType(className) == null)
                {
                    string path = $@"{StaticData.AppHostConfig.AppBinPath}\AgvStationClient.dll";
                  
                    var assembly = Assembly.LoadFrom(path);
                    var tpe = assembly.GetType(className);
                    dObj = Activator.CreateInstance(tpe) as IStationDevice;

                }
                else
                {
                    dObj = Assembly.GetAssembly(Type.GetType(className)).CreateInstance(className) as IStationDevice;
                }

                var proxy = new StationProxyService((AgvStationEnum)Enum.Parse(typeof(AgvStationEnum), item.StationId), dObj);
                proxy.SendSingnalrEvent += Proxy_SendSingnalrEvent;
                proxy.SendLogEvent += Proxy_SendLogEvent;
                stationProxyServices.Add(proxy);

            }

        }

        private void Proxy_SendLogEvent(string obj)
        {
            var hubContext2 = GlobalHost.ConnectionManager.GetHubContext<StationHub>();
            if (hubContext2 != null)
            {
                hubContext2.Clients.All.getUnitLog(obj);

            }
        }

        private void Proxy_SendSingnalrEvent(string arg1, object arg2)
        {
            var obj = Type.GetType($"AGV.Web.Service.Models.{arg1}");
            var instance = Activator.CreateInstance(obj, new object[] { arg2 });

            eventBus.Post(instance, TimeSpan.FromSeconds(1));
        }

        public void Start()
        {

            Parallel.ForEach(stationProxyServices, item =>
            {
                Task.Factory.StartNew(() => item.StartStationClentFlow());
                Task.Factory.StartNew(() => item.FeedingSignalStart());

            });
            resetEvent.WaitOne();
        }

        [EventSubscriber]
        public void receiveOutMissionFinMessage(SendOutMissionFinMessage s)
        {
            stationProxyServices.ForEach(d => d.OnAgvOutMissonEvent(s.Model));

        }
        [EventSubscriber]
        public void receiveInMissionFinMessage(SendInMissionFinMessage s)
        {

            stationProxyServices.ForEach(d => d.OnAgvInMissonEvent(s.Model));

        }

        [EventSubscriber]
        public void getClientOrder(getClientOrder s)
        {
            var node = s.Model;
            string id = node.Split('|')[0];
            string name = node.Split('|')[3];
            string productType = node.Split('|')[1];
            string materielType = node.Split('|')[2];

            stationProxyServices.ForEach(d =>
            {
                if (d.Station_Id.EnumToString() == id)
                {
                    var obj = (d.StationDevice as TestStationDevice);
                    if (obj != null)
                    {
                        // RawIn_Prod RawIn_Mate
                        string preName = name.Split('_')[0];
                        obj.GetType().GetProperty($"{preName}_Prod").SetValue(obj, productType);
                        obj.GetType().GetProperty($"{preName}_Mate").SetValue(obj, materielType);
                        obj.GetType().GetProperty(name).SetValue(obj, true);

                    }
                }

            });

        }
        public void Dispose()
        {
            resetEvent.Reset();
            eventBus.Deregister(this);

        }
    }
}