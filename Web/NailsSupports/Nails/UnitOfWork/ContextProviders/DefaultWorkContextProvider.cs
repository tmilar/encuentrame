using NailsFramework.IoC;
using NailsFramework.Persistence;
using NailsFramework.UnitOfWork.Session;

namespace NailsFramework.UnitOfWork.ContextProviders
{
    /// <summary>
    ///   Default implementation of the provider object for Work Contexts.
    /// </summary>
    public class DefaultWorkContextProvider : IWorkContextProvider
    {
        /// <summary>
        ///   Key used to access context.
        /// </summary>
        protected const string ContextKey = "Nails.WorkContextProvider.Key";

        [Inject]
        public IExecutionContext ExecutionContext { private get; set; }

        [Inject]
        public IPersistenceContext PersistenceContext { private get; set; }

        #region IWorkContextProvider Members

        /// <summary>
        ///   Returns the current WorkContext or creates a new one.
        ///   This method should never returns null;
        /// </summary>
        /// <value>The WorkContext instance.</value>
        public virtual WorkContext CurrentContext
        {
            get
            {
                var context = GetContextFromStore();
                if (context == null)
                {
                    context = GetNewWorkContext();
                    SetContextToStore(context);
                }
                return context;
            }
        }

        #endregion

        /// <summary>
        ///   Returns a new instance of a Work context.
        /// </summary>
        /// <returns>The new instance.</returns>
        protected virtual WorkContext GetNewWorkContext()
        {
            return new WorkContext(PersistenceContext);
        }

        /// <summary>
        ///   Obtains the current instance from persistent storage.
        /// </summary>
        /// <returns>The current instance or null.</returns>
        protected virtual WorkContext GetContextFromStore()
        {
            return ExecutionContext.GetObject<WorkContext>(ContextKey);
        }

        /// <summary>
        ///   Puts a particular instance into persistent storage.
        /// </summary>
        /// <param name = "context">The instance to store.</param>
        protected virtual void SetContextToStore(WorkContext context)
        {
            ExecutionContext.SetObject(ContextKey, context);
        }
    }
}