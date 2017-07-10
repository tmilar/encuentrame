using System.Web.Mvc;
using NailsFramework.Config;
using NailsFramework.Web;
using System.Web.Http;

#if MVC4
using System.Web.Http;
#endif

namespace NailsFramework.UserInterface
{
    public class Mvc : WebUI
    {
        public override void AddCustomConfiguration(INailsConfigurator configurator)
        {
            base.AddCustomConfiguration(configurator);
            configurator.IoC.InspectAssemblyOf<Mvc>();
        }
        public override void Initialize()
        {
            base.Initialize();
            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(Nails.ObjectFactory));

#if MVC4
            GlobalConfiguration.Configuration.DependencyResolver = new DependencyResolver(Nails.ObjectFactory);
            GlobalConfiguration.Configuration.Services.Replace(typeof(System.Web.Http.Filters.IFilterProvider), Nails.ObjectFactory.Inject(new NailsApiFilterProvider()));
#endif
#if MVC5
            GlobalConfiguration.Configuration.DependencyResolver = new DependencyResolver(Nails.ObjectFactory);
            GlobalConfiguration.Configuration.Services.Replace(typeof(System.Web.Http.Filters.IFilterProvider), Nails.ObjectFactory.Inject(new NailsApiFilterProvider()));
#endif
        }
    }
}