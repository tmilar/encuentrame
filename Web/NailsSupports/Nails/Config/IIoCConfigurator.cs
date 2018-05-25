using System;
using System.Linq.Expressions;
using NailsFramework.IoC;

namespace NailsFramework.Config
{
    public interface IIoCConfigurator : INailsConfigurator
    {
        IIoCConfigurator Container(IoCContainer container);
        IIoCConfigurator Container<TIoc>(Action<TIoc> ioc = null) where TIoc : IoCContainer, new();

        IIoCConfigurator Lemming<T>(string name, Action<LemmingConfigurator<T>> config) where T : class;
        IIoCConfigurator Lemming(Type type, Action<LemmingConfigurator> config);
        IIoCConfigurator Lemming<T>(Action<LemmingConfigurator<T>> config) where T : class;
        IIoCConfigurator Lemming(Type type, string name = null);
        IIoCConfigurator Lemming<T>(string name = null) where T : class;

        IIoCConfigurator StaticReference(Expression<Func<object>> property, string referencedLemming = null);
        IIoCConfigurator StaticReference<TReference>(Expression<Func<object>> property) where TReference : class;
        IIoCConfigurator StaticReference<TReference>(Type staticType, string property) where TReference : class;
        IIoCConfigurator StaticReference(Type staticType, string property, string referencedLemming = null);

        IIoCConfigurator StaticValueFromConfiguration<T>(Expression<Func<T>> property);
        IIoCConfigurator StaticValueFromConfiguration(Type staticType, string property);

        IIoCConfigurator StaticValue<T>(Type staticType, string property, T value);
        IIoCConfigurator StaticValue<T>(Expression<Func<T>> staticProperty, T value);

        IIoCConfigurator InjectStaticPropertiesOf<T>() where T : class;
        IIoCConfigurator InjectStaticPropertiesOf(Type type);
    }
}