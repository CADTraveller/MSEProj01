using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CostcoProjectStatus
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "ProjectList",
                "ProjectList/{Display}",
                new { Controller="ProjectList", action="Display",id=""}
                );
            routes.MapRoute(
                "StatusUpdateList",
                "ProjectList/{GetStatusUpdates}/{id}",
                new { Controller = "ProjectList", action = "GetStatusUpdates", id = "projectId" }
                );
            routes.MapRoute(
                "ProjectUpdates",
                "ProjectUpdates/{Update}",
                 new { Controller="ProjectUpdates",action="Update"}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
