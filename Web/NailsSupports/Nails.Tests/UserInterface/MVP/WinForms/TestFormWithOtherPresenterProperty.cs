using NailsFramework.UserInterface;

namespace NailsFramework.Tests.UserInterface.MVP.WinForms
{
    public class TestFormWithOtherPresenterProperty : ViewForm, ITestView
    {
        public ITestPresenter PresenterValue { get; set; }
    }
}