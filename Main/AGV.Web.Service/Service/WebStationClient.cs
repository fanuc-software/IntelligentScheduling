using Agv.Common;
using AGV.Web.Service.Models;
using AgvStationClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AGV.Web.Service.Service
{
    public class WebStationClient : IDisposable
    {

        SignalrService signalrService;

        AutoResetEvent resetEvent = new AutoResetEvent(false);
        public List<StationProxyService> stationProxyServices { set; get; } = new List<StationProxyService>();

        public WebStationClient()
        {
            foreach (var item in StaticData.AppHostConfig.StationNodes)
            {
                string className = item.GetType().GetProperty($"{StaticData.AppHostConfig.Environment}Service").GetValue(item).ToString();
                IStationDevice dObj = Activator.CreateInstance(Type.GetType(className)) as IStationDevice;
                var proxy = new StationProxyService((AgvStationEnum)Enum.Parse(typeof(AgvStationEnum), item.StationId), dObj);
                proxy.SendSingnalrEvent += (s, e) => signalrService.Send(s, e).Wait();
                stationProxyServices.Add(proxy);

            }

        }

        public void Start()
        {
            signalrService = new SignalrService(StaticData.AppHostConfig.AppUrl, "AgvMissonHub");
            InitSignalarEvent();

            signalrService.Start().Wait();

            Parallel.ForEach(stationProxyServices, item =>
            {
                Task.Factory.StartNew(() => item.StartStationClentFlow());
                Task.Factory.StartNew(() => item.FeedingSignalStart());

            });
            resetEvent.WaitOne();
        }

        private void InitSignalarEvent()
        {
            signalrService.OnMessage<AgvOutMisson>(AgvReceiveActionEnum.receiveOutMissionFinMessage.EnumToString(), (s) =>
            {
                stationProxyServices.ForEach(d => d.OnAgvOutMissonEvent(s));
            });
            signalrService.OnMessage<AgvInMisson>(AgvReceiveActionEnum.receiveInMissionFinMessage.EnumToString(), (s) =>
            {
                stationProxyServices.ForEach(d => d.OnAgvInMissonEvent(s));
            });
            signalrService.OnMessage<string>("getClientOrder", (node) =>
            {
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

            });
        }


        public void Dispose()
        {


            resetEvent.Reset();

        }
    }
}