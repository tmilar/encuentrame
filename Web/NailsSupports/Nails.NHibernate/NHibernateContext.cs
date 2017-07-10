using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using NailsFramework.Config;
using NailsFramework.IoC;
using NailsFramework.Logging;
using NailsFramework.Support;
using NHibernate;
using NHibernate.Context;
using Configuration = NHibernate.Cfg.Configuration;

namespace NailsFramework.Persistence
{
    /// <summary>
    ///   Persistence context for NHibernate.
    /// </summary>
    public class NHibernateContext : IPersistenceContext, INHibernateContext
    {
        private readonly List<MissingConfiguration> missingConfigurations = new List<MissingConfiguration>();
        private ISessionFactory sessionFactory;

        [Inject]
        public ILog Log { private get; set; }

        [Inject("NHibernate.ConfigurationFile")]
        public string ConfigurationFile { get; set; }

        public Configuration Configuration { get; private set; }

        public IEnumerable<MissingConfiguration> MissingConfigurations
        {
            get { return missingConfigurations; }
        }

        #region INHibernateContext Members

        public ISession CurrentSession
        {
            get
            {
                try
                {
                    return CurrentSessionContext.HasBind(sessionFactory) ? sessionFactory.GetCurrentSession() : null;
                }
                catch(Exception e)
                {
                    throw new NailsException("There was a problem getting the current unit of work from the CurrentSessionContext. Please, check the value of the current_session_context_class configuration in NHibernate's configuration", e);
                }
            }
        }

        #endregion

        #region IPersistenceContext Members

        /// <summary>
        ///   Open a persistence session.
        /// </summary>
        public virtual void OpenSession()
        {
            if (CurrentSession != null)
                throw new InvalidOperationException("NHibernate session already open");

            var session = sessionFactory.OpenSession();
            session.FlushMode = FlushMode.Never;
            CurrentSessionContext.Bind(session);
        }

        /// <summary>
        ///   Close the persistence session.
        /// </summary>
        public virtual void CloseSession()
        {
            try
            {
                if (!IsSessionOpened)
                {
                    Log.Info("Closing NHibernate Session - Session is not open");
                    return;
                }

                Log.Info("Closing NHibernate Session");
                CurrentSession.Close();
                CurrentSession.Dispose();
            }
            finally
            {
                CurrentSessionContext.Unbind(sessionFactory);
            }
        }

        /// <summary>
        ///   Create a TransactionalContext and return it.
        /// </summary>
        /// <returns></returns>
        public virtual ITransactionalContext CreateTransactionalContext()
        {
            return new NHibernateTransactionalContext(this);
        }

        public PropertyInfo GetIdPropertyOf<T>() where T : class
        {
            var propertyName = sessionFactory.GetClassMetadata(typeof (T)).IdentifierPropertyName;
            return typeof (T).GetProperty(propertyName);
        }

        public bool IsSessionOpened
        {
            get { return CurrentSession != null; }
        }

        #endregion

        /// <summary>
        ///   Initializes this instance.
        /// </summary>
        /// <param name="configure"></param>
        public void Initialize(Action<Configuration> configure)
        {
            Configuration = new Configuration();
            if (string.IsNullOrWhiteSpace(ConfigurationFile))
            {
                missingConfigurations.Add(
                    new MissingConfiguration(
                        "Property ConfigurationFile of NHibernateContext is required. Maybe you forgot to add the key NHibernate.ConfigurationFile in the <appSettings> section of your configuration file."));
                return;
            }

            var path = LocalPath.From(ConfigurationFile);

            Log.Info(string.Format("Loading NHibernate Configuration from {0}", path));
            Configuration.Configure(path);
            Log.Info(string.Format("End Loading NHibernate Configuration from {0}", path));

            if (!ValidateConnectionStringExistence(Configuration))
                return;
            
            configure(Configuration);

            Log.Info("Building new Hibernate Session Factory");
            sessionFactory = Configuration.BuildSessionFactory();
            Log.Info("En Building new Hibernate Session Factory");
        }

        private bool ValidateConnectionStringExistence(Configuration configuration)
        {
            var connStringName = configuration.GetProperty("connection.connection_string_name");

            if (string.IsNullOrWhiteSpace(connStringName))
            {
                var connStringValue = configuration.GetProperty("connection.connection_string");

                if (string.IsNullOrEmpty(connStringValue))
                {
                    missingConfigurations.Add(
                        new MissingConfiguration(
                            string.Format(
                                "Connection string is not configured. Ensure one of the properties connection.connection_string and connection.connection_string_name of your {0} file is filled.",
                                ConfigurationFile)));
                    return false;
                }
            }
            else
            {
                var connString = ConfigurationManager.ConnectionStrings[connStringName];
                if (connString == null)
                {
                    missingConfigurations.Add(
                        new MissingConfiguration(
                            string.Format(
                                "Connection string {0} is missing in your configuration file. Add it to the <connectionStrings> section of your configuration file.",
                                connStringName)));
                    return false;
                }

                if (string.IsNullOrWhiteSpace(connString.ConnectionString))
                {
                    missingConfigurations.Add(
                        new MissingConfiguration(
                            string.Format(
                                "You need to configure the connection string. In your configuration file, inside the <connectionStrings> section you will find an empty entry called {0}. Put there your connection string.",
                                connStringName)));
                    return false;
                }
            }
            return true;
        }
    }
}