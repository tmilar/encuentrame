namespace NailsFramework.Aspects
{
    public interface ILemmingBehavior
    {
        object ApplyTo(MethodInvocationInfo invocation);
    }
}