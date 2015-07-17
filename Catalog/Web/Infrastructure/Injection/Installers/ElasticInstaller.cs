using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Web.Areas.Elastic.Services;
using Web.Areas.Elastic.Workers;

namespace Web.Infrastructure.Injection.Installers
{
    public class ElasticInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Register Worker Services
            container.Register(Component.For<ICatalogWorker>().ImplementedBy<CatalogWorker>().LifeStyle.Transient);
            container.Register(Component.For<ICatalogRepository>().ImplementedBy<CatalogRepository>().LifeStyle.Transient);
        }
    }
}