using NailsFramework.Mvp;

namespace NailsFramework.Tests.UserInterface.MVP
{
    public class TestSingletonPresenter : SingletonPresenter<ITestSingletonView>, ITestSingletonPresenter
    {
        #region ITestSingletonPresenter Members

        public ITestSingletonView GetView()
        {
            return View;
        }

        public int Bla { get; set; }

        #endregion
    }
}