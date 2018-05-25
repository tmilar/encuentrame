using NailsFramework.Aspects;

namespace NailsFramework.Config
{
    public interface IAspectsConfigurator : INailsConfigurator
    {
        BehaviorConfigurator ApplyBehavior<TBehavior>() where TBehavior : ILemmingBehavior, new();
        BehaviorConfigurator ApplyBehavior(ILemmingBehavior behavior);
    }
}