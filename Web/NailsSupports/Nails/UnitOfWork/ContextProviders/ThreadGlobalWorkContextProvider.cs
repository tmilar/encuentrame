using System.Runtime.Remoting.Messaging;

namespace NailsFramework.UnitOfWork.ContextProviders
{
    /// <summary>
    ///   This implementation shares the Context instance across 
    ///   all the threads throughout all application domains.
    /// </summary>
    public class ThreadGlobalWorkContextProvider : DefaultWorkContextProvider
    {
        /// <summary>
        ///   This override obtains the current context, but
        ///   uses an intermediate cross-thread object (<see cref = "ThreadAffinitiveContextContainer" />).
        /// </summary>
        /// <returns>The current context or null.</returns>
        protected override WorkContext GetContextFromStore()
        {
            var container = CallContext.GetData(ContextKey) as ThreadAffinitiveContextContainer;
            return container != null ? container.Context : null;
        }

        /// <summary>
        ///   This override wraps the context in a <see cref = "ThreadAffinitiveContextContainer" />.
        /// </summary>
        /// <param name = "context"></param>
        protected override void SetContextToStore(WorkContext context)
        {
            var container = new ThreadAffinitiveContextContainer {Context = context};
            CallContext.SetData(ContextKey, container);
        }
    }
}