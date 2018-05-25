using System;
using System.Collections.Generic;
using System.Text;

namespace NailsFramework.IoC
{
    public static class LemmingUniqueName
    {
        public static string From(string name, IEnumerable<Type> typeParameters)
        {
            var nameBuilder = new StringBuilder()
                .Append(name)
                .Append("<");
            foreach (var type in typeParameters)
                AppendTypeName(nameBuilder, type);

            nameBuilder.Append(">");

            return nameBuilder.ToString();
        }

        private static void AppendTypeName(StringBuilder builder, Type type)
        {
            builder.Append(type.Namespace)
                .Append(".")
                .Append(type.Name.Split('`')[0]);
            if (!type.IsGenericType) return;

            builder.Append("<");

            foreach (var genericArgument in type.GetGenericArguments())
                AppendTypeName(builder, genericArgument);

            builder.Append(">");
        }
    }
}