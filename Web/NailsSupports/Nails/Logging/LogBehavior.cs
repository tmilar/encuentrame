using System;
using System.Text;
using NailsFramework.Aspects;
using NailsFramework.IoC;
using NailsFramework.Support;

namespace NailsFramework.Logging
{
    /// <summary>
    ///   Behavior that logs method invocation
    /// </summary>
    public class LogBehavior : ILemmingBehavior
    {
        [Inject]
        public static ILog Log { private get; set; }

        #region ILemmingBehavior Members

        /// <summary>
        ///   Invocation of the interceptor
        /// </summary>
        /// <param name = "invocation">The invocation info.</param>
        /// <returns>return value of the invoked method</returns>
        public object ApplyTo(MethodInvocationInfo invocation)
        {
            object result;

            // Logging Starting Method
            if (Log.IsDebugEnabled)
            {
                LogStartDebug(invocation);
            }
            else if (Log.IsInfoEnabled)
            {
                LogStartInfo(invocation);
            }

            // Invoke Method
            try
            {
                result = invocation.Proceed();
            }
            catch (Exception ex)
            {
                // Logging Error
                LogError(invocation, ex.GetBaseException());
                throw;
            }

            // Logging Ending Method
            if (Log.IsDebugEnabled)
            {
                LogEndDebug(invocation, result);
            }
            else if (Log.IsInfoEnabled)
            {
                LogEndInfo(invocation);
            }
            return result;
        }

        #endregion

        protected static void LogStartDebug(MethodInvocationInfo invocationInfo)
        {
            Log.Debug("Starting Method " + GetMethodFullInfo(invocationInfo));
        }

        protected static void LogEndDebug(MethodInvocationInfo invocationInfo, object returnObject)
        {
            Log.Debug("Ending Method " + GetMethodFullInfo(invocationInfo) + " return object: " + returnObject);
        }

        protected static void LogStartInfo(MethodInvocationInfo invocationInfo)
        {
            Log.Info("Starting Method " + invocationInfo.Method.FullFriendlyName());
        }

        protected static void LogEndInfo(MethodInvocationInfo invocationInfo)
        {
            Log.Info("Ending Method " + invocationInfo.Method.FullFriendlyName());
        }

        protected static void LogError(MethodInvocationInfo invocationInfo, Exception exception)
        {
            Log.Error("Error Executing Method: " + GetMethodFullInfo(invocationInfo), exception);
        }


        private static string GetMethodFullInfo(MethodInvocationInfo invocationInfo)
        {
            var sb = new StringBuilder();
            sb.Append(invocationInfo.Method.FullFriendlyName());
            sb.Append(" with parameters: ");
            sb.Append(GetArgumentsInfo(invocationInfo));
            return sb.ToString();
        }

        private static string GetArgumentsInfo(MethodInvocationInfo invocationInfo)
        {
            var sb = new StringBuilder();
            if (invocationInfo.Arguments == null)
                return "";
            foreach (var o in invocationInfo.Arguments)
            {
                var argumentValue = o != null ? o.ToString() : "null";
                sb.Append(argumentValue + ", ");
            }
            return sb.ToString();
        }
    }
}