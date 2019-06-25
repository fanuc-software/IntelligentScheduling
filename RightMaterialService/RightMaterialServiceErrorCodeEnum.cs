using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMaterialService
{
    public enum RightMaterialServiceErrorCodeEnum
    {
        NORMAL = 0,
        INI_GETREQ = 1,
        GETREQ = 11,
        CHECKONBEGIN = 12,
        INOUT = 13,
        OUT_GETPRODUCTTYPE = 51,
        OUT_GETMATERIALTYPE = 52,
        OUT_MOVEOUTTRAY = 53,
        OUT_GETTRAYINPOSITION = 54,
        OUT_TRAYINPOSITIONRESET = 55,
        OUT_GETTRAYPOSITION = 56,
        OUT_SETPRODUCTPOS = 57,
        OUT_SETTRAYPOS = 58,
        OUT_SETMATERIALTYPECONFIRM = 59,
        OUT_SETREQUESTFIN = 60,
        OUT_GETREQINFO = 61,
        OUT_REQINFORESET = 62,
        OUT_WRITEDATA = 63,
        OUT_SETREQINFOFIN = 64,
        IN_GETPRODUCTTYPE = 101,
        IN_GETMATERIALTYPE = 102,
        IN_MOVEINTRAY = 103,
        IN_GETTRAYINPOSITION = 104,
        IN_TRAYINPOSITIONRESET = 105,
        IN_GETTRAYPOSITION = 106,
        IN_SETPRODUCTPOS = 107,
        IN_SETTRAYPOS = 108,
        IN_SETMATERIALTYPECONFIRM = 109,
        IN_SETREQUESTFIN = 10,
        IN_GETREQINFO = 11,
        IN_REQINFORESET = 12,
        IN_WRITEDATA = 13,
        IN_SETREQINFOFIN = 14,
    }
}
