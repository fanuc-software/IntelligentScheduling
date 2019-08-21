using Agv.Common;
using Agv.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGV.Web.Service.Models
{

    public class agvStateChange
    {
        public string Model { private set; get; }
        public agvStateChange(string model)
        {
            Model = model;
        }
    }
    public class receiveOutMissionFinMessage
    {
        public AgvOutMissonModel Model { private set; get; }
        public receiveOutMissionFinMessage(AgvOutMissonModel model)
        {
            Model = model;
        }
    }

    public class receiveInMissionFinMessage
    {

        public AgvInMissonModel Model { private set; get; }
        public receiveInMissionFinMessage(AgvInMissonModel model)
        {
            Model = model;
        }
    }
    public class ClearMissionNode
    {

    }
    public class getClientOrder
    {
        public string Model { private set; get; }
        public getClientOrder(string model)
        {
            Model = model;
        }
    }

    public class SendWaitEndSignal
    {
        public string Model { private set; get; }

        public SendWaitEndSignal(string model)
        {
            Model = model;
        }
    }

    public class SendFirstWaitEndSignal
    {
        public string Model { private set; get; }

        public SendFirstWaitEndSignal(string model)
        {
            Model = model;
        }
    }

    public class SendLastWaitEndSignal
    {
        public string Model { private set; get; }

        public SendLastWaitEndSignal(string model)
        {
            Model = model;
        }
    }

    public class SendOutMission
    {
        public AgvOutMissonModel Model { private set; get; }

        public SendOutMission(AgvOutMissonModel model)
        {
            Model = model;
        }
    }

    public class SendInMission
    {
        public AgvInMissonModel Model { private set; get; }

        public SendInMission(AgvInMissonModel model)
        {
            Model = model;
        }
    }

    public class SendOutMissionFinMessage
    {
        public AgvOutMissonModel Model { private set; get; }

        public SendOutMissionFinMessage(AgvOutMissonModel model)
        {
            Model = model;
        }
    }

    public class SendInMissionFinMessage
    {
        public AgvInMissonModel Model { private set; get; }

        public SendInMissionFinMessage(AgvInMissonModel model)
        {
            Model = model;
        }
    }

    public class SendFeedingSignalMessage
    {
        public AgvFeedingSignal Model { private set; get; }

        public SendFeedingSignalMessage(AgvFeedingSignal model)
        {
            Model = model;
        }
    }

    public class SendMissonInOrder
    {
        public AgvInMissonModel Model { private set; get; }

        public SendMissonInOrder(AgvInMissonModel model)
        {
            Model = model;
        }
    }
    public class SendMissonOutOrder
    {
        public AgvOutMissonModel Model { private set; get; }

        public SendMissonOutOrder(AgvOutMissonModel model)
        {
            Model = model;
        }
    }

    public class SendAgvLog
    {
        public string Model { private set; get; }

        public SendAgvLog(string model)
        {
            Model = model;
        }
    }

    public class SendAgvStateLog
    {
        public AgvMissonModel Model { private set; get; }

        public SendAgvStateLog(AgvMissonModel model)
        {
            Model = model;
        }
    }
}