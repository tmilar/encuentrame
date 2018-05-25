using NailsFramework.Tests.Support;
using NHibernate.Cfg;
using NUnit.Framework;

namespace NailsFramework.Tests.Persistence.NHibernate
{
    public class NHibernateTests : BaseTest
    {
        [Test]
        public void WrongConnectionStringShouldFailNicely()
        {
            var nhib = new NailsFramework.Persistence.NHibernate
                           {
                               ConfigurationFile = "badhibernate_wrongconnection.cfg.xml"
                           };

            Nails.Configure().Persistence.DataMapper(nhib)
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Initialize();

            CollectionAssert.IsNotEmpty(Nails.Status.MissingConfigurations);
            Assert.IsFalse(Nails.Status.IsReady);
        }
    
        [Test, ExpectedException(typeof(HibernateConfigException))]
        public void OldConfigurationWithUseOuterJoinShouldThrowHibernateException()
        {
            var nhib = new NailsFramework.Persistence.NHibernate
            {
                ConfigurationFile = "badhibernate_useouterjoin.cfg.xml"
            };

            Nails.Configure().Persistence.DataMapper(nhib)
                .IoC.Container<NailsFramework.IoC.Spring>()
                .Initialize();
        }
    }
}