using NailsFramework.Mvp;

namespace NailsFramework.Tests.UserInterface.MVP
{
    public interface ITestSingletonPresenter : IPresenter
    {
        int Bla { get; set; }
        ITestSingletonView GetView();
    }
}