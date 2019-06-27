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
        static bool isArrived = false;

        [HttpPost]
        public JsonResult Arrived(string action)
        {
            isArrived = !isArrived;
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
                lastActionStatus = isArrived ? ActionStatus.DONE : ActionStatus.EXECUTING,
                status = isArrived ? Status.IDLE : Status.EXECUTING
            };
            return Json(node);

        }
    }
}