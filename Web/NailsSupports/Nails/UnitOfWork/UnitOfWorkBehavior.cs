using NailsFramework.Aspects;
using NailsFramework.IoC;
using NailsFramework.UnitOfWork.ContextProviders;

namespace NailsFramework.UnitOfWork
{
    /// <summary>
    ///   Behavior which wraps an invocation of a method within a Unit of Work.
    /// </summary>
    [Lemming]
    public class UnitOfWorkBehavior : ILemmingBehavior
    {
        [Inject]
        public static IWorkContextProvider ContextProvider { private get; set; }

        #region ILemmingBehavior Members

        public object ApplyTo(MethodInvocationInfo invocation)
        {
            var uowinfo = UnitOfWorkInfo.From(invocation.Method);
            if (uowinfo.IsAsync)
            {
                ContextProvider.CurrentContext.RunUnitOfWorkAsync(new UnitOfWorkInterceptorCommand(invocation), uowinfo);
                return null;
            }

            return ContextProvider.CurrentContext.RunUnitOfWork(new UnitOfWorkInterceptorCommand(invocation), uowinfo);
        }

        #endregion
    }
}