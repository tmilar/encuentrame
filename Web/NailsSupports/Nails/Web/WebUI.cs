using NailsFramework.Config;
using NailsFramework.UnitOfWork.Session;
using NailsFramework.UserInterface;

namespace NailsFramework.Web
{
    public abstract class WebUI : UIPlatform
    {
        public bool ConfigureContext { get; set; }

        protected WebUI()
        {
            ConfigureContext = true;
        }
        public override void AddCustomConfiguration(INailsConfigurator configurator)
        {
            base.AddCustomConfiguration(configurator);
            
            configurator.UnitOfWork.ConnectionBoundUnitOfWork(false);

            if(ConfigureContext)
                configurator
                    .IoC.Lemming<WebExecutionContext>()
                        .Lemming<WebSessionContext>();
        }
    }
}