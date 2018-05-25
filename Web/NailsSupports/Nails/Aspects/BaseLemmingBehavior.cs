using System;
using System.Collections.Generic;
using System.Reflection;

namespace NailsFramework.Aspects
{
    public class BehaviorCondition
    {
        private readonly Func<MethodBase, bool> condition;
        private readonly IList<string> excludedMethods;

        public BehaviorCondition(Func<MethodBase, bool> condition, IList<string> excludedMethods)
        {
            this.excludedMethods = excludedMethods;
            this.condition = condition;
        }

        /// <summary>
        ///   Returns if this instance applies to the specified method.
        /// </summary>
        /// <param name = "method">The method.</param>
        /// <returns></returns>
        public virtual bool AppliesTo(MethodBase method)
        {
            return !excludedMethods.Contains(GetNameOnly(method.Name)) && condition(method);
        }

        private static string GetNameOnly(string methodName)
        {
            return methodName.Substring(methodName.LastIndexOf(".") + 1);
        }
    }
}