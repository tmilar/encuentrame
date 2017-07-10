using System;

namespace NailsFramework.UnitOfWork
{
    /// <summary>
    ///   Interface that can be used to manually invoke Units of Work.
    /// </summary>
    public interface IUnitOfWorkCommand
    {
        /// <summary>
        ///   Execute the Command.
        /// </summary>
        Object Execute();
    }
}