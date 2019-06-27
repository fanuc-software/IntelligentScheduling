using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvMissionManager
{
    public enum RightMaterialMissionServiceStateEnum
    {
        OFF,
        FATAL,
        ERROR,
        WARN,
        INFO,
        DEBUG
    }

    public class RightMaterialMissionServiceState
    {
        public RightMaterialMissionServiceStateEnum State { get; set; }

        public string Message { get; set; }

        public RightMaterialMissionServiceErrorCodeEnum ErrorCode { get; set; }
    }
}
