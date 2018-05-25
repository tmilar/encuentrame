using NailsFramework.Aspects;
using NailsFramework.Logging;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.Logging
{
    [TestFixture]
    public class LogBehaviorTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            log = new MockLog();
            behavior = new LogBehavior();
            LogBehavior.Log = log;
        }

        #endregion

        private LogBehavior behavior;
        private MockLog log;

        public string MethodToBeIntercepted(bool throwException)
        {
            if (throwException)
                throw new TestException("expected-exception-12345");

            return "test-value-12345";
        }

        [Test]
        public void DebugLoging()
        {
            log.IsDebugEnabled = true;

            var method = GetType().GetMethod("MethodToBeIntercepted");
            var invocation = new MethodInvocationInfo(() => MethodToBeIntercepted(false), method, false);
            behavior.ApplyTo(invocation);

            Assert.AreEqual(2, log.Lines.Count);

            Assert.That(log.Lines[0].Contains("[DEBUG]"));
            Assert.That(log.Lines[0].Contains("MethodToBeIntercepted"));
            Assert.That(log.Lines[0].Contains("False")); //argument

            Assert.That(log.Lines[1].Contains("[DEBUG]"));
            Assert.That(log.Lines[1].Contains("MethodToBeIntercepted"));
            Assert.That(log.Lines[1].Contains("False")); //argument
            Assert.That(log.Lines[1].Contains("test-value-12345")); //argument
        }

        [Test]
        public void ErrorLoging()
        {
            log.IsDebugEnabled = true;

            var method = GetType().GetMethod("MethodToBeIntercepted");
            var invocation = new MethodInvocationInfo(() => MethodToBeIntercepted(true), method,
                                                      true);
            Assert.Throws<TestException>(() => behavior.ApplyTo(invocation));

            Assert.AreEqual(2, log.Lines.Count);

            Assert.That(log.Lines[1].Contains("[ERROR]"));
            Assert.That(log.Lines[1].Contains("MethodToBeIntercepted"));
            Assert.That(log.Lines[1].Contains("True")); //argument
            Assert.That(log.Lines[1].Contains("expected-exception-12345")); //argument
        }

        [Test]
        public void InfoLoging()
        {
            log.IsDebugEnabled = false;
            log.IsInfoEnabled = true;

            var method = GetType().GetMethod("MethodToBeIntercepted");
            var invocation = new MethodInvocationInfo(() => MethodToBeIntercepted(false), method, false);
            behavior.ApplyTo(invocation);

            Assert.AreEqual(2, log.Lines.Count);

            Assert.That(log.Lines[0].Contains("[INFO]"));
            Assert.That(log.Lines[0].Contains("MethodToBeIntercepted"));
            Assert.That(!log.Lines[0].Contains("False")); //argument

            Assert.That(log.Lines[0].Contains("[INFO]"));
            Assert.That(log.Lines[1].Contains("MethodToBeIntercepted"));
            Assert.That(!log.Lines[1].Contains("False")); //argument
            Assert.That(!log.Lines[1].Contains("test-value-12345")); //argument
        }

        [Test]
        public void ReturnsInvocationValue()
        {
            var method = GetType().GetMethod("MethodToBeIntercepted");
            var invocation = new MethodInvocationInfo(() => MethodToBeIntercepted(false), method, false);
            var result = behavior.ApplyTo(invocation);
            Assert.AreEqual("test-value-12345", result);
        }
    }
}