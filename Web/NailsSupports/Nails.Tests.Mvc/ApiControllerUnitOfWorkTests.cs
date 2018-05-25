using NailsFramework.Tests.Support;
using NailsFramework.UserInterface.TestingSupport;
using NUnit.Framework;

namespace NailsFramework.Mvc.Tests
{
    [TestFixture]
    public class ApiControllerUnitOfWorkTests : BaseTest
    {
        [Test]
        public void ActionsShouldBeRanWithinUnitOfWorks()
        {
            Nails.Configure()
                .IoC.Container<NailsFramework.IoC.Spring>()
                .InspectAssemblyOf<TestApiController>()
                .UserInterface.Platform(new NailsFramework.UserInterface.Mvc
                                            {
                                                ConfigureContext = false
                                            });

            Nails.Initialize();
            
            var controller = TestControllers.GetApiController<TestApiController>();

            var result = controller.Index();

            Assert.IsTrue(result);
        }
    }
}