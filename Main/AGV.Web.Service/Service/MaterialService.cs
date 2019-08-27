using LeftMaterialService;
using RightMaterialService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGV.Web.Service.Service
{
    public class WebLeftMaterialService : BaseLeftMaterialService
    {
        public override LeftMaterialService.IControlDevice ControlDevice => new LeftMaterialService.AllenBradleyControlDevice();

    }
    public class WebRightMaterialService : BaseRightMaterialService
    {
        public override RightMaterialService.IControlDevice ControlDevice => new RightMaterialService.AllenBradleyControlDevice();

    }
}