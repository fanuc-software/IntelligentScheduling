using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgvStationClient
{
    public enum AgvStationCommandTypeEnum
    {
        RAW_IN,
        EMPTY_OUT,
        EMPTY_IN,
        FIN_OUT,
    }

    public class AgvStationCommand
    {
        public AgvStationCommandTypeEnum Type { get; set; }
    }
}
