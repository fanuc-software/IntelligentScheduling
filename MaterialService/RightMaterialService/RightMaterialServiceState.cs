using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMaterialService
{

    public enum RightMaterialServiceStateEnum
    {
        [System.Runtime.Serialization.EnumMember(Value = @"OFF")]
        OFF,
        [System.Runtime.Serialization.EnumMember(Value = @"FATAL")]
        FATAL,
        [System.Runtime.Serialization.EnumMember(Value = @"ERROR")]
        ERROR,
        [System.Runtime.Serialization.EnumMember(Value = @"WARN")]
        WARN,
        [System.Runtime.Serialization.EnumMember(Value = @"INFO")]
        INFO,
        [System.Runtime.Serialization.EnumMember(Value = @"DEBUG")]
        DEBUG
    }

    public class RightMaterialServiceState
    {
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

        public RightMaterialServiceStateEnum State { get; set; }

        public string Message { get; set; }

        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

        public RightMaterialServiceErrorCodeEnum ErrorCode { get; set; }

        public override string ToString()
        {
            return $"【{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}】 【{State}】 {Message} 【{ErrorCode}】";

        }
    }
}
