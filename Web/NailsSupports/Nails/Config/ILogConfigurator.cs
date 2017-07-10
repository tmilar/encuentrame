using System;
using NailsFramework.Logging;

namespace NailsFramework.Config
{
    public interface ILogConfigurator : INailsConfigurator
    {
        ILogConfigurator Logger<TLogger>(Action<TLogger> logger) where TLogger : Logger, new();
        ILogConfigurator Logger(Logger logger);
        ILogConfigurator Logger<TLogger>() where TLogger : Logger, new();
    }
}