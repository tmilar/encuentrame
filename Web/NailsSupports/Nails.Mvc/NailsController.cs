using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NailsFramework.IoC;

namespace NailsFramework.UserInterface
{
    [Lemming(Singleton = false)]
    [UnitOfWorkFilter]
    public abstract class NailsController : Controller
    {
        [Inject]
        public NailsControllerActionInvoker NailsControllerActionInvoker { private get; set; }

        private readonly Dictionary<Type, Func<Exception, ActionResult>> errorHandlers = new Dictionary<Type, Func<Exception, ActionResult>>();

        protected override IActionInvoker CreateActionInvoker()
        {
            return NailsControllerActionInvoker;
        }
        
        protected void OnError(Func<Exception, ActionResult> onError)
        {
            errorHandlers.Add(typeof(Exception), onError);
        }

        protected void OnError<TException>(Func<TException, ActionResult> onError) where TException : Exception
        {
            errorHandlers.Add(typeof (TException), e => onError((TException) e));
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var exceptionType = filterContext.Exception.GetType();
            var handledExceptionType = errorHandlers.Keys.FirstOrDefault(x => x.IsAssignableFrom(exceptionType));
            
            if (handledExceptionType != null)
            {
                var onError = errorHandlers[handledExceptionType];
                filterContext.Result = onError(filterContext.Exception);
                filterContext.ExceptionHandled = true;
            }

            base.OnException(filterContext);
        }
    }
}
