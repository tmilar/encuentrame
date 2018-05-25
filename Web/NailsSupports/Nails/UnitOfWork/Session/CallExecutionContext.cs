using System.Runtime.Remoting.Messaging;

namespace NailsFramework.UnitOfWork.Session
{
    public class CallExecutionContext : DisposableContext, IExecutionContext
    {
        protected override void DoSetObject(string key, object val)
        {
            CallContext.SetData(key,val);
        }

        protected override object DoGetObject(string key)
        {
            return CallContext.GetData(key);
        }

        protected override void DoRemoveObject(string key)
        {
            CallContext.SetData(key, null);
        }
    }
}