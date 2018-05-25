using System;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NailsFramework.TestSupport;
using Encuentrame.Support;


namespace Encuentrame.Tests.Supports
{
    public abstract class BaseTest : NailsTestsConfigurable
    {
        [Inject]
        public INHibernateContext NHibernateContext { get; set; }
        public override string ConfigurationName { get { return "BaseTestConfiguration"; } }

        public static void CreateDatabase()
        {            
            DatabaseCreator.Create();
        }
        protected void RefreshObject(object obj)
        {
            try
            {
                NHibernateContext.CurrentSession.Refresh(obj);
            }
            catch (NHibernate.UnresolvableObjectException ex)
            {
                
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        public static Delorean Delorean { get; private set; }
        public override void SetUp()
        {
            Delorean = Delorean.StoppedIn(DateTime.Now);
            SystemDateTime.Use(Delorean);
            base.SetUp();
        }

        public void ReturnToThePresent()
        {
            Delorean = Delorean.StoppedIn(DateTime.Now);
            SystemDateTime.Use(Delorean);
        }
        public override void TearDown()
        {
           

            base.TearDown();

        }
    }
}

