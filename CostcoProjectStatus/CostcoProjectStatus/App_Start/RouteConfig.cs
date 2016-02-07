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
            //routes.MapRoute(
            //    "Account",
            //    "Account/{ExternalLogin}",
            //     new { Controller = "Account", action = "ExternalLogin" }
            //    );

            routes.MapRoute(
                "ProjectList",
                "ProjectList/{Display}",
                new { Controller="ProjectList", action="Display",id=""}
                );
            routes.MapRoute(
                "Account",
                "Account/{ExternalLogin}",
                new { Controller = "ProjectList", action = "ExternalLogin", id = "" }
                );
            routes.MapRoute(
                "StatusUpdateList",
                "ProjectList/{GetStatusUpdates}/{id}",
                new { Controller = "ProjectList", action = "GetStatusUpdates", id = "projectId" }
                );
            routes.MapRoute(
                "StatusDataList",
                "ProjectList/{GetStatusData}/{projectId}/{phaseId}/{statusSequence}",
                new { Controller = "ProjectList", action = "GetStatusData", projectId = "projectId", phaseId = "phaseId", statusSequence = "statusSequence"  }
                );
            routes.MapRoute(
                "ProjectUpdates",
                "ProjectUpdates/{Update}",
                 new { Controller="ProjectUpdates",action="Update"}
                );
            //routes.MapRoute(
            //    name: "LoginOverride",
            //    url: "{Dashboard}/{*.}",
            //    defaults: new { controller = "Projectlist", action = "Index" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
