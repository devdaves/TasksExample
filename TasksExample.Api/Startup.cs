using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using Castle.Windsor;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;
using TasksExample.Api.Infrastructure.Handlers;
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
                new MediatrInstaller(),
                new DefaultInstaller());
            var httpDependencyResolver = new WindsorHttpDependencyResolver(container);

            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.DependencyResolver = httpDependencyResolver;

            // remove xml as supported media type and just return json
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(
                config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml"));

            //var jsonformatter = new JsonMediaTypeFormatter
            //{
            //    SerializerSettings =
            //                        {
            //                            NullValueHandling = NullValueHandling.Ignore
            //                        }
            //};

            //config.Formatters.RemoveAt(0);
            //config.Formatters.Insert(0, jsonformatter);

            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;


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

            config.MessageHandlers.Add(new WrappingHandler());

            appBuilder.UseWebApi(config);
        }

        protected static string GetXmlCommentsPath()
        {
            return $@"{System.AppDomain.CurrentDomain.BaseDirectory}\bin\TasksExample.Api.XML";
        }
    }
}