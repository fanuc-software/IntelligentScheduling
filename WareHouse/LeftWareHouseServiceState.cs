using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouse
{

    public enum LeftWareHouseServiceStateEnum
    {
        OFF,
        FATAL,
        ERROR,
        WARN,
        INFO,
        DEBUG
    }

    public class LeftWareHouseServiceState
    {
        public LeftWareHouseServiceStateEnum State { get; set; }

        public string Message { get; set; }
    }
}
