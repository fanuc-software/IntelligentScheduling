using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVConsoleTest
{
    public enum AGVAction
    {
        JackLoad,
        JackUnload,
        Wait
    }
    public class AGVNode
    {
        public string StationName { get; set; }

        public string OrderName { get; set; }
        public AGVAction Action { get; set; }
    }

}
