using NailsFramework.UserInterface;

namespace NailsFramework.Tests.UserInterface.MVP.WebForms
{
    public class TestPage : ViewPage<ITestPresenter>, ITestView
    {
        public ITestPresenter GetPresenter()
        {
            return Presenter;
        }
    }
}