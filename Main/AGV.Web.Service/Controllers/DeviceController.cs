﻿using AGV.Web.Service.AgvHub;
using AGV.Web.Service.Models;
using EventBus;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AGV.Web.Service.Controllers
{
    public class DeviceController : Controller
    {

        public DeviceController()
        {

        }

        public JsonResult Init()
        {

            return Json(new { state = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LocationDevices(string id)
        {
            var query = Request.QueryString["action"];

            if (Request.HttpMethod == "POST" && query == "arrived")
            {
                var keyArr = id.Split('_');
                var key = $"{keyArr[0]}_{keyArr[1]}";
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<AgvMissonHub>();
                hubContext.Clients.All.agvStateChange(id);

                var hubContext2 = GlobalHost.ConnectionManager.GetHubContext<NoticeHub>();
                hubContext2.Clients.All.agvSignalChange(id);
                var eventBus = SimpleEventBus.GetDefaultEventBus();
                eventBus.Post(new agvStateChange(id), TimeSpan.FromSeconds(1));

             
                if (keyArr.Length >= 3)
                {

                    hubContext2.Clients.All.agvOrderArrived(id);
                }

                return Json(new { state = true, id = id }, JsonRequestBehavior.AllowGet);
            }
            bool isArrived = true;
            if (StaticData.SignalDict.ContainsKey(id))
            {
                isArrived = StaticData.SignalDict[id];
            }
            var actionStatus = isArrived ? ActionStatus.DONE : ActionStatus.EXECUTING;
            var status = isArrived ? Status.IDLE : Status.EXECUTING;
            var node = new LocationDevice()
            {
                lastAction = query,
                name = id,
                lastActionStatus = Enum.Parse(typeof(ActionStatus), actionStatus.ToString(), false).ToString(),
                status = Enum.Parse(typeof(Status), status.ToString(), false).ToString()
            };
            return Json(node, JsonRequestBehavior.AllowGet);
        }
    }
}