using System.Web;

namespace NailsFramework.UnitOfWork.Session
{
    public class WebSessionContext : DisposableContext, ISessionContext
    {
        protected override void DoSetObject(string key, object val)
        {
            HttpContext.Current.Session[key] = val;
        }

        protected override object DoGetObject(string key)
        {
            return HttpContext.Current.Session[key];
        }

        protected override void DoRemoveObject(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }
    }
}