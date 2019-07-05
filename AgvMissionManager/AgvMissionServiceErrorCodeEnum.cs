using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvMissionManager
{
    public enum AgvMissionServiceErrorCodeEnum
    {
        NORMAL=0,
		IDCONFLICT=1,
        QUANTITYLIMIT=2,
        TWOOUTPICKING=3,
        WAREHOUSEIN=100,
        CARRYIN = 101,
        AGVIN=102,
        WAREHOUSEOUT = 150,
        CARRYOUT=151,
        AGVOUT=152,
        AGVOUTPREPLACEWAIT=153,
    }
}
