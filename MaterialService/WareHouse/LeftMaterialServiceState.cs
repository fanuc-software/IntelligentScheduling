using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftMaterialService
{

    public enum LeftMaterialServiceStateEnum
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

    public class LeftMaterialServiceState
    {
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

        public LeftMaterialServiceStateEnum State { get; set; }

        public string Message { get; set; }
        [Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]

        public LeftMaterialServiceErrorCodeEnum ErrorCode { get; set; }

        public override string ToString()
        {
            return $"【{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}】 【{State}】 {Message} 【{ErrorCode}】";
        }
    }
}
