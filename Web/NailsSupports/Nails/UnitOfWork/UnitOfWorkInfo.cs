using System.Reflection;

namespace NailsFramework.UnitOfWork
{
    /// <summary>
    ///   Information that describes a Unit of Work in particular.
    /// </summary>
    public class UnitOfWorkInfo
    {
        /// <summary>
        ///   Constructor.
        /// </summary>
        public UnitOfWorkInfo() : this(true)
        {
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name = "isTransactional">If the Unit of Work is transactional.</param>
        public UnitOfWorkInfo(bool isTransactional)
            : this(isTransactional, false)
        {
        }

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name = "isTransactional">If the Unit of Work is transactional.</param>
        /// <param name = "isAsync">If the Unit of Work should be run asynchronously.</param>
        public UnitOfWorkInfo(bool isTransactional, bool isAsync)
        {
            IsAsync = isAsync;
            IsTransactional = isTransactional;
        }

        /// <summary>
        ///   Is the Unit of Work Transactional.
        /// </summary>
        public bool IsTransactional { get; private set; }

        /// <summary>
        ///   Should the Unit of Work be run asynchronously.
        /// </summary>
        public bool IsAsync { get; private set; }

        private static string GetUowName(MethodBase method)
        {
            return method.DeclaringType.FullName + "." + method.Name;
        }

        /// <summary>
        ///   Creates and returns a <see cref = "UnitOfWorkInfo" /> for a particular MethodInfo.
        /// </summary>
        /// <param name = "method">MethodInfo.</param>
        /// <returns>The UnitOfWorkInfo.</returns>
        public static UnitOfWorkInfo From(MethodBase method)
        {
            var methodInfo = method as MethodInfo;
            var builder = new UnitOfWorkInfoBuilder
                              {
                                  AllowAsync = methodInfo != null && methodInfo.ReturnParameter.ParameterType == typeof (void),
                                  CustomAttributeProvider = methodInfo,
                                  GetUowName = () => GetUowName(method)
                              };
            return builder.Build();
        }
    }
}