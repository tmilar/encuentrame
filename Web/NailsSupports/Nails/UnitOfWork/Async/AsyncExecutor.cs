using System;

namespace NailsFramework.UnitOfWork.Async
{
    public static class AsyncExecutor
    {
        private static readonly Action<Delegate, object[]> DynamicInvokeShimProcDelegate = DynamicInvokeShim;

        /// <summary>
        ///   Fires the specified action.
        /// </summary>
        /// <param name = "action">The action.</param>
        /// <param name = "args">The action arguments.</param>
        public static void FireAndForget(Delegate action, params object[] args)
        {
            DynamicInvokeShimProcDelegate.BeginInvoke(action, args, DynamicInvokeDone, null);
        }

        private static void DynamicInvokeShim(Delegate action, object[] args)
        {
            action.DynamicInvoke(args);
        }

        private static void DynamicInvokeDone(IAsyncResult asyncResult)
        {
            DynamicInvokeShimProcDelegate.EndInvoke(asyncResult);
        }
    }
}