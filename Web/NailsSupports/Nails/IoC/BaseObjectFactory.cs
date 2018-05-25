using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NailsFramework.Support;

namespace NailsFramework.IoC
{
    public abstract class BaseObjectFactory : IConfigurableObjectFactory
    {
        private readonly List<Lemming> addedGenericLemmings = new List<Lemming>();
        private readonly List<Lemming> addedLemmings = new List<Lemming>();
        private readonly List<Lemming> genericLemmings = new List<Lemming>();
        private readonly List<Lemming> preparedLemmings = new List<Lemming>();

        private readonly Dictionary<Type, Lemming> requiredEnumerableLemmings = new Dictionary<Type, Lemming>();
        private readonly List<ReferenceInjection> requiredGenericInjections = new List<ReferenceInjection>();
        private InstanceInjector instanceInjector;
        private IEnumerable<Lemming> lemmings;
        private IEnumerable<Injection> staticInjections;

        protected BaseObjectFactory()
        {
            RegisteredTypes = new List<Type>();
        }

        protected static List<Type> RegisteredTypes { get; private set; }

        #region IConfigurableObjectFactory Members

        public IEnumerable GetObjects(Type type)
        {
            return FindObjectsByType(type);
        }

        public virtual object GetObject(string objectName)
        {
            return FindObjectByName(objectName);
        }

        public T GetObject<T>() where T : class
        {
            return (T) GetObject(typeof (T));
        }

        public IEnumerable<T> GetObjects<T>() where T : class
        {
            return GetObjects(typeof (T)).Cast<T>();
        }

        public virtual void Configure(IEnumerable<Lemming> allLemmings, IEnumerable<Injection> staticInjections)
        {
            lemmings = allLemmings.Where(x => !x.Ignore);
            this.staticInjections = staticInjections;
            ProcessLemmings(lemmings);
            ProcessCollectionReferences(staticInjections);
            ProcessLemmings(requiredEnumerableLemmings.Values);

            foreach (var lemming in lemmings)
                Add(lemming);

            foreach (var lemming in requiredEnumerableLemmings.Values)
                Add(lemming);

            ProcessStaticInjections();

            PrepareGenericTypes(requiredGenericInjections.Distinct(), false);

            RenameGenericInjections();

            NameUnnamedInjections();

            foreach (var preparedLemming in preparedLemmings)
            {
                AddToContext(preparedLemming);
                addedLemmings.Add(preparedLemming);
            }

            new StaticInjector(this).Inject(staticInjections);
            instanceInjector = new InstanceInjector(this);
        }

        public T GetObject<T>(string objectName) where T : class
        {
            var obj = (T) FindObjectByName(objectName);
            if (obj != null || !typeof (T).IsGenericType)
                return obj;

            //manage the generic types... 
            return (T) GetGenericObject(objectName, typeof (T).GetGenericArguments());
        }

        public object GetGenericObject<T>(string name)
        {
            return GetGenericObject(name, typeof (T));
        }

        public object GetGenericObject<T1, T2>(string name)
        {
            return GetGenericObject(name, typeof (T1), typeof (T2));
        }

        public object GetGenericObject<T1, T2, T3>(string name)
        {
            return GetGenericObject(name, typeof (T1), typeof (T2), typeof (T3));
        }

        public object GetGenericObject<T1, T2, T3, T4>(string name)
        {
            return GetGenericObject(name, typeof (T1), typeof (T2), typeof (T3), typeof (T4));
        }

        public object GetGenericObject<T1, T2, T3, T4, T5>(string name)
        {
            return GetGenericObject(name, typeof (T1), typeof (T2), typeof (T3), typeof (T4), typeof (T5));
        }

        public object GetGenericObject(string name, Type typeParameter1, params Type[] othersTypeParameters)
        {
            return GetGenericObject(name, new[] {typeParameter1}.Union(othersTypeParameters));
        }

        public T Inject<T>(T instance) where T : class
        {
            instanceInjector.Inject(typeof(T), instance);
            return instance;
        }

        public object Inject(Type type, object instance)
        {
            instanceInjector.Inject(type, instance);
            return instance;
        }

        public virtual object GetObject(Type type)
        {
            var obj = FindObjectByType(type);

            if (obj != null)
                return obj;

            if (typeof (IEnumerable<>).GenericDefinitionIsAssignableFrom(type))
            {
                var enumeratedType = type.GetGenericArguments()[0];
                if (RegisteredTypes.Contains(enumeratedType))
                {
                    Add(LemmingsCollection(enumeratedType));
                    return FindObjectByType(type);
                }
            }

            if (!type.IsGenericType)
                return null;

            RegisterGenericType(type);
            return FindObjectByType(type);
        }

