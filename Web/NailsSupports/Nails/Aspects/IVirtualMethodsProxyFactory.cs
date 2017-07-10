namespace NailsFramework.Aspects
{
    public interface IVirtualMethodsProxyFactory
    {
        T Create<T>(params Aspect[] with) where T:new();
    }
}