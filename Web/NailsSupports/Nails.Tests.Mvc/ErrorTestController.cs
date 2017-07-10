using System;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.ContextProviders;
using NailsFramework.UserInterface;

namespace NailsFramework.Mvc.Tests
{
    public class ErrorTestController : NailsController
    {
        [Inject]
        public ICurrentUnitOfWork CurrentUnitOfWork { get; set; }
       
        private ActionResult HandleError(Exception e)
        {
            ViewData["Exception"] = e;
            return View("ex");
        }

        public virtual ActionResult ExceptionHandled()
        {
            OnError(HandleError);
            throw new Exception();
        }

        public ErrorTestController()
        {
            ControllerContext = new ControllerContext();
        }
        public virtual void ForceOnException(Exception e)
        {
            OnException(new ExceptionContext(ControllerContext, e));
        }

        public virtual ActionResult NoError()
        {
            ViewData["Executed"] = true;
            return View();
        }

        public virtual ActionResult SpecificExceptionHandled()
        {
            OnError<InvalidOperationException>(HandleError);
            throw new InvalidOperationException();
        }

        public virtual ActionResult SpecificExceptionHandledBySuper()
        {
            OnError(HandleError);
            throw new InvalidOperationException(); 
        }

        public virtual ActionResult ExceptionUnhandled()
        {
            OnError<InvalidOperationException>(HandleError);
            throw new Exception();
        }

        public virtual ActionResult ExceptionWithNoHandledExceptions()
        {
            throw new Exception();
        }

        public virtual ActionResult HandleAndRollback()
        {
            OnError<Exception>(HandleError);
            CurrentUnitOfWork.OnFailureCall(e => ViewData["UOWException"] = e);
            throw new Exception();
        }

        [Inject]
        public virtual IWorkContextProvider WorkContextProvider { get; set; }
            
    }
}