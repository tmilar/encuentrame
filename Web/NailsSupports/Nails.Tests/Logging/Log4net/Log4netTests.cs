using System;
using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Logging;
using NailsFramework.Tests.Support;
using NUnit.Framework;

namespace NailsFramework.Tests.Logging.Log4net
{
    [TestFixture]
    public class Log4netTests : BaseTest
    {
        private const string LogMessage = "Message to be logged";
        private const string LogMessageFormat = "Formatted message to be logged for {0} and {1}";
        private const string LogMessageFormatArgument1 = "formatted argument 1";
        private const string LogMessageFormatArgument2 = "second formatted argument";

        private static string LogMessageFormatted()
        {
            return String.Format(LogMessageFormat, LogMessageFormatArgument1, LogMessageFormatArgument2);
        }

        [Inject]
        public string LogFileName { get; set; }

        [Inject]
        public string LoggerName { get; set; }

        [Inject]
        public ILog Log { private get; set; }
        private Log4netFileLogHelper fileLogHelper;

        [SetUp]
        public void SetUp()
        {
            Nails.Configure()
               .IoC.Container<NailsFramework.IoC.Spring>()
               .Persistence.DataMapper<NailsFramework.Persistence.Memory>()
               .Logging.Logger<NailsFramework.Logging.Log4net>()
               .Initialize();

            Nails.ObjectFactory.Inject(this);
            Log.LogName = LoggerName;
            fileLogHelper = new Log4netFileLogHelper(LogFileName);
        }

        [TearDown]
        public void TearDown()
        {
            fileLogHelper.DeleteFile();
        }

        #region Literal Messages
        [Test]
        public void TestDebugEntryShouldBeWritten() {
            Log.Debug(LogMessage);

            var logLines = fileLogHelper.GetLines().ToArray();
            Assert.AreEqual(1, logLines.Count());
            StringAssert.Contains(LogMessage, logLines[0]);
            StringAssert.Contains("DEBUG", logLines[0]);
        }

        [Test]
        public void TestInfoEntryShouldBeWritten()
        {
            Log.Info(LogMessage);

            var logLines = fileLogHelper.GetLines().ToArray();
            Assert.AreEqual(1, logLines.Count());
            StringAssert.Contains(LogMessage, logLines[0]);
            StringAssert.Contains("INFO", logLines[0]);
        }

        [Test]
        public void TestWarnEntryShouldBeWritten()
        {
            Log.Warn(LogMessage);

            var logLines = fileLogHelper.GetLines().ToArray();
            Assert.AreEqual(1, logLines.Count());
            StringAssert.Contains(LogMessage, logLines[0]);
            StringAssert.Contains("WARN", logLines[0]);
        }

        [Test]
        public void TestErrorEntryShouldBeWritten()
        {
            Log.Error(LogMessage);

            var logLines = fileLogHelper.GetLines().ToArray();
            Assert.AreEqual(1, logLines.Count());
            StringAssert.Contains(LogMessage, logLines[0]);
            StringAssert.Contains("ERROR", logLines[0]);
        }

        [Test]
        public void TestFatalEntryShouldBeWritten()
        {
            Log.Fatal(LogMessage);

            var logLines = fileLogHelper.GetLines().ToArray();
            Assert.AreEqual(1, logLines.Count());
            StringAssert.Contains(LogMessage, logLines[0]);
            StringAssert.Contains("FATAL", logLines[0]);
        }
        #endregion

        #region Formatted Messages
        [Test]
        public void TestDebugFormatEntryShouldBeWritten()
        {
            Log.DebugFormat(LogMessageFormat, LogMessageFormatArgument1, LogMessageFormatArgument2);

            var logLines = fileLogHelper.GetLines().ToArray();
            Assert.AreEqual(1, logLines.Count());
            StringAssert.Contains(LogMessageFormatted(), logLines[0]);
            StringAssert.Contains("DEBUG", logLines[0]);
        }

        [Test]
        public void TestInfoFormatEntryShouldBeWritten()
        {
            Log.InfoFormat(LogMessageFormat, LogMessageFormatArgument1, LogMessageFormatArgument2);

            var logLines = fileLogHelper.GetLines().ToArray();
            Assert.AreEqual(1, logLines.Count());
            StringAssert.Contains(LogMessageFormatted(), logLines[0]);
            StringAssert.Contains("INFO", logLines[0]);
        }

        [Test]
        public void TestWarnFormatEntryShouldBeWritten()
        {
            Log.WarnFormat(LogMessageFormat, LogMessageFormatArgument1, LogMessageFormatArgument2);

            var logLines = fileLogHelper.GetLines().ToArray();
            Assert.AreEqual(1, logLines.Count());
            StringAssert.Contains(LogMessageFormatted(), logLines[0]);
            StringAssert.Contains("WARN", logLines[0]);
        }

        [Test]
        public void TestErrorFormatEntryShouldBeWritten()
        {
            Log.ErrorFormat(LogMessageFormat, LogMessageFormatArgument1, LogMessageFormatArgument2);

            var logLines = fileLogHelper.GetLines().ToArray();
            Assert.AreEqual(1, logLines.Count());
            StringAssert.Contains(LogMessageFormatted(), logLines[0]);
            StringAssert.Contains("ERROR", logLines[0]);
        }

        [Test]
        public void TestFatalFormatEntryShouldBeWritten()
        {
            Log.FatalFormat(LogMessageFormat, LogMessageFormatArgument1, LogMessageFormatArgument2);

            var logLines = fileLogHelper.GetLines().ToArray();
            Assert.AreEqual(1, logLines.Count());
            StringAssert.Contains(LogMessageFormatted(), logLines[0]);
            StringAssert.Contains("FATAL", logLines[0]);
        }
        #endregion
    }
}
