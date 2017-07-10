using System.Collections.Generic;
using System.Reflection;
using NailsFramework.Aspects;
using NailsFramework.IoC;
using NailsFramework.Plugins;
using NailsFramework.UnitOfWork;

namespace NailsFramework.Config
{
    public interface INailsConfiguration
    {
        bool ConnectionBoundUnitOfWork { get; }
        bool DefaultAsyncMode { get; }
        bool AllowAsyncExecution { get; }
        int PageSize { get; }
        TransactionMode DefaultTransactionMode { get; }
        IEnumerable<Lemming> LemmingsSchema { get; }
        IEnumerable<Assembly> AssembliesToInspect { get; }
        IEnumerable<Injection> StaticInjections { get; }
        IEnumerable<Aspect> Aspects { get; }
        IEnumerable<NailsPlugin> Plugins { get; }
    }
}