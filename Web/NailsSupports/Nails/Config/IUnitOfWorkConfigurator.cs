using NailsFramework.UnitOfWork;

namespace NailsFramework.Config
{
    public interface IUnitOfWorkConfigurator : INailsConfigurator
    {
        IUnitOfWorkConfigurator ConnectionBoundUnitOfWork(bool connectionBoundUnitOfWork);
        IUnitOfWorkConfigurator AllowAsyncExecution(bool allowAsyncExecution);
        IUnitOfWorkConfigurator DefaultAsyncMode(bool defaultAsyncMode);
        IUnitOfWorkConfigurator DefaultTransactionMode(TransactionMode defaultTransactionMode);
    }
}