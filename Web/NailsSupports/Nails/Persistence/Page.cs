using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NailsFramework.Support;

namespace NailsFramework.Persistence
{
    public class Page<T> : IPage<T>
    {
        private static readonly IDictionary<Type, int> PageSizeByType = new Dictionary<Type, int>();

        private readonly IQueryable<T> originalQuery;
        private readonly IQueryable<T> pageQuery;
        private int? totalPages;

        public Page(IQueryable<T> queryable, int pageNumber) : this(queryable, pageNumber, GetPageSize())
        {
        }

        public Page(IQueryable<T> queryable, int pageNumber, int pageSize)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
            var skip = Math.Max(PageSize*(pageNumber - 1), 0);
            pageQuery = queryable.Skip(skip).Take(PageSize);
            originalQuery = queryable;
        }

        #region IPage<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return pageQuery.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int PageNumber { get; private set; }

        public int PageSize { get; private set; }


        public int TotalPages
        {
            get
            {
                if (!totalPages.HasValue)
                    totalPages = Math.Max((int) Math.Ceiling(originalQuery.Count()/((decimal) PageSize)), 1);

                return totalPages.Value;
            }
        }

        #endregion

        private static int GetPageSize()
        {
            int pageSize;
            var type = typeof (T);
            if (PageSizeByType.TryGetValue(type, out pageSize))
                return pageSize;

            var attribute = type.Attribute<PageSizeAttribute>(true);

            pageSize = attribute != null ? attribute.PageSize : Nails.Configuration.PageSize;

            PageSizeByType.Add(type, pageSize);
            return pageSize;
        }
    }
}