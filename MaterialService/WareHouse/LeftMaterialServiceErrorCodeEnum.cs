using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeftMaterialService
{
    public enum LeftMaterialServiceErrorCodeEnum
    {
        [System.Runtime.Serialization.EnumMember(Value = @"NORMAL")]
        NORMAL = 0, 

        [System.Runtime.Serialization.EnumMember(Value = @"INI_GETREQ")]
        INI_GETREQ = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"GETREQ")]
        GETREQ = 11,
        [System.Runtime.Serialization.EnumMember(Value = @"CHECKONBEGIN")]

        CHECKONBEGIN = 12,
        [System.Runtime.Serialization.EnumMember(Value = @"INOUT")]

        INOUT = 13,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_GETPRODUCTTYPE")]

        OUT_GETPRODUCTTYPE = 51,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_GETMATERIALTYPE")]

        OUT_GETMATERIALTYPE = 52,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_MOVEOUTTRAY")]

        OUT_MOVEOUTTRAY = 53,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_GETTRAYINPOSITION")]

        OUT_GETTRAYINPOSITION = 54,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_TRAYINPOSITIONRESET")]

        OUT_TRAYINPOSITIONRESET = 55,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_GETTRAYPOSITION")]

        OUT_GETTRAYPOSITION = 56,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_SETPRODUCTPOS")]

        OUT_SETPRODUCTPOS = 57,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_SETTRAYPOS")]

        OUT_SETTRAYPOS = 58,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_SETQUANTITY")]

        OUT_SETQUANTITY = 59,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_SETMATERIALTYPECONFIRM")]

        OUT_SETMATERIALTYPECONFIRM = 60,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_SETREQUESTFIN")]

        OUT_SETREQUESTFIN = 61,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_GETREQINFO")]

        OUT_GETREQINFO = 62,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_REQINFORESET")]

        OUT_REQINFORESET = 63,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_WRITEDATA")]

        OUT_WRITEDATA = 64,
        [System.Runtime.Serialization.EnumMember(Value = @"OUT_SETREQINFOFIN")]

        OUT_SETREQINFOFIN = 65,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_GETPRODUCTTYPE")]

        IN_GETPRODUCTTYPE = 101,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_GETMATERIALTYPE")]

        IN_GETMATERIALTYPE = 102,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_MOVEINTRAY")]

        IN_MOVEINTRAY = 103,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_GETTRAYINPOSITION")]

        IN_GETTRAYINPOSITION = 104,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_TRAYINPOSITIONRESET")]

        IN_TRAYINPOSITIONRESET = 105,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_GETTRAYPOSITION")]

        IN_GETTRAYPOSITION = 106,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_SETPRODUCTPOS")]

        IN_SETPRODUCTPOS = 107,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_SETTRAYPOS")]

        IN_SETTRAYPOS = 108,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_SETQUANTITY")]

        IN_SETQUANTITY = 109,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_SETMATERIALTYPECONFIRM")]

        IN_SETMATERIALTYPECONFIRM = 110,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_SETREQUESTFIN")]

        IN_SETREQUESTFIN = 111,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_GETREQINFO")]

        IN_GETREQINFO = 112,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_REQINFORESET")]

        IN_REQINFORESET = 113,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_WRITEDATA")]

        IN_WRITEDATA = 114,
        [System.Runtime.Serialization.EnumMember(Value = @"IN_SETREQINFOFIN")]

        IN_SETREQINFOFIN = 115,
    }
}
