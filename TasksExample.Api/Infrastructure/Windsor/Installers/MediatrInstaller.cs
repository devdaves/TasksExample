using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using MediatR;

namespace TasksExample.Api.Infrastructure.Windsor.Installers
{
    public class MediatrInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Kernel.AddHandlersFilter(new ContravariantFilter());
            container.Register(Classes.FromThisAssembly().Pick().WithServiceAllInterfaces());
            container.Register(Component.For<IMediator>().ImplementedBy<Mediator>());
            container.Register(Component.For<SingleInstanceFactory>().UsingFactoryMethod<SingleInstanceFactory>(k => t => Resolve(k, t)));
            container.Register(Component.For<MultiInstanceFactory>().UsingFactoryMethod<MultiInstanceFactory>(k => t => (IEnumerable<object>)k.ResolveAll(t)));
        }

        private static object Resolve(IKernel k, Type t)
        {
            try
            {
                return k.Resolve(t);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}