        #endregion

        private void RenameGenericInjections()
        {
            var toRename = requiredGenericInjections.Where(x => !string.IsNullOrWhiteSpace(x.ReferencedLemming));
            foreach (var injection in toRename)
            {
                var lemming =
                    preparedLemmings.Single(
                        x =>
                        injection.Property.PropertyType.IsAssignableFrom(x.ConcreteType) &&
                        x.Name == injection.ReferencedLemming);
                injection.ReferencedLemming = lemming.UniqueName;
            }
        }

        private void NameUnnamedInjections()
        {
            var unnamed = preparedLemmings.Union(lemmings).SelectMany(UnnamedReferences);
            NameUnnamedInjections(unnamed);
        }

        private static IEnumerable<ReferenceInjection> UnnamedReferences(Lemming lemming)
        {
            return
                lemming.Injections.OfType<ReferenceInjection>().Where(
                    x => string.IsNullOrWhiteSpace(x.ReferencedLemming) && !x.IsGeneric);
        }

        private void NameUnnamedInjections(IEnumerable<ReferenceInjection> unnamed)
        {
            foreach (var injection in unnamed)
            {
                var lemmingsOfType =
                    preparedLemmings.Where(x => injection.Property.PropertyType.IsAssignableFrom(x.ConcreteType)).ToList
                        ();

                if (lemmingsOfType.Count > 1)
                    throw new InvalidOperationException(
                        string.Format(
                            "There are too many candidates for injecting property {0}.{1} of Lemming {2}. Try to specify a name to the injection (and the Lemming) or to be little bit more explcit in the property type",
                            injection.Property.DeclaringType.Name, injection.Property.Name, injection.Owner));

                if (lemmingsOfType.Count == 0)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            "There are no Lemming for injecting property {0}.{1} of Lemming {2}. Try to specify a name to the injection (and the Lemming) or to be little bit more explcit in the property type",
                            injection.Property.DeclaringType.FullFriendlyName(), injection.Property.Name,
                            injection.Owner));
                }
                injection.ReferencedLemming = lemmingsOfType.Single().UniqueName;
            }
        }

        private object GetGenericObject(string name, IEnumerable<Type> typeParameters)
        {
            var uniqueName = LemmingUniqueName.From(name, typeParameters);
            var obj = GetObject(uniqueName);

            if (obj != null)
                return obj;

            RegisterGenericType(name, typeParameters);
            return GetObject(uniqueName);
        }

        private void ProcessCollectionReferences(IEnumerable<Injection> injections)
        {
            var enumerableTypes = injections.UnmanagedTypesReferencedAsEnumerable();
            foreach (var type in enumerableTypes)
            {
                if (requiredEnumerableLemmings.ContainsKey(type)) continue;

                requiredEnumerableLemmings.Add(type, LemmingsCollection(type));
            }
        }

        protected abstract object FindObjectByName(string name);

        private void ProcessLemmings(IEnumerable<Lemming> lemmings)
        {
            RegisteredTypes.AddRange(lemmings.Select(x => x.ConcreteType));

            var injections = lemmings.SelectMany(x => x.Injections);

            ProcessCollectionReferences(injections);
            ProcessGenericInjections(injections);
        }

        private static Lemming LemmingsCollection(Type type)
        {
            var lemmingType = typeof (LemmingsCollection<>).MakeGenericType(type);
            return Lemming.From(lemmingType);
        }

        protected abstract object FindObjectByType(Type type);
        protected abstract IEnumerable FindObjectsByType(Type type);

        protected abstract void AddToContext(Lemming lemming);

        private void Add(Lemming lemming)
        {
            if (!lemming.IsGenericDefinition)
                preparedLemmings.Add(lemming);
            else
                genericLemmings.Add(lemming);
        }

        private Lemming GetGenericLemmingFrom(Type type, bool validate)
        {
            var existing = addedGenericLemmings.SingleOrDefault(x => type.IsAssignableFrom(x.ConcreteType));

            if (existing != null)
                return existing;

            var lemmings = genericLemmings.Where(x => x.ConcreteType.GenericDefinitionInheritsFrom(type)).ToList();

            if (lemmings.Count > 1)
                throw new NailsException(string.Format("There are too many lemming defined for type {0}",
                                                       type.FullFriendlyName()));
            if (lemmings.Count == 0)
                throw new NailsException(string.Format("There's no lemming defined for type {0}",
                                                       type.FullFriendlyName()));
            var genericLemming = lemmings[0];

            var final = genericLemming.MakeGenericLemming(type.GetGenericArguments());

            if (validate)
            {
                ValidateGenericInjections(final);
                NameUnnamedInjections(UnnamedReferences(final));
            }
            addedGenericLemmings.Add(final);
            return final;
        }

        private void ValidateGenericInjections(Lemming lemming)
        {
            var refereneces = GetGenericInjections(lemming.Injections);
            PrepareGenericTypes(refereneces, true);
            foreach (var preparedLemming in preparedLemmings)
            {
                if (addedLemmings.Contains(preparedLemming))
                    continue;

                NameUnnamedInjections(UnnamedReferences(preparedLemming));
                RegisterGenericLemming(preparedLemming);
                addedLemmings.Add(preparedLemming);
                addedGenericLemmings.Add(preparedLemming);
            }
        }

        private void RegisterGenericType(Type genericBaseType)
        {
            if (RegisteredTypes.Contains(genericBaseType))
                return;

            var lemming = GetGenericLemmingFrom(genericBaseType, validate: true);

            RegisterGenericLemming(lemming);
        }

        private void RegisterGenericType(string lemmingName, IEnumerable<Type> typeArguments)
        {
            var lemming = genericLemmings.SingleOrDefault(x => x.Name == lemmingName);
            if (lemming == null)
                throw new InvalidOperationException(string.Format("There's no lemming named {0}.", lemmingName));

            RegisterGenericLemming(lemming.MakeGenericLemming(typeArguments));
        }

        private void RegisterGenericLemming(Lemming lemming)
        {
            AddToContext(lemming);
        }

        private IEnumerable<ReferenceInjection> GetGenericInjections(IEnumerable<Injection> injections)
        {
            return injections.OfType<ReferenceInjection>()
                .Where(x => x.Property.PropertyType.IsGenericType &&
                            !x.Property.PropertyType.ContainsGenericParameters &&
                            (string.IsNullOrWhiteSpace(x.ReferencedLemming) ||
                             !lemmings.Any(l => !l.IsGenericDefinition && l.Name == x.ReferencedLemming)) &&
                            (!string.IsNullOrWhiteSpace(x.ReferencedLemming) ||
                             !RegisteredTypes.Any(t => x.Property.PropertyType.IsAssignableFrom(t))) &&
                            (x.Property.PropertyType.GetGenericTypeDefinition() != typeof (IEnumerable<>) ||
                             !requiredEnumerableLemmings.ContainsKey(x.Property.PropertyType.GetGenericArguments()[0])));
        }

        private void ProcessGenericInjections(IEnumerable<Injection> injections)
        {
            var genericInjections = GetGenericInjections(injections);
            requiredGenericInjections.AddRange(genericInjections);
        }

        private void ProcessStaticInjections()
        {
            ProcessGenericInjections(staticInjections);
        }

        private Lemming GetGenericLemmingFrom(ReferenceInjection injection, bool validate)
        {
            if (string.IsNullOrWhiteSpace(injection.ReferencedLemming))
                return GetGenericLemmingFrom(injection.Property.PropertyType, validate);

            Lemming final;

            if (injection.IsGeneric)
            {
                var candidate =
                    genericLemmings.Single(
                        x =>
                        x.ConcreteType.GenericDefinitionInheritsFrom(injection.Property.PropertyType) &&
                        x.Name == injection.ReferencedLemming);
                final = candidate.MakeGenericLemming(injection.Property.PropertyType.GetGenericArguments());
            }
            else
            {
                var candidates = genericLemmings.Where(
                    x => x.ConcreteType.GenericDefinitionInheritsFrom(injection.Property.PropertyType))
                    .Select(x => x.MakeGenericLemming(injection.Property.PropertyType.GetGenericArguments()));
                final = candidates.SingleOrDefault(x => x.UniqueName == injection.ReferencedLemming) ??
                        candidates.SingleOrDefault(x => x.Name == injection.ReferencedLemming);
            }

            if (final == null)
                throw new NailsException(string.Format("There's no lemming defined for type {0} and name {1}",
                                                       injection.Property.PropertyType.FullFriendlyName(),
                                                       injection.ReferencedLemming));

            var existing = addedGenericLemmings.SingleOrDefault(x => x.UniqueName == final.UniqueName);

            if (existing != null)
                return existing; //So, if it already exists we return that one instead of the new one.

            addedGenericLemmings.Add(final);
            return final;
        }


        private void PrepareGenericTypes(IEnumerable<ReferenceInjection> genericInjections, bool validate)
        {
            foreach (var injection in genericInjections)
            {
                if (RegisteredTypes.Contains(injection.Property.PropertyType))
                    continue;

                var lemming = GetGenericLemmingFrom(injection, validate);

                if (!preparedLemmings.Contains(lemming))
                    preparedLemmings.Add(lemming);
            }
        }
    }
}