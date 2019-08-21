using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agv.Common
{
    public enum AgvStationEnum
    {
        [System.Runtime.Serialization.EnumMember(Value = @"WareHouse")]
        WareHouse = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"RX07")]
        RX07 = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"RX08")]
        RX08 = 3,

        [System.Runtime.Serialization.EnumMember(Value = @"RX09")]
        RX09 = 4,

        [System.Runtime.Serialization.EnumMember(Value = @"RX10")]
        RX10 = 5,
    }
}
