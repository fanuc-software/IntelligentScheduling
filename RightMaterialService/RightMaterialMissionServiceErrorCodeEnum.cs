using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RightMaterialService
{
    public enum RightMaterialMissionServiceErrorCodeEnum
    {
        NORMAL=0,
		IDCONFLICT=1,
        QUANTITYLIMIT=2,
        TWOOUTPICKING=3,
    }
}
