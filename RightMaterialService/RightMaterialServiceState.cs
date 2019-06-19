using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMaterialService
{

    public enum StationClientStateEnum
    {
        OFF,
        FATAL,
        ERROR,
        WARN,
        INFO,
        DEBUG
    }

    public class StationClientState
    {
        public StationClientStateEnum State { get; set; }

        public string Message { get; set; }
    }
}
