using NailsFramework.Web;

namespace NailsFramework.UserInterface
{
    public abstract class NailsMvcApplication : NailsHttpApplication
    {
        protected override WebUI GetUI()
        {
            return new Mvc();
        }
    }
}