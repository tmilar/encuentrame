using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using NailsFramework.Aspects;
using NailsFramework.Support;

namespace NailsFramework.Config
{
    public class BehaviorConfigurator
    {
        private readonly ILemmingBehavior behavior;
        private readonly NailsConfiguration configuration;
        private readonly List<string> excludedMethods = new List<string>();

        public BehaviorConfigurator(ILemmingBehavior behavior, NailsConfiguration configuration)
        {
            this.behavior = behavior;
            this.configuration = configuration;
        }

        public IAspectsConfigurator ToMethodsSatisfying(Func<MethodBase, bool> condition)
        {
            return configuration.AddAspect(new Aspect(behavior, new BehaviorCondition(condition, excludedMethods)));
        }

        public IAspectsConfigurator ToMethodsMatching(string regexPattern)
        {
            var pattern = new Regex(regexPattern, RegexOptions.None);
            return ToMethodsSatisfying(x => pattern.Match(x.FullFriendlyName()).Success);
        }

        public IAspectsConfigurator ToTypesWithAttribute(Type attributeType)
        {
            return ToMethodsSatisfying(x => x.DeclaringType.IsDefined(attributeType, true));
        }

        public IAspectsConfigurator ToTypesWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            return ToTypesWithAttribute(typeof(TAttribute));
        }

        public IAspectsConfigurator ToMethodsWithAttribute<TAttribute>() where TAttribute : Attribute
        {
            return ToMethodsWithAttribute(typeof(TAttribute));
        }

        public IAspectsConfigurator ToMethodsWithAttribute(Type attributeType)
        {
            return ToMethodsSatisfying(x => x.IsDefined(attributeType, true));
        }

        public IAspectsConfigurator ToInheritorsOf(Type type)
        {
            return type.IsGenericTypeDefinition
                       ? ToMethodsSatisfying(x => type.GenericDefinitionIsAssignableFrom(x.ReflectedType))
                       : ToMethodsSatisfying(x => type.IsAssignableFrom(x.ReflectedType));
        }

        public IAspectsConfigurator ToInheritorsOf<T>() where T : class
        {
            return ToInheritorsOf(typeof(T));
        }

        public IAspectsConfigurator ToType(Type type)
        {
            return ToMethodsSatisfying(x => x.ReflectedType == type);
        }

        public IAspectsConfigurator ToType<T>() where T : class
        {
            return ToType(typeof(T));
        }

        public BehaviorConfigurator ExcludingMethods(params string[] methods)
        {
            excludedMethods.AddRange(methods);
            return this;
        }

        private static IEnumerable<string> MethodsOfProperty(string property)
        {
            yield return "get_" + property;
            yield return "set_" + property;
        }

        public BehaviorConfigurator ExcludingProperties(params string[] methods)
        {
            excludedMethods.AddRange(methods.SelectMany(MethodsOfProperty));
            return this;
        }
    }
}