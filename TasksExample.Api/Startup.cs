using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Castle.Windsor;
using Owin;
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

            appBuilder.UseWebApi(config);
        }
    }
}