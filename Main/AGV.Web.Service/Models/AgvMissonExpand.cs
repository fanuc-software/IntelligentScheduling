using Agv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGV.Web.Service.Models
{
    public static class AgvMissonExpand
    {
        public static TransportOrder AgvMissonToTransportOrder(this AgvInMisson agvInMisson)
        {
            var key = agvInMisson.Id.ToString();
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
                    var waitNode = item.IncludeWaits.FirstOrDefault(d => !d.IsOccupy)??new WaitNode();
                    waitNode.IsOccupy = true;
                    waitNode.State = WaitNodeState.OccupyNotArrival;
                    waitNode.WaitKey = agvInMisson.Id;
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
                            new Property(){ Key="device:queryAtExecuted",Value=$"{agvInMisson.Id}:wait"}
                        },
                    };
                    if (item.ArrivalNotice)
                    {
                        temp.Properties.Add(new Property()
                        {
                            Key = "device:requestAtSend",
                            Value = $"{agvInMisson.Id}_{item.Signal}:arrived"
                        });
                    }
                    transportOrder.Destinations.Add(temp);
                    if (!StaticData.SignalDict.ContainsKey(agvInMisson.Id))
                    {
                        StaticData.SignalDict.TryAdd(agvInMisson.Id, false);
                    }
                    else
                    {

                        StaticData.SignalDict[agvInMisson.Id] = false;

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
                            Value = $"{agvInMisson.Id}_{item.Signal}:arrived"
                        });
                    }
                    transportOrder.Destinations.Add(node);
                }

            }

            return transportOrder;

        }

        public static TransportOrder AgvMissonToTransportOrder(this AgvOutMisson agvInMisson)
        {
            var key = agvInMisson.Id.ToString();
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
                    var waitNode = item.IncludeWaits.FirstOrDefault(d => !d.IsOccupy)??new WaitNode();
                    
                    waitNode.IsOccupy = true;
                    waitNode.State = WaitNodeState.OccupyNotArrival;
                    waitNode.WaitKey = agvInMisson.Id;
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
                            new Property(){ Key="device:queryAtExecuted",Value=$"{agvInMisson.Id}:wait"}
                        },
                    };
                    if (item.ArrivalNotice)
                    {
                        temp.Properties.Add(new Property()
                        {
                            Key = "device:requestAtSend",
                            Value = $"{agvInMisson.Id}_{item.Signal}:arrived"
                        });
                    }
                    transportOrder.Destinations.Add(temp);
                    if (!StaticData.SignalDict.ContainsKey(agvInMisson.Id))
                    {
                        StaticData.SignalDict.TryAdd(agvInMisson.Id, false);
                    }
                    else
                    {

                        StaticData.SignalDict[agvInMisson.Id] = false;

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
                            Value = $"{agvInMisson.Id}_{item.Signal}:arrived"
                        });
                    }
                    transportOrder.Destinations.Add(node);
                }

            }

            return transportOrder;

        }

    }
}