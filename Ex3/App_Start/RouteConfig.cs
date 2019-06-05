using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ex3
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name:"display", url:"display/{IP}/{port}", 
                defaults: new { Controller = "Flight" , action = "Initial"}
                );
            routes.MapRoute(name: "display_flight", url: "display/{IP}/{port}/{frequency}",
                defaults: new { Controller = "Flight", action = "display_flight" }
                );
            routes.MapRoute(name: "save", url: "save/{IP}/{port}/{frequency}/{time}/{filename}",
                 defaults: new { Controller = "Flight", action = "save" }
                 );
            routes.MapRoute(name: "load", url: "display/{filename}/{frequency}",
                defaults: new { Controller = "Flight", action = "load" }
                );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Flight", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
