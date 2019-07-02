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
    }
}