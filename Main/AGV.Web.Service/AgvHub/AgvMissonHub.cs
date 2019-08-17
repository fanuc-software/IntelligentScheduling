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

        public string SendFirstWaitEndSignal(string id)
        {
            if (!StaticData.ProductNodeDict.ContainsKey(id))
            {
                return "Error";
            }
            var nodes = StaticData.ProductNodeDict[id].FirstOrDefault(d => d.IsRequiredWait);
            string key = $"{id}";
            if (nodes != null)
            {
                key += nodes.Station;
                if (StaticData.SignalDict.ContainsKey(key))
                {
                    StaticData.SignalDict[key] = true;
                    var waitNode = StaticData.WaitNodes.FirstOrDefault(d => d.WaitKey == key);
                    if (waitNode != null)
                    {
                        waitNode.IsOccupy = false;
                        waitNode.State = WaitNodeState.Free;
                    }
                }
            }

            return id + ":True";
        }


        public string SendLastWaitEndSignal(string id)
        {
            if (!StaticData.ProductNodeDict.ContainsKey(id))
            {
                return "Error";
            }
            var nodes = StaticData.ProductNodeDict[id].LastOrDefault(d => d.IsRequiredWait);
            string key = $"{id}";
            if (nodes != null)
            {
                key += nodes.Station;
                if (StaticData.SignalDict.ContainsKey(key))
                {
                    StaticData.SignalDict[key] = true;
                    var waitNode = StaticData.WaitNodes.FirstOrDefault(d => d.WaitKey == key);
                    if (waitNode != null)
                    {
                        waitNode.IsOccupy = false;
                        waitNode.State = WaitNodeState.Free;
                    }
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
            var hubContext2 = GlobalHost.ConnectionManager.GetHubContext<NoticeHub>();

            try
            {
                var client = new Client();
                string id = $"{message.Id}_{ message.TimeId}";
                client.TransportOrders2(id, message.AgvMissonToTransportOrder());
                hubContext2.Clients.All.queryOrder(id);
                return message;
            }
            catch (Exception ex)
            {
                hubContext2.Clients.All.pushSystemMessage($"AGV调度服务连接失败,异常信息:{ex.Message}", new { state = false });

                return message;
            }

        }

        public AgvOutMisson SendMissonOutOrder(AgvOutMisson message)
        {
            var hubContext2 = GlobalHost.ConnectionManager.GetHubContext<NoticeHub>();
            try
            {
                var client = new Client();
                string id = $"{message.Id}_{ message.TimeId}";
                client.TransportOrders2(id, message.AgvMissonToTransportOrder());


                hubContext2.Clients.All.queryOrder(id);
                return message;
            }
            catch (Exception ex)
            {
                hubContext2.Clients.All.pushSystemMessage($"AGV调度服务连接失败,异常信息:{ex.Message}", new { state = false });

                return message;
            }

        }

        public void CloseAgvMission(string id)
        {

        }
    }
}