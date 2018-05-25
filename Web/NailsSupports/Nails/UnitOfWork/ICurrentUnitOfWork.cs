using System;

namespace NailsFramework.UnitOfWork
{
    public interface ICurrentUnitOfWork
    {
        UnitOfWorkCache Cache { get; }

        /// <summary>
        ///   Provides the means to define custom behavior that should be executed 
        ///   if the UnitOfWork succeeds.
        /// </summary>
        /// <param name = "handler">The custom behavior.</param>
        void OnSuccessCall(Action handler);

        /// <summary>
        ///   Provides the means to define custom behavior that should be executed 
        ///   if the UnitOfWork fails.
        /// </summary>
        /// <param name = "handler">The custom behavior.</param>
        void OnFailureCall(Action<Exception> handler);

        /// <summary>
        ///   Partially divides the unit of work in several smaller atomic transactions.
        ///   Each time this method is invoked the transactional context will be commited.<br />
        ///   WARNING: Establishing a checkpoint is not undoable, and they cannot be nested. What has been commited CANNOT be rolled back.
        /// </summary>
        void Checkpoint();

        void Cancel();
    }
}