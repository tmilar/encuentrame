using System;
using System.Reflection;

namespace NailsFramework.Config
{
    public interface INailsConfigurator
    {
        ILogConfigurator Logging { get; }
        IIoCConfigurator IoC { get; }
        IAspectsConfigurator Aspects { get; }
        IUnitOfWorkConfigurator UnitOfWork { get; }
        IPersistenceConfigurator Persistence { get; }
        IUserInterfaceConfigurator UserInterface { get; }
        IPluginsConfigurator Plugins { get; }
        INailsConfigurator InspectAssemblyOf<T>();
        INailsConfigurator InspectAssemblyOf(Type type);
        INailsConfigurator InspectAssembly(Assembly assembly);
        INailsConfigurator InspectAssembly(string assemblyFile);
        void Initialize(bool configureDefaults = true);
    }
}