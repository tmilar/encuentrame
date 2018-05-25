using System.Reflection;

namespace NailsFramework.Aspects
{
    public class Aspect
    {
        private readonly BehaviorCondition condition;

        public Aspect(ILemmingBehavior behavior, BehaviorCondition condition)
        {
            Behavior = behavior;
            this.condition = condition;
        }

        public ILemmingBehavior Behavior { get; private set; }

        public bool AppliesTo(MethodBase method)
        {
            return condition.AppliesTo(method);
        }
    }
}