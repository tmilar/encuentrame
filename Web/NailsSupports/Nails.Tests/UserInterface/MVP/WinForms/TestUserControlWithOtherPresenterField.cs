using NailsFramework.UserInterface;

namespace NailsFramework.Tests.UserInterface.MVP.WinForms
{
    public class TestUserControlWithOtherPresenterField : ViewUserControl, ITestView
    {
#pragma warning disable 649
        private ITestPresenter _presenter;
#pragma warning restore 649

        public ITestPresenter GetPresenter()
        {
            return _presenter;
        }
    }
}