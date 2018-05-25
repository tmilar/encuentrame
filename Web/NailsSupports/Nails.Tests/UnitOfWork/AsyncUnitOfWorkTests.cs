using System;
using System.Diagnostics;
using System.Threading;
using NailsFramework;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NailsFramework.Tests.Support;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.ContextProviders;
using NUnit.Framework;
using Rhino.Mocks;

namespace NailsFramework.Tests.UnitOfWork
{

    [TestFixture]
    public class AsyncUnitOfWorkTests : BaseTest
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();

            failureCall = false;
            successCall = false;
            asyncWorked = false;


            Nails.Configure()
                .IoC.Container<NullIoCContainer>()
                .Initialize();

            workContextProvider = mocks.DynamicMock<IWorkContextProvider>();
            persistenceContext = mocks.DynamicMock<IPersistenceContext>();
            workContext = new WorkContext(persistenceContext);
            workContextProvider.Expect(x => x.CurrentContext).Return(workContext).Repeat.Any();
        }

        #endregion

        private const int AsyncTimeout = 10000;

        private bool failureCall;
        private bool successCall;

        private MockRepository mocks;
        private IWorkContextProvider workContextProvider;
        private WorkContext workContext;
        private IPersistenceContext persistenceContext;
        private bool asyncWorked;

        public void BeginEventEvenIfAnExceptionWasThrown()
        {
            mocks.ReplayAll();

            workContext.Begin += delegate { AsyncDone(); };

            workContext.RunUnitOfWorkAsync(() => { throw new TestException(); }, new UnitOfWorkInfo(false, true));

            Wait();
        }

        private void TestSuscriptions(bool throwException)
        {
            workContext.CurrentUnitOfWork.Subscriptions.OnSuccessCall(() => { successCall = true; });
            workContext.CurrentUnitOfWork.Subscriptions.OnFailureCall(e => { failureCall = true; });

            AsyncDone();

            if (throwException)
                throw new Exception("expected");
        }


        private void AsyncDone()
        {
            asyncWorked = true;
        }

        private void Wait()
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                while (!asyncWorked)
                {
                    if (sw.ElapsedMilliseconds > AsyncTimeout)
                        Assert.Fail("Async call didn't work");

                    Thread.Sleep(1000);
                }
            }
            finally
            {
                sw.Stop();
            }
        }

        [Test]
        public void BeginEvent()
        {
            mocks.ReplayAll();
            workContext.Begin += delegate { AsyncDone(); };

            workContext.RunUnitOfWorkAsync(() => { }, new UnitOfWorkInfo(false, true));

            Wait();
        }

        [Test]
        public void EndEvent()
        {
            mocks.ReplayAll();
            workContext.End += delegate { AsyncDone(); };

            workContext.RunUnitOfWorkAsync(() => { }, new UnitOfWorkInfo(false, true));

            Wait();
        }


        [Test]
        public void EndEventEventIfAnExceptionWasThrown()
        {
            mocks.ReplayAll();
            workContext.End += delegate { AsyncDone(); };

            workContext.RunUnitOfWorkAsync(() => { throw new TestException(); }, new UnitOfWorkInfo(false, true));

            Wait();
        }

        [Test]
        public void ItsActuallyAsync()
        {
            mocks.ReplayAll();
            var thread = Thread.CurrentThread;

            workContext.RunUnitOfWorkAsync(() =>
                                          {
                                              Assert.AreNotSame(thread, Thread.CurrentThread);
                                              AsyncDone();
                                          }, new UnitOfWorkInfo(false, true));
            Wait();
        }

        [Test]
        public void OnSuccessCall()
        {
            mocks.ReplayAll();

            workContext.RunUnitOfWorkAsync(() => TestSuscriptions(throwException: false), new UnitOfWorkInfo(false));

            Wait();
            Assert.IsFalse(failureCall);
            Assert.IsTrue(successCall);
        }

        [Test]
        public void ShouldCallAsynchronously()
        {
            mocks.ReplayAll();

            workContext.RunUnitOfWorkAsync(AsyncDone, new UnitOfWorkInfo(false, true));

            Wait();
        }

        [Test]
        public void ShouldNotFireUnhandledExceptionIfNoExceptionWasThrown()
        {
            mocks.ReplayAll();
            workContext.UnhandledException += delegate { Assert.Fail("UnhandledException not expected"); };
            workContext.RunUnitOfWorkAsync(() => { }, new UnitOfWorkInfo(false, true));

            Thread.Sleep(AsyncTimeout); //to see if it fails asynchronously
        }

        [Test]
        public void UnhandledException()
        {
            mocks.ReplayAll();
            workContext.UnhandledException += (sender, e) =>
                                                  {
                                                      Assert.IsInstanceOf<TestException>(e.ExceptionObject);
                                                      AsyncDone();
                                                  };

            workContext.RunUnitOfWorkAsync(() => { throw new TestException(); }, new UnitOfWorkInfo(false, true));
            Wait();
        }
    }
}