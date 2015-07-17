using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Castle.MicroKernel.Registration;

namespace Web.Infrastructure.Injection.Installers
{
    public class MappersInstaller : IWindsorInstaller
    {
        public void Install(Castle.Windsor.IWindsorContainer container, Castle.MicroKernel.SubSystems.Configuration.IConfigurationStore store)
        {
            container.Register(Component.For<IMappingEngine>().UsingFactoryMethod(() => Mapper.Engine));
        }
    }
}