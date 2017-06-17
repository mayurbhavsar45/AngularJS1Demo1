using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Routing;
namespace API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //  name: "Web API Resource",
            //  routeTemplate: "api/{controller}/{action}/{id}",
            //  defaults: new { code = RouteParameter.Optional },
            //  constraints: new { id = @"\d+" }
            //  );

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            config.Routes.MapHttpRoute(
                name: "Web API RPC Post",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { action = "Post" },
                constraints: new { action = @"[A-Za-z]+", httpMethod = new HttpMethodConstraint("POST") }
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // GET /api/{resource}/{action}
            config.Routes.MapHttpRoute(
                name: "Web API RPC",
                routeTemplate: "{controller}/{action}",
                defaults: new { },
                constraints: new { action = @"[A-Za-z]+", httpMethod = new HttpMethodConstraint("GET") }
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi1",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { action = @"[A-Za-z]+", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
             name: "APIEMAIL",
             routeTemplate: "api/{controller}/{action}/{Email}",
             defaults: new { action = @"[A-Za-z]+",Email = RouteParameter.Optional }
         );
            config.Routes.MapHttpRoute(
                name: "DefaultApi2",
                routeTemplate: "api/{controller}/{action}/{Email}/{Password}",
                defaults: new { action = @"[A-Za-z]+", Email = RouteParameter.Optional, Password = RouteParameter.Optional }
            );
          
            // GET|PUT|DELETE /api/{resource}/{id}/{code}
            config.Routes.MapHttpRoute(
                name: "Web API Resource",
                routeTemplate: "api/{controller}/{id}/{code}",
                defaults: new { code = RouteParameter.Optional }
                );

            config.Routes.MapHttpRoute(
                name: "Web API Post",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { action = "Post" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );

            config.Routes.MapHttpRoute(
                name: "Web API Post1",
                routeTemplate: "api/{controller}",
                defaults: new { action = "Post" },
                constraints: new { httpMethod = new HttpMethodConstraint("POST") }
                );


            config.Routes.MapHttpRoute("DefaultApiWithId", "api/{controller}/{id}", new { id = RouteParameter.Optional }, new { id = @"\d+" });                 
            config.Routes.MapHttpRoute("DefaultApiWithAction", "api/{controller}/{action}");
            config.Routes.MapHttpRoute("DefaultApiGetEmail", "api/{controller}/{Email}", new { Email = RouteParameter.Optional, action = "Get" }, new { httpMethod = new HttpMethodConstraint("GET") });
            config.Routes.MapHttpRoute("DefaultApiGet", "api/{controller}", new { action = "Get" }, new { httpMethod = new HttpMethodConstraint("GET") });
            config.Routes.MapHttpRoute("DefaultApiPost", "api/{controller}", new { action = "Post" }, new { httpMethod = new HttpMethodConstraint("POST") });
            config.Routes.MapHttpRoute("DefaultApiPut", "api/{controller}", new { action = "Put" }, new { httpMethod = new HttpMethodConstraint("PUT") });
            config.Routes.MapHttpRoute("DefaultApiDelete", "api/{controller}", new { action = "Delete" }, new { httpMethod = new HttpMethodConstraint("DELETE") });


            //config.Routes.MapHttpRoute(name: "DefaultApiWithId", routeTemplate: "Api/{controller}/{id}", defaults: new { id = RouteParameter.Optional }, constraints: new { id = @"\d+" });
            //config.Routes.MapHttpRoute(name: "DefaultApiWithAction", routeTemplate: "Api/{controller}/{action}");

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            config.Formatters.Insert(0, new System.Net.Http.Formatting.JsonMediaTypeFormatter());
            config.Formatters.Insert(0, new System.Net.Http.Formatting.FormUrlEncodedMediaTypeFormatter());

            config.EnableSystemDiagnosticsTracing();
        }
    }
}
