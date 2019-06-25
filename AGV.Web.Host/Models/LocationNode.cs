using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AGV.Web.Host.Models
{
    public class LocationDevice
    {
        public string name { get; set; }

        public string lastAction { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]

        public ActionStatus lastActionStatus { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]

        public Status status { get; set; }
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
