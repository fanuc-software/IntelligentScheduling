using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvMissionManager
{
    public enum AgvMissionServiceStateEnum
    {
        OFF,
        FATAL,
        ERROR,
        WARN,
        INFO,
        DEBUG
    }

    public class AgvMissionServiceState
    {
        public AgvMissionServiceStateEnum State { get; set; }

        public string Message { get; set; }

        public AgvMissionServiceErrorCodeEnum ErrorCode { get; set; }

        public override string ToString()
        {
            return $"【ErrorCode={ErrorCode},Message={Message},State={State},Time={DateTime.Now}】";
        }
    }
}
