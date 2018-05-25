namespace NailsFramework.UnitOfWork
{
    /// <summary>
    ///   Enumeration for the Status of a Unit of Work.
    /// </summary>
    public enum UnitOfWorkStatus
    {
        /// <summary>
        ///   Not started yet.
        /// </summary>
        NotStarted,
        /// <summary>
        ///   Currently running.
        /// </summary>
        Running,
        /// <summary>
        ///   Completed successfully.
        /// </summary>
        Successfull,
        /// <summary>
        ///   Failed to complete.
        /// </summary>
        Failed,
        Canceled
    }
}