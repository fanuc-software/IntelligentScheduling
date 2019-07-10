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

        public string SendWaitEndSignal(string id)
        {
            if (StaticData.SignalDict.ContainsKey(id))
            {
                StaticData.SignalDict[id] = true;
                var waitNode = StaticData.WaitNodes.FirstOrDefault(d => d.WaitKey == id);
                if (waitNode != null)
                {
                    waitNode.IsOccupy = false;
                    waitNode.State = WaitNodeState.Free;
                }
            }
            return id + ":True";
        }
        public void SendOutMission(AgvOutMisson message)
        {

            Clients.All.receiveOutMissionMessage(message);
        }

        public void SendInMission(AgvInMisson message)
        {
            
            Clients.All.receiveInMissionMessage(message);
        }

        public void SendOutMissionFinMessage(AgvOutMisson message)
        {
            Clients.All.receiveOutMissionFinMessage(message);
        }

        public void SendInMissionFinMessage(AgvInMisson message)
        {
            Clients.All.receiveInMissionFinMessage(message);
        }

        public void SendFeedingSignalMessage(AgvFeedingSignal message)
        {
            Clients.All.receiveFeedingSignalMessage(message);
        }

        public AgvInMisson SendMissonInOrder(AgvInMisson message)
        {
            var client = new Client();

            client.TransportOrders2($"{message.Id}_{ message.TimeId}", message.AgvMissonToTransportOrder());
            return message;
        }

        public AgvOutMisson SendMissonOutOrder(AgvOutMisson message)
        {
            var client = new Client();

            client.TransportOrders2($"{message.Id}_{ message.TimeId}", message.AgvMissonToTransportOrder());
            return message;
        }

        public void CloseAgvMission(string id)
        {

        }
    }
}