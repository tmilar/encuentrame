using System;

namespace Encuentrame.Support
{
    public static class EnumExtensions
    {
        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        public static T ToEnum<T>(this int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }

        public static bool HasValue(this Enum enumValue)
        {
            return Enum.IsDefined(enumValue.GetType(), enumValue);
        }
        public static bool DontHasValue(this Enum enumValue)
        {
            return !enumValue.HasValue();
        }
    }
}