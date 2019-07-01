using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Agv.Common;
using Microsoft.AspNet.SignalR;

namespace AGV.Web.Service.AgvHub
{
    public class AgvMissonHub : Hub
    {

        public void SendOutMission(AgvOutMisson message)
        {
            Clients.All.receiveOutMissionMessage(message);
        }

        public void SendInMission(AgvInMisson message)
        {
            Clients.All.receiveInMissionMessage(message);
        }

    }
}