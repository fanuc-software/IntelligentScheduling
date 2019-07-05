using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agv.Common
{
    public enum AgvSendActionEnum
    {
        SendOutMission,
        SendInMission,
        SendOutMissionFinMessage,
        SendInMissionFinMessage,
        SendFeedingSignalMessage,
    }
}
