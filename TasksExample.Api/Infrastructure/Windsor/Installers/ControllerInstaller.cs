using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TasksExample.Api.Infrastructure.Data;

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
        }
    }
}