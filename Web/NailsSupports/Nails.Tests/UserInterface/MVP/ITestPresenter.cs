using NailsFramework.Mvp;

namespace NailsFramework.Tests.UserInterface.MVP
{
    public interface ITestPresenter : IPresenter
    {
        void Test();
        ITestView GetView();
    }
}