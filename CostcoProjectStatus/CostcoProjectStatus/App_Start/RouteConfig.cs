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
                "ProjectList/{GetProjectUpdates}/{id}",
                new { Controller = "ProjectList", action = "GetStatusUpdates", id = "id" }
                );
            routes.MapRoute(
                "StatusDataList",
                "ProjectList/{GetStatusData}/{projectId}/{phaseId}/{statusSequence}",
                new { Controller = "ProjectList", action = "GetStatusData", projectId = "projectId", phaseId = "phaseId", statusSequence = "statusSequence"  }
                );
            routes.MapRoute(
                "ProjectUpdate",
                "ProjectUpdate/{Update}",
                 new { Controller="ProjectUpdate",action="Update"}
                );
            routes.MapRoute(
                "Account",
                "Account/IsLogged",
                new { Controller = "Account", action = "IsLogin" }
                );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ProjectList", action = "Display", id = "" }
            );
        }
    }
}
