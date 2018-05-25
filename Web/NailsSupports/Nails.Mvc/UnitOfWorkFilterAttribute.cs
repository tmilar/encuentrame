using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.ContextProviders;
using NailsFramework.UnitOfWork.Session;

namespace NailsFramework.UserInterface
{
    public class UnitOfWorkFilterAttribute : ActionFilterAttribute
    {
        private const string ExecutionKey = "UnitOfWorkFilterAttribute.UnitOfWorkExecution";
        [Inject]
        public IWorkContextProvider WorkContextProvider { private get; set; }

        [Inject]
        public IExecutionContext ExecutionContext { private get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var builder = new UnitOfWorkInfoBuilder
                              {
                                  CustomAttributeProvider = filterContext.ActionDescriptor,
                                  GetUowName = () => GetUowName(filterContext.ActionDescriptor)
                              };

            var info = builder.Build();
            var unitOfWork = WorkContextProvider.CurrentContext.BeginUnitOfWork(info);
            unitOfWork.OnEnd(() => ExecutionContext.RemoveObject(ExecutionKey));
         
            ExecutionContext.SetObject(ExecutionKey, unitOfWork);

            base.OnActionExecuting(filterContext);
        }

        private static string GetUowName(ActionDescriptor actionDescriptor)
        {
            return string.Format("{0}.{1}", actionDescriptor.ControllerDescriptor.ControllerName, actionDescriptor.ActionName);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            var execution = ExecutionContext.GetObject<UnitOfWorkExecution>(ExecutionKey);

            if (filterContext.Exception != null)
                execution.HandleException(execution.Exception);

            execution.End();
        }
    }
}