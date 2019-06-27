using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMaterialService
{

    public enum RightMaterialServiceStateEnum
    {
        OFF,
        FATAL,
        ERROR,
        WARN,
        INFO,
        DEBUG
    }

    public class RightMaterialServiceState
    {
        public RightMaterialServiceStateEnum State { get; set; }

        public string Message { get; set; }

        public RightMaterialServiceErrorCodeEnum ErrorCode { get; set; }
    }
}
