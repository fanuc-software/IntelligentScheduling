using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AGV.Web.Service.Models
{
    public class LocationDevice
    {
        public string name { get; set; }

        public string lastAction { get; set; }




        public string lastActionStatus { set; get; }



        public string status { set; get; }
    }

    public enum ActionStatus
    {
        DONE,

        EXECUTING,

        FAILED
    }
    public enum Status
    {
        IDLE,


        EXECUTING,

        ERROR,

        TIMEOUT
    }
}
