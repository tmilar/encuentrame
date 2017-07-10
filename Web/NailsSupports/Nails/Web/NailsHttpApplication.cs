using System.Web;
using NailsFramework.Config;
using NailsFramework.Persistence;

namespace NailsFramework.Web
{
    public abstract class NailsHttpApplication : HttpApplication
    {
        private static bool configured;
        private readonly static object SyncObject = new object();

#warning If initialization fails, shouldn't we throw the exception directly rather than masking it and get possible errors on application execution?
        public override void Init()
        {
            base.Init();

            if (!configured)
            {
                lock (SyncObject)
                {
                    if (!configured)
                    {
                        configured = true;

                        var configuration = Nails.Configure()
                            .UserInterface.Platform(GetUI());

                        ConfigureNails(configuration);

                        configuration.Initialize();

                        if (!Nails.Status.IsReady)
                            return;

                        PostNailsInitialize();
                    }
                }
            }

            if (!Nails.Status.IsReady) return;
            BeginRequest += delegate { Nails.ObjectFactory.GetObject<IPersistenceContext>().OpenSession(); };
            EndRequest += delegate { Nails.ObjectFactory.GetObject<IPersistenceContext>().CloseSession(); };
        }

        protected abstract void ConfigureNails(INailsConfigurator nails);
        protected virtual void PostNailsInitialize()
        {
        }
        protected abstract WebUI GetUI();
    }
}