using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Agv.Common;
using AGV.Web.Service.Models;
using Microsoft.AspNet.SignalR;

namespace AGV.Web.Service.AgvHub
{
    public class NoticeHub : Hub
    {
        Client client;

        public NoticeHub()
        {
            try
            {
                client = new Client();

            }
            catch (Exception ex)
            {

                Clients.All.pushSystemMessage($"AGV调度服务连接失败,异常信息:{ex.Message}", new { state = false });
            }
        }
        public void sendTask(string id)
        {
            TransportOrder order = new TransportOrder();
            string name = $"{id}{ DateTime.Now.ToFileTime()}";
            try
            {
                order = new AgvInMisson() { Id = id }.AgvMissonToTransportOrder(name);
                client.TransportOrders2(name, order);
                Clients.All.queryOrder(name);


            }
            catch (Exception ex)
            {

                Clients.All.pushSystemMessage($"发送订单失败,异常信息:{ex.Message}", new { state = false });
            }

        }
        public string sendWaitEndSignal(string id)
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

        public void clearAllWaitSignal()
        {
            foreach (var waitNode in StaticData.WaitNodes)
            {
                waitNode.IsOccupy = false;
                waitNode.State = WaitNodeState.Free;
            }
            foreach (var item in StaticData.SignalDict)
            {
                StaticData.SignalDict[item.Key] = true;

            }
        }
        public void loadOrderProxy(string name)
        {
            var obj = client.TransportOrders(name);
            Clients.All.getCurrentOrder(obj);


        }
        public override Task OnConnected()
        {

            Clients.All.getAllAgvOrder(StaticData.ProductNodeDict);
            return base.OnConnected();
        }
    }
}