using NailsFramework.UserInterface;

namespace NailsFramework.Tests.UserInterface.MVP.WinForms
{
    public class TestFormWithOtherPresenterField : ViewForm, ITestView
    {
        public ITestPresenter _presenter;

        public ITestPresenter GetPresenter()
        {
            return _presenter;
        }
    }
}