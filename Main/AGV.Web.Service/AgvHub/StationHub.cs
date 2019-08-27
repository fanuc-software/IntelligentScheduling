using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AGV.Web.Service.Models;
using DeviceAsset;
using EventBus;
using Microsoft.AspNet.SignalR;
namespace AGV.Web.Service.AgvHub
{
    public class StationHub : Hub
    {
        private SimpleEventBus eventBus;

        public StationHub()
        {
            eventBus = SimpleEventBus.GetDefaultEventBus();

        }
        public void clearOrder()
        {
            StaticData.CurrentMissionOrder = new System.Collections.Concurrent.BlockingCollection<Agv.Common.Model.AgvMissonModel>();

            eventBus.Post(new ClearMissionNode(), TimeSpan.FromSeconds(1));

        }

        public void writerSingnal(string ip, string adr, string value)
        {
            var m_FanucRobotDevice = new FanucRobotModbus(ip);
            var ret = m_FanucRobotDevice.Write(new FanucRobotDataConfig() { DataType = FanucRobotDataTypeEnum.DI, DataAdr = adr }, value);
            Clients.Client(Context.ConnectionId).getSignalrRes(ret);
        }
        public void readerSingnal(string ip, string adr)
        {
            var m_FanucRobotDevice = new FanucRobotModbus(ip);
            var ret = m_FanucRobotDevice.Read(new FanucRobotDataConfig() { DataType = FanucRobotDataTypeEnum.DO, DataAdr = adr });
            Clients.Client(Context.ConnectionId).getSignalrRes(ret);
        }
        public void loadAllOrder()
        {
            int index = 1;
            foreach (var item in StaticData.CurrentMissionOrder)
            {
                if (index++ <= 10)
                {
                    Clients.All.getAgvStateLog(item);

                }
            }

        }

        public void carryJobFinish()
        {
            eventBus.Post(new CarryJobFinish(), TimeSpan.FromSeconds(1));

        }

        public void sendOrder(StationNodeConfig id, string propName)
        {
            var hubContext2 = GlobalHost.ConnectionManager.GetHubContext<AgvMissonHub>();
            string order = $"{id.StationId}|{id.ProdeuctType}|{id.MaterielType}|{propName}";
            eventBus.Post(new getClientOrder(order), TimeSpan.FromSeconds(1));
            hubContext2.Clients.All.getClientOrder(order);

        }

        public override Task OnConnected()
        {
            Clients.Client(Context.ConnectionId).getAllStationInfo(StaticData.AppHostConfig.StationNodes);
            return base.OnConnected();
        }
    }
}