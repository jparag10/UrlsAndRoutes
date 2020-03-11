using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Routing.Constraints;
using System.Web.Routing;

namespace UrlsAndRoutes
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes(); //used for Attribute routing
            //[Route("CustomerIndex")] in CustomerController 
            //browser run /CustomerIndex

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute("MyRoute", "MyApp/{controller}/{action}");
            //routes.MapRoute("MyRoute", "MyApp/X{controller}/{action}");

            //routes.MapRoute("MyRoute", "{action}/{controller}");
            //For1st route browser, Customer/List
            //For 2nd route, List/Customer   ...   route changed

            //routes.MapRoute("MyRoute", "{controller}/{action}",
            //new { controller = "Home", action = "Index" });
            ////new{} Default defined

            //for static segment
            //routes.MapRoute("MyRoute", "Public/{controller}/{action}",
            //    new { controller = "Home", action = "Index" });

            // basic start with H and action must be index or about
            //routes.MapRoute("MyRoute", "{controller}/{action}/{id}",
            //new { controller = "Home", action = "Index",id=UrlParameter.Optional },
            //new { controller = "^H.*", action = "^Index$|^About$"} );

            //routes.MapRoute("MyRoute", "{controller}/{action}/{id}",
            //new { controller = "Home", action = "Index", id = UrlParameter.Optional },
            //new { controller = "^H.*", id = new RangeRouteConstraint(10,20) });
        }
    }
}
