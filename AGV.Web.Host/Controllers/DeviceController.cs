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
        [HttpGet]
        [HttpPost]
        public JsonResult LocationDevices(string id)
        {
            if (Request.Method == "POST")
            {

            }
            var query = Request.Query["action"];
            var node = new LocationDevice()
            {
                lastAction = query,
                name = id,
                lastActionStatus = ActionStatus.DONE,
                status = Status.IDLE
            };
            return Json(node);
        }
    }
}