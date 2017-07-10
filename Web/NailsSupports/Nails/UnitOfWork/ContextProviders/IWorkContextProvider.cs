namespace NailsFramework.UnitOfWork.ContextProviders
{
    /// <summary>
    ///   Contract for WorkContext providers.
    /// </summary>
    public interface IWorkContextProvider
    {
        /// <summary>
        ///   Returns the current WorkContext or creates a new one.
        ///   This method should never returns null;
        /// </summary>
        /// <value>The WorkContext instance.</value>
        WorkContext CurrentContext { get; }
    }
}