namespace NailsFramework.UnitOfWork
{
    /// <summary>
    ///   Valid Transaction Modes.
    /// </summary>
    public enum TransactionMode
    {
        /// <summary>
        ///   Transactional mode.
        /// </summary>
        UseTransaction,
        /// <summary>
        ///   Non Transactional mode.
        /// </summary>
        NoTransaction
    }
}