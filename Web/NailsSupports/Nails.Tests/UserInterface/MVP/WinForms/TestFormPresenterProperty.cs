using NailsFramework.UserInterface;

namespace NailsFramework.Tests.UserInterface.MVP.WinForms
{
    public class TestFormPresenterProperty : ViewForm, ITestView
    {
        public ITestPresenter Presenter { get; set; }
    }
}