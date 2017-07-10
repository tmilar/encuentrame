using System.Runtime.Remoting.Messaging;

namespace NailsFramework.UnitOfWork.ContextProviders
{
    /// <summary>
    ///   Cross-thread WorkContext intance Container.
    ///   <br />
    ///   This is required in order to have the instance cross threads
    ///   and AppDomains.
    /// </summary>
    public class ThreadAffinitiveContextContainer : ILogicalThreadAffinative
    {
        /// <summary>
        ///   Instance that is contained.
        /// </summary>
        public WorkContext Context { get; set; }
    }
}