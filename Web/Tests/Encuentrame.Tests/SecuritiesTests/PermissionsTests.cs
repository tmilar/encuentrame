using Microsoft.VisualStudio.TestTools.UnitTesting;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Tests.Supports;

namespace Encuentrame.Tests.SecuritiesTests
{
    [TestClass]
    public class PermissionsTests : BaseTest
    {
        #region Filters

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {

        }

        [TestInitialize]
        public override void SetUp()
        {
            this.TestsInUnitOfWork = true;
            base.SetUp();
            CreateDatabase();

        }
        [TestCleanup]
        public override void TearDown()
        {
            //Your specific cleanup code
            base.TearDown();
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {

        }

        #endregion

       
    }
}