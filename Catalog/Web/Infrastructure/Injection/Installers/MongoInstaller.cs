using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Web.Areas.Mongo.Services;
using Web.Areas.Mongo.Workers;

namespace Web.Infrastructure.Injection.Installers
{
    public class MongoInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Register Worker Services
            container.Register(Component.For<ICatalogWorker>().ImplementedBy<CatalogWorker>().LifeStyle.Transient);
            container.Register(Component.For<ICatalogRepository>().ImplementedBy<CatalogRepository>().LifeStyle.Transient);
        }
    }
}