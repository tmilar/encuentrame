using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using NailsFramework.Aspects;
using NailsFramework.IoC;
using NailsFramework.Logging;
using NailsFramework.Persistence;
using NailsFramework.Plugins;
using NailsFramework.Support;
using NailsFramework.UnitOfWork;
using NailsFramework.UserInterface;

namespace NailsFramework.Config
{
    public class NailsConfiguration : INailsConfiguration, IAspectsConfigurator, IIoCConfigurator,
                                      IUnitOfWorkConfigurator, ILogConfigurator, IPersistenceConfigurator,
                                      IUserInterfaceConfigurator, IPluginsConfigurator, IConfigurationStatus
    {
        private readonly List<Aspect> aspects;
        private readonly List<Assembly> assembliesToInspect = new List<Assembly>();
        private readonly List<Lemming> lemmings = new List<Lemming>();
        private readonly List<MissingConfiguration> missingConfigurations = new List<MissingConfiguration>();
        private readonly List<NailsPlugin> plugins = new List<NailsPlugin>();
        private readonly List<Injection> staticInjections = new List<Injection>();
        private DataMapper dataMapper;
        private IoCContainer iocContainer;
        private Logger logger;
        private UIPlatform uiPlatform = new NullUIPlatform();

        public NailsConfiguration()
        {
            aspects = new List<Aspect>();
            ConnectionBoundUnitOfWork = true;
            logger = new NullLogger();
            dataMapper = new NullDataMapper();
            iocContainer = new NullIoCContainer();
            PageSize = 25;
        }

        public bool Initialized { get; private set; }
        public IObjectFactory ObjectFactory { get; private set; }

        public IUserInterfaceConfigurator UI
        {
            get { return this; }
        }

        #region IAspectsConfigurator Members

        public BehaviorConfigurator ApplyBehavior<TBehavior>() where TBehavior : ILemmingBehavior, new()
        {
            return ApplyBehavior(new TBehavior());
        }

        public BehaviorConfigurator ApplyBehavior(ILemmingBehavior behavior)
        {
            return new BehaviorConfigurator(behavior, this);
        }

        public IUnitOfWorkConfigurator UnitOfWork
        {
            get { return this; }
        }

        IAspectsConfigurator INailsConfigurator.Aspects
        {
            get { return this; }
        }

        public INailsConfigurator InspectAssemblyOf<T>()
        {
            return InspectAssemblyOf(typeof (T));
        }

        public INailsConfigurator InspectAssemblyOf(Type type)
        {
            return InspectAssembly(type.Assembly);
        }

        public INailsConfigurator InspectAssembly(Assembly assembly)
        {
            assembliesToInspect.Add(assembly);

            lemmings.AddRange(assembly.TypesWithAttribute<LemmingAttribute>(true).Select(x=>GetOrCreateLemming(x,null)));

            foreach (var type in assembly.GetTypes())
                InjectStaticPropertiesOf(type);

            return this;
        }

        public INailsConfigurator InspectAssembly(string assemblyFile)
        {
            return InspectAssembly(Assembly.LoadFrom(LocalPath.From(assemblyFile)));
        }

        public IPersistenceConfigurator Persistence
        {
            get { return this; }
        }

        public IUserInterfaceConfigurator UserInterface
        {
            get { return this; }
        }

        public IPluginsConfigurator Plugins
        {
            get { return this; }
        }

        ILogConfigurator INailsConfigurator.Logging
        {
            get { return this; }
        }

        IIoCConfigurator INailsConfigurator.IoC
        {
            get { return this; }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Initialize(bool configureDefaults)
        {
            if (Initialized) return;

            var components = GetComponents();

            foreach (var component in components)
                component.AddCustomConfiguration(this);

            ConfigureDataMapper();
            ConfigureUI();

            if (configureDefaults)
                new DefaultRulesConfigurator().Configure();

            ConfigureLogger();
            ConfigureIoC();

            try
            {
                foreach (var component in components)
                {
                    component.Initialize();
                    missingConfigurations.AddRange(component.MissingConfigurations);
                }
            }
            catch
            {
                missingConfigurations.Add(new MissingConfiguration("Something is wrong in Nails configuration. Please, review your previous steps and restart the application."));
                throw;
            }

            Initialized = true;
        }

        #endregion

        #region IConfigurationStatus Members

        public IEnumerable<MissingConfiguration> MissingConfigurations
        {
            get { return missingConfigurations; }
        }

        public bool IsReady
        {
            get { return Initialized && missingConfigurations.Count == 0; }
        }

        #endregion

        #region IIoCConfigurator Members

        IIoCConfigurator IIoCConfigurator.Container(IoCContainer iocContainer)
        {
            this.iocContainer = iocContainer;
            return this;
        }

        IIoCConfigurator IIoCConfigurator.Container<TIoc>(Action<TIoc> configure)
        {
            var ioc = new TIoc();
            iocContainer = ioc;
            if(configure!=null)
                configure(ioc);
            return this;
        }

        public IIoCConfigurator InjectStaticPropertiesOf(Type type)
        {
            foreach (var property in type.Setters().Where(x => x.IsStatic()))
                InjectStaticReference(property);

            return this;
        }

        public IIoCConfigurator Lemming<T>(string name, Action<LemmingConfigurator<T>> config) where T : class
        {
            var lemming = GetOrCreateLemming(typeof (T), name);

            var builder = new LemmingConfigurator<T>(lemming, lemmings);
            config(builder);
            lemmings.Add(lemming);
            return this;
        }

        public IIoCConfigurator Lemming(Type type, Action<LemmingConfigurator> config)
        {
            return Lemming(type, null, config);
        }

        public IIoCConfigurator Lemming<T>(Action<LemmingConfigurator<T>> config) where T : class
        {
            return Lemming(null, config);
        }

        public IIoCConfigurator Lemming(Type type, string name)
        {
            var lemming = GetOrCreateLemming(type, name);
            if (!string.IsNullOrEmpty(name))
                lemming.Name = name;

            lemmings.Add(lemming);
            InjectStaticPropertiesOf(type);
            return this;
        }

        public IIoCConfigurator Lemming<T>(string name) where T : class
        {
            return Lemming(typeof (T), name);
        }

        public IIoCConfigurator InjectStaticPropertiesOf<T>() where T : class
        {
            return InjectStaticPropertiesOf(typeof (T));
        }

        public IIoCConfigurator StaticReference(Expression<Func<object>> property, string referencedLemming)
        {
            return InjectStaticReference(property.ToPropertyInfo(), referencedLemming);
        }

        public IIoCConfigurator StaticReference<TReference>(Expression<Func<object>> property)
            where TReference : class
        {
            return InjectStaticReference<TReference>(property.ToPropertyInfo());
        }

        public IIoCConfigurator StaticReference<TReference>(Type staticType, string property) where TReference : class
        {
            return InjectStaticReference<TReference>(GetStaticProperty(staticType, property));
        }

        public IIoCConfigurator StaticReference(Type staticType, string property, string referencedLemming)
        {
            var propertyInfo = GetStaticProperty(staticType, property);
            return InjectStaticReference(propertyInfo, referencedLemming);
        }

        public IIoCConfigurator StaticValueFromConfiguration<T>(Expression<Func<T>> property)
        {
            return ValueFromConfiguration(property.ToPropertyInfo());
        }

        public IIoCConfigurator StaticValueFromConfiguration(Type staticType, string property)
        {
            return ValueFromConfiguration(GetStaticProperty(staticType, property));
        }

        public IIoCConfigurator StaticValue<T>(Type staticType, string property, T value)
        {
            return Value(GetStaticProperty(staticType, property), value);
        }

        public IIoCConfigurator StaticValue<T>(Expression<Func<T>> staticProperty, T value)
        {
            return Value(staticProperty.ToPropertyInfo(), value);
        }

        #endregion

        #region ILogConfigurator Members
        ILogConfigurator ILogConfigurator.Logger<TLogger>()
        {
            var log = new TLogger();
            logger = log;
            return this;
        }
        ILogConfigurator ILogConfigurator.Logger<TLogger>(Action<TLogger> configure)
        {
            var log =new TLogger();
            logger = log;
            if (configure != null) 
                configure(log);
            return this;
        }

        ILogConfigurator ILogConfigurator.Logger(Logger logger)
        {
            this.logger = logger;
            return this;
        }

        #endregion

        #region INailsConfiguration Members

        public bool ConnectionBoundUnitOfWork { get; private set; }
        public bool DefaultAsyncMode { get; private set; }
        public bool AllowAsyncExecution { get; private set; }

        public int PageSize { get; private set; }

        public IEnumerable<Lemming> LemmingsSchema
        {
            get { return lemmings; }
        }

        public IEnumerable<Assembly> AssembliesToInspect
        {
            get { return assembliesToInspect; }
        }

        public IEnumerable<Injection> StaticInjections
        {
            get { return staticInjections; }
        }

        public IEnumerable<Aspect> Aspects
        {
            get { return aspects; }
        }

        IEnumerable<NailsPlugin> INailsConfiguration.Plugins
        {
            get { return plugins; }
        }

        public TransactionMode DefaultTransactionMode { get; private set; }

        #endregion

        #region IPersistenceConfigurator Members

        IPersistenceConfigurator IPersistenceConfigurator.DataMapper(DataMapper dataMapper)
        {
            this.dataMapper = dataMapper;
            return this;
        }

        IPersistenceConfigurator IPersistenceConfigurator.DataMapper<TDataMapper>(Action<TDataMapper> configure)
        {
            var data = new TDataMapper();
            dataMapper = data;
            if (configure != null) 
                configure(data);
            return this;
        }

        IPersistenceConfigurator IPersistenceConfigurator.PageSize(int pageSize)
        {
            PageSize = pageSize;
            return this;
        }

        #endregion

        #region IPluginsConfigurator Members

        INailsConfigurator IPluginsConfigurator.Add<TPlugin>(Action<TPlugin> configure)
        {
            var plugin = new TPlugin();
            plugins.Add(plugin);
            if (configure != null) 
                configure(plugin);
            return this;
        }

        INailsConfigurator IPluginsConfigurator.Add(NailsPlugin plugin)
        {
            plugins.Add(plugin);
            return this;
        }

        #endregion

        #region IUnitOfWorkConfigurator Members

        IUnitOfWorkConfigurator IUnitOfWorkConfigurator.ConnectionBoundUnitOfWork(bool connectionBoundUnitOfWork)
        {
            ConnectionBoundUnitOfWork = connectionBoundUnitOfWork;
            return this;
        }

        IUnitOfWorkConfigurator IUnitOfWorkConfigurator.DefaultAsyncMode(bool defaultAsyncMode)
        {
            DefaultAsyncMode = defaultAsyncMode;
            return this;
        }

        IUnitOfWorkConfigurator IUnitOfWorkConfigurator.DefaultTransactionMode(TransactionMode defaultTransactionMode)
        {
            DefaultTransactionMode = defaultTransactionMode;
            return this;
        }

        IUnitOfWorkConfigurator IUnitOfWorkConfigurator.AllowAsyncExecution(bool allowAsyncExecution)
        {
            AllowAsyncExecution = allowAsyncExecution;
            return this;
        }

        #endregion

        #region IUserInterfaceConfigurator Members

        INailsConfigurator IUserInterfaceConfigurator.Platform(UIPlatform uiPlatform)
        {
            this.uiPlatform = uiPlatform;
            return this;
        }

        INailsConfigurator IUserInterfaceConfigurator.Platform<TUIPlatform>(Action<TUIPlatform> configure)
        {
            var ui =  new TUIPlatform();
            uiPlatform = ui;
            if (configure != null) 
                configure(ui);
            return this;
        }

        #endregion

        public IIoCConfigurator Lemming(Type type, string name, Action<LemmingConfigurator> config)
        {
            var lemming = GetOrCreateLemming(type, name);

            var builder = new LemmingConfigurator(type, lemming, lemmings);
            config(builder);
            lemmings.Add(lemming);
            return this;
        }

        private IEnumerable<NailsComponent> GetComponents()
        {
            return plugins.Union(new NailsComponent[]
                                     {
                                         logger, iocContainer, dataMapper, uiPlatform
                                     });
        }

        public IIoCConfigurator Lemming(Type type)
        {
            return Lemming(type, default(string));
        }

        private void InjectStaticReference(PropertyInfo property)
        {
            var attribute = property.Attribute<InjectAttribute>();

            if (attribute == null)
                return;

            var injection = attribute.InjectionFrom(property);
            staticInjections.Add(injection);
        }

        private ReferenceInjection GetOrCreateReference(PropertyInfo property)
        {
            var injection = staticInjections
                .OfType<ReferenceInjection>()
                .SingleOrDefault(x => x.Property == property);

            if (injection == null)
            {
                injection = new ReferenceInjection(property);
                staticInjections.Add(injection);
            }

            return injection;
        }

        private IIoCConfigurator InjectStaticReference(PropertyInfo property, string referencedLemming)
        {
            var injection = GetOrCreateReference(property);
            if (!string.IsNullOrWhiteSpace(referencedLemming))
                injection.ReferencedLemming = referencedLemming;

            return this;
        }

        private IIoCConfigurator InjectStaticReference<TReference>(PropertyInfo staticProperty)
            where TReference : class
        {
            var referenceLemming = lemmings.SingleOrDefault(x => x.ConcreteType == typeof (TReference));

            if (referenceLemming == null)
            {
                referenceLemming = IoC.Lemming.From(typeof (TReference));
                lemmings.Add(referenceLemming);
            }
            return InjectStaticReference(staticProperty, referenceLemming.Name);
        }

        private static PropertyInfo GetStaticProperty(Type staticType, string property)
        {
            return staticType.GetProperty(property, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
        }

        private void RemoveStaticInjection(PropertyInfo property)
        {
            var injection = staticInjections.SingleOrDefault(x => x.Property == property);
            if (injection != null)
                staticInjections.Remove(injection);
        }

        private IIoCConfigurator Value<TValue>(PropertyInfo property, TValue value)
        {
            RemoveStaticInjection(property);
            staticInjections.Add(new ValueInjection(property, value));
            return this;
        }

        private IIoCConfigurator ValueFromConfiguration(PropertyInfo property, string appSetting = null)
        {
            RemoveStaticInjection(property);
            staticInjections.Add(new ConfigurationInjection(property, appSetting));
            return this;
        }

        private Lemming GetOrCreateLemming(Type type, string name)
        {
            var candidate = IoC.Lemming.From(type);
            if (!string.IsNullOrWhiteSpace(name))
                candidate.Name = name;

            var lemming = lemmings.SingleOrDefault(x => x.Name == candidate.Name);

            if (lemming == null)
            {
                lemming = candidate;
            }
            else
            {
                if (lemming.ConcreteType != type)
                    throw new NailsException(string.Format("Lemming {0} was already added for type {1}", name,
                                                           type.FullName));
            }
            return lemming;
        }

        public IAspectsConfigurator AddAspect(Aspect aspect)
        {
            aspects.Add(aspect);
            return this;
        }

        private void ConfigureUI()
        {
        }

        private void ConfigureDataMapper()
        {
            Lemming(dataMapper.BagType, dataMapper.ConfigureBags);
            Lemming(dataMapper.PersistenceContextType, dataMapper.ConfigurePersistenceContext);
        }

        private void ConfigureLogger()
        {
            var requiredLogs =
                lemmings.SelectMany(x => x.Injections)
                    .Union(staticInjections)
                    .OfType<ReferenceInjection>()
                    .Where(x => x.Property.PropertyType == typeof (ILog) &&
                                string.IsNullOrWhiteSpace(x.ReferencedLemming)).ToList();

            foreach (var requiredLog in requiredLogs)
            {
                var logName = requiredLog.ParentLemming == null
                                  ? requiredLog.Property.ReflectedType.FullFriendlyName()
                                  : requiredLog.ParentLemming.UniqueName;

                var logLemmingName = string.Format("Log{{{0}}}", logName);
                requiredLog.ReferencedLemming = logLemmingName;

                Lemming(logger.LogType, logLemmingName, x =>
                                                            {
                                                                x.Value("LogName", logName);
                                                                logger.ConfigureLogLemming(x);
                                                            });
            }
        }

        private void ConfigureIoC()
        {
            //Needs the aspects configured before the application context
            iocContainer.ConfigureAspects(aspects);

            //Then, we configure the object factory
            var configurableObjectFactory = iocContainer.GetObjectFactory();
            ObjectFactory = configurableObjectFactory;
            configurableObjectFactory.Configure(lemmings, staticInjections);
        }
    }
}