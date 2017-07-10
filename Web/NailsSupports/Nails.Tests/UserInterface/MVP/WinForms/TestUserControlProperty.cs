using NailsFramework.UserInterface;

namespace NailsFramework.Tests.UserInterface.MVP.WinForms
{
    public class TestUserControlProperty : ViewUserControl, ITestView
    {
        public ITestPresenter Presenter { get; set; }
    }
}