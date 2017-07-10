namespace NailsFramework.Persistence
{
    public interface IConnectionStringProvider
    {
        string ConnectionString { get; }
        string ConnectionStringName { get;  }

        string ConnectionStringKey { get; }
        string ConnectionStringNameKey { get; }
    }
}