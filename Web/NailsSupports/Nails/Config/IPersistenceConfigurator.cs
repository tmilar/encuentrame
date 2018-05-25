using System;
using NailsFramework.Persistence;

namespace NailsFramework.Config
{
    public interface IPersistenceConfigurator : INailsConfigurator
    {
        IPersistenceConfigurator DataMapper(DataMapper dataMapper);
        IPersistenceConfigurator DataMapper<TDataMapper>(Action<TDataMapper> dataMapper = null) where TDataMapper : DataMapper, new();
        IPersistenceConfigurator PageSize(int pageSize);
    }
}