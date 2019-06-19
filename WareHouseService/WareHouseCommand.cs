using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseService
{
    public enum WareHouseCommandTypeEnum
    {
        MOVEIN,
        MOVEOUT,
        RESET,

    }

    public class WareHouseCommand
    {
        public WareHouseCommandTypeEnum Type { get; set; }

    }
}
