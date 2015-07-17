using System;
using System.Web.Http.Controllers;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Web.Infrastructure.Common;

namespace Web.Infrastructure.Injection.Installers
{
    /// <summary>
    /// Windsor.Castle ControllerInstaller
    /// see http://docs.castleproject.org/Windsor.Windsor-tutorial-ASP-NET-MVC-3-application-To-be-Seen.ashx
    /// </summary>
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");

            // Register API Controllers
            container.Register(Classes.FromThisAssembly()
                                .BasedOn<IHttpController>()
                                .LifestyleTransient());

            // Register Worker Services
            //container.Register(Component.For<ToDoWorker>().LifeStyle.Transient);
        }

    }
}