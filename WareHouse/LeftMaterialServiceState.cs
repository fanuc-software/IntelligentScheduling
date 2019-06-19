using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftMaterialService
{

    public enum LeftMaterialServiceStateEnum
    {
        OFF,
        FATAL,
        ERROR,
        WARN,
        INFO,
        DEBUG
    }

    public class LeftMaterialServiceState
    {
        public LeftMaterialServiceStateEnum State { get; set; }

        public string Message { get; set; }
    }
}
