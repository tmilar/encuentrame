using System;
using System.Linq;

namespace NailsFramework.Support
{
    public static class GenericInheritanceExtensions
    {
        public static bool GenericDefinitionIsAssignableFrom(this Type genericTypeDefinition, Type childType)
        {
            if (!genericTypeDefinition.IsGenericTypeDefinition)
                throw new InvalidOperationException("Should be applied to a generic type definition.");

            if (childType.IsGenericTypeDefinition)
                throw new InvalidOperationException("The parent type cannot be be a generic type definition.");

            return childType.AllParentTypes(includeItself: true).Any(x => AreTheSame(genericTypeDefinition, x));
        }

        public static bool GenericDefinitionInheritsFrom(this Type genericTypeDefinition, Type parentType)
        {
            if (!genericTypeDefinition.IsGenericTypeDefinition)
                throw new InvalidOperationException("Should be applied to a generic type definition.");

            if (parentType.IsGenericTypeDefinition)
                throw new InvalidOperationException("The parent type cannot be be a generic type definition.");

            var parentGenericTypeDefinition = parentType.IsGenericType
                                                  ? parentType.GetGenericTypeDefinition()
                                                  : parentType;

            if (parentType == typeof (object))
                return true;

            var typeArguments = genericTypeDefinition.GetGenericTypeDefinition().GetGenericArguments();
            var parentArguments = parentGenericTypeDefinition.GetGenericArguments();

            if (typeArguments.Length > parentArguments.Length)
                return false;

            if (genericTypeDefinition.IsClass)
            {
                //Search the superclass that matches
                var current = genericTypeDefinition;
                while (current != typeof (object))
                {
                    if (AreTheSame(current, parentType))
                        return true;

                    current = current.BaseType;
                }
            }
            //search the matching interface
            return genericTypeDefinition.GetInterfaces().Any(x => AreTheSame(x, parentType));
        }

        private static bool AreTheSame(Type genericTypeDefinition, Type concreteType)
        {
            if (!concreteType.IsGenericType)
                return genericTypeDefinition == concreteType;

            if (concreteType.GetGenericTypeDefinition().GUID == genericTypeDefinition.GUID)
            {
                for (var i = 0; i < genericTypeDefinition.GetGenericArguments().Length; i++)
                {
                    var parentTypeGenericArgument = concreteType.GetGenericArguments()[i];

                    var genericArgument = genericTypeDefinition.GetGenericArguments()[i];
                    if (genericArgument.IsGenericParameter)
                    {
                        if (genericArgument.GetGenericParameterConstraints()
                            .Any(x => !x.IsAssignableFrom(parentTypeGenericArgument)))
                            return false;
                    }
                    else if (!genericArgument.IsAssignableFrom(parentTypeGenericArgument))
                        return false;
                }
                return true;
            }
            return false;
        }
    }
}