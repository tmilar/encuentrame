using System;
using NailsFramework.Config;
using NailsFramework.Support;

namespace NailsFramework.Logging
{
    public abstract class Logger : NailsComponent
    {
        public abstract Type LogType { get; }

        public virtual void ConfigureLogLemming(LemmingConfigurator loglemming)
        {
        }
    }
}