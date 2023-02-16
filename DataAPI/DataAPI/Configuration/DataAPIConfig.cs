using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace DataAPI.Configuration
{
    public class DataAPIConfig
    {
        public static void Register (HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }

            //    );

            IHttpRoute defaultRoute = config.Routes.CreateRoute(
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional },
                null
                );
            config.Routes.Add("DefaultApi", defaultRoute);
        }
    }
}