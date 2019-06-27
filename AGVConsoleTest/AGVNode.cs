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
        JackUnload
    }
    public class AGVNode
    {
        public string StationName { get; set; }
    }
}
