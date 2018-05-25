using System;
using NailsFramework.IoC;
using NailsFramework.UnitOfWork.ContextProviders;

namespace NailsFramework.UnitOfWork
{
    /// <summary>
    ///   Provides easy access to  functionality related to the current Unit of Work, 
    ///   such as custom event behavior (OnSuccess, OnFailure, etc).
    /// </summary>
    [Lemming]
    public class CurrentUnitOfWork : ICurrentUnitOfWork
    {
        [Inject]
        public IWorkContextProvider WorkContextProvider { private get; set; }

        #region ICurrentUnitOfWork Members

        /// <summary>
        ///   Provides the means to define custom behavior that should be executed 
        ///   if the UnitOfWork succeeds.
        /// </summary>
        /// <param name = "handler">The custom behavior.</param>
        public void OnSuccessCall(Action handler)
        {
            GetCurrent().Subscriptions.OnSuccessCall(handler);
        }

        /// <summary>
        ///   Provides the means to define custom behavior that should be executed 
        ///   if the UnitOfWork fails.
        /// </summary>
        /// <param name = "handler">The custom behavior.</param>
        public void OnFailureCall(Action<Exception> handler)
        {
            GetCurrent().Subscriptions.OnFailureCall(handler);
        }

        /// <summary>
        ///   Partially divides the unit of work in several smaller atomic transactions.
        ///   Each time this method is invoked the transactional context will be commited.<br />
        ///   WARNING: Establishing a checkpoint is not undoable, and they cannot be nested. What has been commited CANNOT be rolled back.
        /// </summary>
        public void Checkpoint()
        {
            GetCurrent().Checkpoint();
        }

        public UnitOfWorkCache Cache
        {
            get { return GetCurrent().Cache; }
        }

        #endregion

        private UnitOfWork GetCurrent()
        {
            var context = WorkContextProvider.CurrentContext;
            if (!context.IsUnitOfWorkRunning)
                throw new NailsException("Not unit of work is running");

            return context.CurrentUnitOfWork;
        }

        public void Cancel()
        {
            GetCurrent().Cancel();
        }
    }
}