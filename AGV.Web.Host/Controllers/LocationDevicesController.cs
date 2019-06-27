using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGV.Web.Host.Models;
using Microsoft.AspNetCore.Mvc;

namespace AGV.Web.Host.Controllers
{
    public class LocationDevicesController : Controller
    {

        [HttpPost]
        public JsonResult Arrived(string action)
        {
            return null;
        }

        [HttpGet]
        public JsonResult WaitQuery()
        {
            var query = Request.Query["action"];
            var node = new LocationDevice()
            {
                lastAction = query,
                name = "WaitQuery",
                lastActionStatus = ActionStatus.DONE,
                status = Status.IDLE
            };
            return Json(node);

        }
    }
}