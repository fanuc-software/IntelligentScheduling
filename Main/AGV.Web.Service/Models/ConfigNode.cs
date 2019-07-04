using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGV.Web.Service.Models
{
    public class ConfigNode
    {
        public string Station { get; set; }

        public bool IsRequiredWait { get; set; }

        public bool ArrivalNotice  { get; set; }

        public string Operation { get; set; }
        public List<WaitNode>  IncludeWaits { get; set; }

        public WaitNode CurrentWait { get; set; }
    }


    public class WaitNode
    {
        public string Station { get; set; }

        /// <summary>
        /// 是否被占用
        /// </summary>
        public bool IsOccupy { get; set; }

        public WaitNodeState State { get; set; }

        public string WaitKey { get; set; }
    }

    public enum WaitNodeState
    {
        Free,
        OccupyNotArrival,
        ArrivalAndWaitSignal,
        GetTheSignal,
        Leave
    }
}