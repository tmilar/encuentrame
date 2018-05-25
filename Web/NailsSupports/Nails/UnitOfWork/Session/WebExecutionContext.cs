using System.Web;

namespace NailsFramework.UnitOfWork.Session
{
    public class WebExecutionContext : DisposableContext, IExecutionContext
    {
        protected override void DoSetObject(string key, object val)
        {
            HttpContext.Current.Items.Add(key, val);
        }

        protected override object DoGetObject(string key)
        {
            return HttpContext.Current.Items[key];
        }

        protected override void DoRemoveObject(string key)
        {
            HttpContext.Current.Items.Remove(key);
        }
    }
}