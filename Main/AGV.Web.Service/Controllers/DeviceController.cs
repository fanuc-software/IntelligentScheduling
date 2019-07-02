using AGV.Web.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AGV.Web.Service.Controllers
{
    public class DeviceController : Controller
    {


        [HttpGet]
        [HttpPost]
        public JsonResult Init()
        {

            return Json(new { state = true });
        }

        [HttpGet]
        [HttpPost]
        public JsonResult LocationDevices(string id)
        {
            var query = Request.QueryString["action"];

            if (Request.HttpMethod == "POST" && query == "arrived")
            {
                if (StaticData.SignalDict.ContainsKey(id))
                {
                    StaticData.SignalDict[id] = true;

                }
                return Json(new { state = true, id = id });
            }
            bool isArrived = false;
            if (StaticData.SignalDict.ContainsKey(id))
            {
                isArrived = StaticData.SignalDict[id];
            }
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