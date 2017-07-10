using System;

namespace NailsFramework.Persistence
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PageSizeAttribute : Attribute
    {
        public PageSizeAttribute(int pageSize)
        {
            PageSize = pageSize;
        }

        public int PageSize { get; private set; }
    }
}