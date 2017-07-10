using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Encuentrame.Support
{
    //From http://stackoverflow.com/a/26289874/752842
    //By Raif Atef (http://stackoverflow.com/users/111830/raif-atef)
    public static class EnumConverter<TEnum> where TEnum : struct, IConvertible
    {
        public static readonly Func<int, TEnum> Convert = GenerateConverter();

        static Func<int, TEnum> GenerateConverter()
        {
            var parameter = Expression.Parameter(typeof(int));
            var dynamicMethod = Expression.Lambda<Func<int, TEnum>>(
                Expression.Convert(parameter, typeof(TEnum)),
                parameter);
            return dynamicMethod.Compile();
        }
    }
}
