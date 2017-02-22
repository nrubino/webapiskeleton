using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using webapiskeleton.Filters;
using webapiskeleton.Unity;

namespace webapiskeleton
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // CORS stuff for angular/client apps
            //config.SetCorsPolicyProviderFactory();
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*") { SupportsCredentials = true });

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));

            config.Formatters.XmlFormatter.UseXmlSerializer = true;

            //Call unity to register injectable classes
            UnityConfig.RegisterTypes(config);

            //Configure filters here
            config.Filters.Add(new ExceptionFilter());
            config.Filters.Add(new LoggingFilter());
        }
    }
}
