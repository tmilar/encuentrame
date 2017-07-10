using System.Collections.Generic;

namespace NailsFramework.Config
{
    public interface IConfigurationStatus
    {
        IEnumerable<MissingConfiguration> MissingConfigurations { get; }
        bool IsReady { get; }
    }
}