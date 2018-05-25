using System;

namespace Encuentrame.Support
{
    public static class ComparableExtensions
    {
        public static bool IsGreaterOrEqualThan(this IComparable first, IComparable second)
        {
            return first.CompareTo(second) >= 0;
        }
        public static bool IsGreaterThan(this IComparable first, IComparable second)
        {
            return first.CompareTo(second) > 0;
        }
        public static bool IsLessOrEqualThan(this IComparable first, IComparable second)
        {
            return first.CompareTo(second) <= 0;
        }
        public static bool IsLessThan(this IComparable first, IComparable second)
        {
            return first.CompareTo(second) < 0;
        }
        public static bool IsEqualThan(this IComparable first, IComparable second)
        {
            return first.CompareTo(second) == 0;
        }
    }
}