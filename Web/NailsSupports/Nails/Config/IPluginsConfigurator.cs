using System;
using NailsFramework.Plugins;

namespace NailsFramework.Config
{
    public interface IPluginsConfigurator : INailsConfigurator
    {
        INailsConfigurator Add<TPlugin>(Action<TPlugin> plugin) where TPlugin : NailsPlugin, new();
        INailsConfigurator Add(NailsPlugin plugin);
    }
}