using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AGV.Web.Host.Models;
using Microsoft.AspNetCore.Mvc;

namespace AGV.Web.Host.Controllers
{
    public class DeviceController : Controller
    {
        static bool isArrived = false;

        [HttpGet]
        [HttpPost]
        public JsonResult Init()
        {
            isArrived = false;
            return Json(new { state = true });
        }

        [HttpGet]
        [HttpPost]
        public JsonResult LocationDevices(string id)
        {
            if (Request.Method == "POST")
            {
                isArrived = true;

            }
            var query = Request.Query["action"];
            var node = new LocationDevice()
            {
                lastAction = query,
                name = id,
                lastActionStatus = isArrived ? ActionStatus.DONE : ActionStatus.EXECUTING,
                status = isArrived ? Status.IDLE : Status.EXECUTING
            };
            return Json(node);
        }
    }
}