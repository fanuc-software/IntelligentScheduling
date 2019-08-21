using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agv.Common
{
    public enum AgvMissionTypeEnum
    {
        [System.Runtime.Serialization.EnumMember(Value = @"RAW_IN")]

        RAW_IN,
        [System.Runtime.Serialization.EnumMember(Value = @"EMPTY_OUT")]

        EMPTY_OUT,
        [System.Runtime.Serialization.EnumMember(Value = @"EMPTY_IN")]

        EMPTY_IN,
        [System.Runtime.Serialization.EnumMember(Value = @"FIN_OUT")]

        FIN_OUT,
    }
}
