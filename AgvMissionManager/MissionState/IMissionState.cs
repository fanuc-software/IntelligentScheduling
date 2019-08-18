using Agv.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvMissionManager.MissionState
{
    public interface IMissionState
    {
        string Condition();
        bool CanRequest();
        void Handle();

    }
}
