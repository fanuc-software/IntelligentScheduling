using Agv.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agv.Common
{
    public class AgvFeedingSignal
    {
        public string Id { get; set; }

        public AgvStationEnum ClientId { get; set; }

        public AgvMissionTypeEnum Type { get; set; }

        public bool Value { get; set; }
    }
}
