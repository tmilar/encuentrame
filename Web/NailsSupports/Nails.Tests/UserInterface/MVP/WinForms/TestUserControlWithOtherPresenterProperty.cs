using NailsFramework.UserInterface;

namespace NailsFramework.Tests.UserInterface.MVP.WinForms
{
    public class TestUserControlWithOtherPresenterProperty : ViewUserControl, ITestView
    {
        public ITestPresenter PresenterValue { get; set; }
    }
}