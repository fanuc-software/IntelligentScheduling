using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGV.Web.Service.Models
{
    public class AppHostConfig
    {
        public string Environment { get; set; }

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