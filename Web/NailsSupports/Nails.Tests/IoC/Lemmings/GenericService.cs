using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Lemmings
{
    [Lemming]
    public class GenericService<T> : IGenericService<T>
    {
    }
}