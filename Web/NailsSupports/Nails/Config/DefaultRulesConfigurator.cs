using System.Linq;
using NailsFramework.Logging;
using NailsFramework.Persistence;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.ContextProviders;
using NailsFramework.UnitOfWork.Session;

namespace NailsFramework.Config
{
    public class DefaultRulesConfigurator
    {
        public void Configure()
        {
            RequireUnitOfWorkForAccessingBags();
            RequireUnitOfWorkForAccessingSessionContext();
            RequirePersistenceSessionForAccessingBags();
            InspectNailsAssembly();
            LogBagsActivities();
            Default<ISessionContext, SingletonSessionContext>();
            Default<IExecutionContext, CallExecutionContext>();
            Default<IWorkContextProvider, DefaultWorkContextProvider>();
        }

        private static void RequirePersistenceSessionForAccessingBags()
        {
            Nails.Configure().Aspects.ApplyBehavior<RunningPersistenceContextValidationBehavior>().ToInheritorsOf(typeof (IBag<>));
        }

        private static void LogBagsActivities()
        {
            Nails.Configure().Aspects.ApplyBehavior<LogBehavior>().ToInheritorsOf(typeof (IBag<>));
        }

        private static void Default<TInterface, TImplementation>() where TImplementation : class, TInterface
        {
            if (!Nails.Configuration.LemmingsSchema.Any(x => typeof (TInterface).IsAssignableFrom(x.ConcreteType)))
                Nails.Configure().IoC.Lemming<TImplementation>();
        }

        private void InspectNailsAssembly()
        {
            Nails.Configure().InspectAssembly(GetType().Assembly);
        }

        private static void RequireUnitOfWorkForAccessingBags()
        {
            var behavior = new RunningUnitOfWorkValidationBehavior();
            behavior.SetTransactionalMethods(typeof (IBag<>), "Put", "Remove");

            Nails.Configure().Aspects.ApplyBehavior(behavior)
                .ExcludingMethods("GetEnumerator")
                .ExcludingProperties("Expression", "ElementType", "Provider")
                .ToInheritorsOf(typeof (IBag<>));
        }

        private static void RequireUnitOfWorkForAccessingSessionContext()
        {
            Nails.Configure().Aspects
                .ApplyBehavior<RunningUnitOfWorkValidationBehavior>()
                .ExcludingMethods("Dispose")
                .ToInheritorsOf<ISessionContext>();
        }
    }
}