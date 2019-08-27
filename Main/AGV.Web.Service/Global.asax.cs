using AGV.Web.Service.AgvHub;
using AGV.Web.Service.Service;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AGV.Web.Service
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


        }
        public void Application_End(object sender, EventArgs e)
        {
            foreach (var item in RestHub.ListAgvJob)
            {

                BackgroundJob.Delete(item);
            }
            if (!string.IsNullOrEmpty(RestHub.WebLeftMaterial))
            {
                BackgroundJob.Delete(RestHub.WebLeftMaterial);
            }

            if (!string.IsNullOrEmpty(RestHub.WebRightMaterial))
            {
                BackgroundJob.Delete(RestHub.WebRightMaterial);
            }
        }


    }
}
