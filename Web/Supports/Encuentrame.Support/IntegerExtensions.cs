using System;

namespace Encuentrame.Support
{
    public static class IntegerExtensions
    {
        public static void Times(this int times, Action action)
        {
            for (var i = 0; i < times; i++)
                action();
        }

        public static void Times(this int times, Action<int> action)
        {
            for (var i = 0; i < times; i++)
                action(i);
        }

        public static void For(this int count, Action<int> action)
        {
            for (var i = 0; i <= count; i++)
                action(i);
        }

        public static void For(this int count, int start, Action<int> action)
        {
            for (var i = start; i <= count; i++)
                action(i);
        }


        public static T AsEnum<T>(this int value) where T : struct
        {
            var result = value.TryAsEnum<T>();
            if (result.HasValue)
                return result.Value;
            throw new ArgumentException(string.Format("Value is not a valid element of enum {0}", typeof(T).Name));
        }

        public static T? TryAsEnum<T>(this int value) where T : struct
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                if (value == (int)item)
                    return (T)item;
            }
            return null;
        }

        public static bool Between(this int i, int start, int end)
        {
            return i >= start && i <= end;
        }

        public static bool HasValueIdentifiable(this int? value)
        {
            return value.HasValue && value.Value > 0;
        }

    }
}