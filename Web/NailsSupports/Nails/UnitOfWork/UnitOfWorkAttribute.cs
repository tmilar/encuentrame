using System;

namespace NailsFramework.UnitOfWork
{
    /// <summary>
    ///   Decoration that defines a Method/Property as a <see cref = "UnitOfWork" /> as boundary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property)]
    public class UnitOfWorkAttribute : Attribute
    {
        public UnitOfWorkAttribute()
        {
            TransactionMode = Nails.Configuration.DefaultTransactionMode;
        }

        /// <summary>
        ///   Transactional mode of the Unit of Work.
        /// </summary>
        public TransactionMode TransactionMode { get; set; }

        /// <summary>
        ///   Name of the Unit of Work.
        /// </summary>
        public string Name { get; set; }
    }
}