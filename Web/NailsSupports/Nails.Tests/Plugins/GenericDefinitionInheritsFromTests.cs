using NailsFramework.Plugins;
using NailsFramework.Tests.Support;
using NUnit.Framework;
using Rhino.Mocks;

namespace NailsFramework.Tests.Plugins
{
    [TestFixture]
    public class PluginsTests : BaseTest
    {
        [Test]
        public void ShouldAddCustomConfiguration()
        {
            var mocks = new MockRepository();
            var plugin = mocks.DynamicMock<NailsPlugin>();
            plugin.Expect(x => x.AddCustomConfiguration(null)).IgnoreArguments();
            plugin.Expect(x => x.Initialize());

            mocks.ReplayAll();
            Nails.Configure().Plugins.Add(plugin).Initialize();
            mocks.VerifyAll();
        }
    }
}