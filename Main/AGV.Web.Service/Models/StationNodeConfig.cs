using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGV.Web.Service.Models
{
    public class AppHostConfig
    {
        public string Environment { get; set; }

        public string AgvServiceUrl { get; set; }
        public string AppUrl { get; set; }
        public string AppBinPath { get; set; }
        public string DevelopmentCarryDevice { get; set; }

        public string ProductionCarryDevice { get; set; }
        public List<StationNodeConfig> StationNodes { get; set; }
    }

    public class StationNodeConfig
    {
        public string Name { get; set; }

        public string StationId { get; set; }

        public int ProdeuctType { get; set; }

        public int MaterielType { get; set; }

        public string DevelopmentService { get; set; }

        public string ProductionService { get; set; }
    }
}