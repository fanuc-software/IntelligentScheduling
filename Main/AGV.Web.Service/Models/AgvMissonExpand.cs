using Agv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGV.Web.Service.Models
{
    public static class AgvMissonExpand
    {
        public static TransportOrder AgvMissonToTransportOrder(this AgvInMisson agvInMisson, string name = "")
        {
            return GetOrder(agvInMisson.Id, name);
        }

        public static TransportOrder AgvMissonToTransportOrder(this AgvOutMisson agvInMisson, string name = "")
        {

            return GetOrder(agvInMisson.Id, name);
        }


        private static TransportOrder GetOrder(string Id, string name = "")
        {
            var key = Id.ToString();
            var listNode = new List<ConfigNode>();
            if (StaticData.ProductNodeDict.ContainsKey(key))
            {
                listNode = StaticData.ProductNodeDict[key];
            }

            var transportOrder = new TransportOrder()
            {

                Deadline = DateTime.UtcNow.AddMinutes(20),
                Destinations = new List<DestinationOrder>(),
                Properties = new List<Property>(),
                Dependencies = new List<string>()
            };
            foreach (var item in listNode)
            {

                if (item.IsRequiredWait)
                {
                    var waitNode = item.IncludeWaits.FirstOrDefault(d => !d.IsOccupy) ?? new WaitNode();
                    waitNode.IsOccupy = true;
                    waitNode.State = WaitNodeState.OccupyNotArrival;
                    waitNode.WaitKey = Id;
                    item.CurrentWait = waitNode;

                    transportOrder.Destinations.Add(new DestinationOrder()
                    {
                        LocationName = waitNode.Station,
                        Operation = "Wait",
                        Properties = new List<Property>(),
                    });
                    var temp = new DestinationOrder()
                    {
                        LocationName = waitNode.Station,
                        Operation = "Wait",
                        Properties = new List<Property>()
                        {
                            new Property(){ Key="device:queryAtExecuted",Value=$"{Id}:wait"}
                        },
                    };
                    if (item.ArrivalNotice)
                    {
                        temp.Properties.Add(new Property()
                        {
                            Key = "device:requestAtSend",
                            Value = $"{Id}_{item.Signal}_{name}:arrived"
                        });
                    }
                    transportOrder.Destinations.Add(temp);
                    var node = new DestinationOrder()
                    {
                        LocationName = item.Station,
                        Operation = item.Operation,
                        Properties = new List<Property>(),

                    };
                    transportOrder.Destinations.Add(node);

                    if (!StaticData.SignalDict.ContainsKey(Id))
                    {
                        StaticData.SignalDict.TryAdd(Id, false);
                    }
                    else
                    {

                        StaticData.SignalDict[Id] = false;

                    }
                }
                else
                {
                    var node = new DestinationOrder()
                    {
                        LocationName = item.Station,
                        Operation = item.Operation,
                        Properties = new List<Property>(),

                    };
                    if (item.ArrivalNotice)
                    {
                        node.Properties.Add(new Property()
                        {
                            Key = "device:requestAtSend",
                            Value = $"{Id}_{item.Signal}_{name}:arrived"
                        });
                    }
                    transportOrder.Destinations.Add(node);
                }

            }
            return transportOrder;
        }
    }
}