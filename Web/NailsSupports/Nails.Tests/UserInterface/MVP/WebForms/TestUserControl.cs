using NailsFramework.UserInterface;

namespace NailsFramework.Tests.UserInterface.MVP.WebForms
{
    public class TestUserControl : ViewUserControl<ITestPresenter>, ITestView
    {
        public ITestPresenter GetPresenter()
        {
            return Presenter;
        }
    }
}