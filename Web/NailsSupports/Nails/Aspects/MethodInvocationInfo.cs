using System;
using System.Reflection;

namespace NailsFramework.Aspects
{
    public class MethodInvocationInfo
    {
        private readonly object[] arguments;
        private readonly Func<object> invocation;
        private readonly MethodBase method;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "MethodInvocationInfo" /> class.
        /// </summary>
        /// <param name = "invocation">Delegate representing the current invocation</param>
        /// <param name = "methodInfo">The method info.</param>
        /// <param name = "arguments">The arguments.</param>
        public MethodInvocationInfo(Func<object> invocation, MethodBase methodInfo, params object[] arguments)
        {
            this.invocation = invocation;
            method = methodInfo;
            this.arguments = arguments;
        }

        /// <summary>
        ///   Gets the method.
        /// </summary>
        /// <value>The method.</value>
        public MethodBase Method
        {
            get { return method; }
        }

        /// <summary>
        ///   Gets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public object[] Arguments
        {
            get { return arguments; }
        }

        /// <summary>
        ///   Proceeds with the invocation.
        /// </summary>
        /// <returns></returns>
        public object Proceed()
        {
            return invocation();
        }
    }
}