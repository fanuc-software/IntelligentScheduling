using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;
namespace AGV.Web.Service.Models
{
    public class StaticData
    {
        public static BlockingCollection<WaitNode> WaitNodes = new BlockingCollection<WaitNode>();

        public static ConcurrentDictionary<string, bool> SignalDict = new ConcurrentDictionary<string, bool>();

        public static ConcurrentDictionary<string, List<ConfigNode>> ProductNodeDict = new ConcurrentDictionary<string, List<ConfigNode>>();

        static StaticData()
        {
            var waitArr = new string[] { "wait_C", "wait_D", "wait_E", "wait_F" };
            waitArr.ToList().ForEach(d =>
            {
                WaitNodes.Add(new WaitNode() { Station = d, State = WaitNodeState.Free });
            });
            var listNode = WaitNodes.OrderBy(d => d.Station).ToList();
            ProductNodeDict.TryAdd("RX08_RAWIN", new List<ConfigNode>()
            {
                new ConfigNode(){ Station="A",Operation="JackUnload"},
                new ConfigNode()
                {
                    Station ="A1",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    ArrivalNotice=true,
                    IncludeWaits=new List<WaitNode>(){ listNode[0],listNode[1] }
                },
                new ConfigNode(){ Station="A",Operation="JackUnload"}

            });
            ProductNodeDict.TryAdd("RX09_RAWIN", new List<ConfigNode>()
            {
                 new ConfigNode(){ Station="B",Operation="JackUnload"},
                new ConfigNode()
                {
                    Station ="B1",
                    Operation ="JackUnload",
                    IsRequiredWait =true,
                    ArrivalNotice=true,
                    IncludeWaits=new List<WaitNode>(){ listNode[2],listNode[3] }
                },
                new ConfigNode(){ Station="B",Operation="JackUnload"}
            });

        }

    }
}