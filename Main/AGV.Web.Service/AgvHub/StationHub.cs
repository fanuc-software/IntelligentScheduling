using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AGV.Web.Service.Models;
using Microsoft.AspNet.SignalR;
namespace AGV.Web.Service.AgvHub
{
    public class StationHub : Hub
    {

        public void sendOrder(StationNodeConfig id, string propName)
        {
            var hubContext2 = GlobalHost.ConnectionManager.GetHubContext<AgvMissonHub>();
            hubContext2.Clients.All.getClientOrder($"{id.StationId}|{id.ProdeuctType}|{id.MaterielType}|{propName}");

        }

        public override Task OnConnected()
        {
            Clients.Client(Context.ConnectionId).getAllStationInfo(StaticData.AppHostConfig.StationNodes);
            return base.OnConnected();
        }
    }
}