using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Castle.Windsor;
using Owin;
using Swashbuckle.Application;
using TasksExample.Api.Infrastructure.Windsor;
using TasksExample.Api.Infrastructure.Windsor.Installers;

namespace TasksExample.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var container = new WindsorContainer().Install(
                new ControllerInstaller(),
                new DefaultInstaller());
            var httpDependencyResolver = new WindsorHttpDependencyResolver(container);

            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.DependencyResolver = httpDependencyResolver;

            // remove xml as supported media type and just return json
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(
                config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml"));

            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "Tasks Example API");
                    c.IncludeXmlComments(GetXmlCommentsPath());
                })
                .EnableSwaggerUi(c =>
                {
                    c.DocExpansion(DocExpansion.List);
                });

            appBuilder.UseWebApi(config);
        }

        protected static string GetXmlCommentsPath()
        {
            return $@"{System.AppDomain.CurrentDomain.BaseDirectory}\bin\TasksExample.Api.XML";
        }
    }
}