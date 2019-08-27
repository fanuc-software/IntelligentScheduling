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
                client = new Client(StaticData.AppHostConfig.AgvServiceUrl);

            }
            catch (Exception ex)
            {

                Clients.Client(Context.ConnectionId).pushSystemMessage($"AGV调度服务连接失败,异常信息:{ex.Message}", new { state = false });
            }
        }
        public void queryWaitSignal()
        {

            Clients.Client(Context.ConnectionId).loadWaitSignal(StaticData.SignalDict);
        }
        public void sendTask(string id)
        {
            TransportOrder order = new TransportOrder();
            string name = $"{id}_{ DateTime.Now.ToFileTime()}";
            try
            {
                order = new AgvInMisson() { Id = id }.AgvMissonToTransportOrder(name);
                client.TransportOrders2(name, order);
                StaticData.OrderName.Add(name);
                Clients.All.queryOrder(name);


            }
            catch (Exception ex)
            {

                Clients.Client(Context.ConnectionId).pushSystemMessage($"发送订单失败,异常信息:{ex.Message}", new { state = false });
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
            try
            {
                var obj = client.TransportOrders(name);
                Clients.Client(Context.ConnectionId).getCurrentOrder(obj);
            }
            catch (Exception ex)
            {

                Clients.Client(Context.ConnectionId)?.pushSystemMessage($"AGV订单查询失败,异常信息:{ex.Message}", new { state = false });

            }



        }

        public void clearAgvOrder()
        {
            foreach (var item in StaticData.OrderName)
            {
                try
                {
                    client.Withdrawal2(item);
                }
                catch (Exception ex)
                {

                    Clients.Client(Context.ConnectionId)?.pushSystemMessage($"AGV订单撤销失败,异常信息:{ex.Message}", new { state = false });

                }
            }
            StaticData.OrderName = new System.Collections.Concurrent.BlockingCollection<string>();
            
        }
        public void loadAllOrder()
        {
            int index = 1;
            foreach (var item in StaticData.OrderName)
            {
                if (index++ <= 10)
                {
                    Clients.All.queryOrder(item);
                }

            }
        }
        public override Task OnConnected()
        {
            Clients.Client(Context.ConnectionId).getAllAgvOrder(StaticData.ProductNodeDict);
            return base.OnConnected();
        }
    }
}