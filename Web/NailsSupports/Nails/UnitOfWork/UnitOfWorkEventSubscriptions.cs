using System;

namespace NailsFramework.UnitOfWork
{
    public class UnitOfWorkEventSubscriptions
    {
        private Action<Exception> onFailure = x => { };
        private Action onSuccess = () => { };

        /// <summary>
        ///   Provides the means to define custom behavior that should be executed 
        ///   if the UnitOfWork succeeds.
        /// </summary>
        /// <param name = "handler">The custom behavior.</param>
        public void OnSuccessCall(Action handler)
        {
            onSuccess += handler;
        }

        /// <summary>
        ///   Provides the means to define custom behavior that should be executed 
        ///   if the UnitOfWork fails.
        /// </summary>
        /// <param name = "handler">The custom behavior.</param>
        public void OnFailureCall(Action<Exception> handler)
        {
            onFailure += handler;
        }

        /// <summary>
        ///   Executes OnSuccess or OnFailureCall, depending on the exception.
        /// </summary>
        /// <param name = "uowException">Exception that occured or null.</param>
        public void ExecuteCalls(Exception uowException)
        {
            if (uowException != null)
                onFailure(uowException);
            else
                onSuccess();
        }
    }
}