using System;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TasksExample.Api.Infrastructure.Data;
using TasksExample.Api.Models;

namespace TasksExample.Api.Infrastructure.Windsor.Installers
{
    public class ControllerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
            .Pick().If(t => t.Name.EndsWith("Controller"))
            .Configure(configurer => configurer.Named(configurer.Implementation.Name))
            .LifestylePerWebRequest());

            //cheeting way to make persisted in memory data store...
            container.Register(Component.For<ITasksContext>().ImplementedBy<TasksContext>().LifestyleSingleton());

            container.Register(
                Component.For<IRequestInfo>()
                    .UsingFactoryMethod(c =>
                    {
                        var x = new RequestInfo() {TransactionId = Guid.NewGuid()};
                        return x;
                    })
                    .LifestylePerWebRequest());

            container.AddFacility<LoggingFacility>(f => f.LogUsing(LoggerImplementation.NLog).WithConfig("nlog.config"));
        }
    }
}