using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Encuentrame.Support
{
    public static class CollectionExtensions
    {
  
        public static void AddList<T>(this IList<T> list, IList<T> source)
        {
            foreach (var item in source)
            {
                list.Add(item);   
            }
        }

        public static bool IsNotEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.IsEmpty();
        }

        public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }

        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection.Count == 0;
        }

        public static bool IsNotEmpty<T>(this ICollection<T> collection)
        {
            return !collection.IsEmpty();
        }

        public static IList<int> AsIdList<T>(this IList<T> list) where T : IIdentifiable
        {
            var idList = new List<int>();
            foreach (var item in list)
            {
                idList.Add(item.Id);
            }
            return idList;
        }

        public static IList<T> AddIfSomething<T>(this IList<T> list, T item) where T : class
        {
            if (item != null)
                list.Add(item);
            return list;
        }

        public static IEnumerable<int> Ids<T>(this IEnumerable<T> items) where T : IIdentifiable
        {
            return items.Select(x => x.Id);
        }

        public static string Join<T>(this IEnumerable<T> enumerable, string separator, Func<T, string> converter)
        {
            return string.Join(separator, enumerable.Select(converter).ToArray());
        }

        public static string Join<T>(this IEnumerable<T> enumerable, string separator)
        {
            return enumerable.Join(separator, x => x.ToString());
        }

        public static IEnumerable<T> ConcatWithOutRepeatedItems<T>(this IEnumerable<T> values, IEnumerable<T> extraValues)
        {
            var notRepeatedItems = extraValues.Where(x => x.NotIn(values));
            return values.Concat(notRepeatedItems);
        }

        public static IEnumerable<U> Iterate<T, U>(this IEnumerable<T> items, Func<T, IEnumerable<U>> getChilds)
        {
            foreach (var item in items)
            {
                foreach (var child in getChilds(item))
                {
                    yield return child;
                }
            }
        }

        public static void ForEach<T>(this IEnumerable items, Action<T> action)
        {
            foreach (T item in items)
                action(item);
        }
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
                action(item);
        }

        public static bool In<T>(this T item, params T[] items)
        {
            return items.Contains(item);
        }

        public static bool NotIn<T>(this T item, params T[] items)
        {
            return !item.In(items);
        }

        public static bool In<T>(this T item, IEnumerable<T> items)
        {
            return items.Contains(item);
        }

        public static bool NotIn<T>(this T item, IEnumerable<T> items)
        {
            return !item.In(items);
        }

        public static IList<T> NotDeleted<T>(this IEnumerable<T> items) where T : IDeleteable
        {
            return items.Where(x => !x.IsDeleted()).ToList();
        }

        public static IList<T> ReplaceWith<T>(this IList<T> list, IEnumerable<T> other)
        {
            list.Clear();
            other.ForEach(list.Add);
            return list;
        }

        public static IEnumerable<T> As<T>(this IEnumerable list)
        {
            return list.Cast<T>();
        }


        public static bool Matches<T>(this IEnumerable<T> list1, IEnumerable<T> list2)
        {
            return list1.Any(s => s.In(list2.ToList()));
        }

        public static IEnumerable<T> Page<T>(this IEnumerable<T> items, int page, int pageSize)
        {
            var list = items.ToList();
            var pageCount = list.PageCount(pageSize);

            if (pageCount < page)
                if (pageCount > 0)
                    page--;
                else
                    throw new InvalidOperationException("Invalid page.");

            return list.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public static int PageCount<T>(this IEnumerable<T> items, int pageSize)
        {
            if (pageSize == 0) return 1;

            var count = items.Count();
            if (count == 0)
                return 1;
            return (count - (count % pageSize == 0 ? 1 : 0)) / pageSize + 1;
        }

        public static IList<T> RemoveAcordingTo<T>(this IList<T> list, Func<T, bool> condition)
        {
            var element = list.FirstOrDefault(condition);
            if (!Equals(element, default(T)))
                list.Remove(element);
            return list;
        }
    }
}
