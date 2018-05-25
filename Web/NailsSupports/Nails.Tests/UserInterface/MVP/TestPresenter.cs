using NailsFramework.Mvp;

namespace NailsFramework.Tests.UserInterface.MVP
{
    public class TestPresenter : Presenter<ITestView>, ITestPresenter
    {
        #region ITestPresenter Members

        public ITestView GetView()
        {
            return View;
        }

        public void Test()
        {
        }

        #endregion
    }
}