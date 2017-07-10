using NailsFramework.UserInterface;

namespace NailsFramework.Tests.UserInterface.MVP.WinForms
{
    public class TestUserControlField : ViewUserControl, ITestView
    {
#pragma warning disable 649
        private ITestPresenter presenter;
#pragma warning restore 649

        public ITestPresenter GetPresenter()
        {
            return presenter;
        }
    }
}