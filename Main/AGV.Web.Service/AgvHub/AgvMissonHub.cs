using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agv.Common;
using AGV.Web.Service.Models;
using Microsoft.AspNet.SignalR;

namespace AGV.Web.Service.AgvHub
{
    public class AgvMissonHub : Hub
    {
        private IClient client;

        public AgvMissonHub()
        {
          client = new Client();
        }
                
        public void SendOutMission(AgvOutMisson message)
        {
            Clients.All.receiveOutMissionMessage(message);
        }

        public void SendInMission(AgvInMisson message)
        {
            client.TransportOrders2(message.Id, message.AgvMissonToTransportOrder());

            Clients.All.receiveInMissionMessage(message);
        }

    }
}