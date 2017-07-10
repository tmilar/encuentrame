using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NailsFramework.IoC;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.ContextProviders;
using NailsFramework.UnitOfWork.Session;

namespace NailsFramework.UserInterface
{
    public class ApiUnitOfWorkFilterAttribute : ActionFilterAttribute 
    {
        private const string ExecutionKey = "ApiUnitOfWorkFilterAttribute.UnitOfWorkExecution";
        [Inject]
        public IWorkContextProvider WorkContextProvider { private get; set; }
        [Inject]
        public IExecutionContext ExecutionContext { private get; set; }

        private static string GetUowName(HttpActionDescriptor actionDescriptor)
        {
            return string.Format("{0}.{1}", actionDescriptor.ControllerDescriptor.ControllerName, actionDescriptor.ActionName);
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var builder = new UnitOfWorkInfoBuilder
            {
                CustomAttributeProvider = new HttpActionDescriptorToICustomAttributeProviderAdapter(actionContext.ActionDescriptor),
                GetUowName = () => GetUowName(actionContext.ActionDescriptor)
            };

            var info = builder.Build();
            var unitOfWork = WorkContextProvider.CurrentContext.BeginUnitOfWork(info);
            unitOfWork.OnEnd(() => ExecutionContext.RemoveObject(ExecutionKey));

            ExecutionContext.SetObject(ExecutionKey, unitOfWork);

            base.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);

            var execution = ExecutionContext.GetObject<UnitOfWorkExecution>(ExecutionKey);

            if (actionExecutedContext.Exception != null)
                execution.HandleException(execution.Exception);

            execution.End();
        }
    }
}
