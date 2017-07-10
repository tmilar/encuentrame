namespace NailsFramework.IoC
{
    public interface IInjector
    {
        void Inject(ValueInjection injection);
        void Inject(ReferenceInjection injection);
    }
}