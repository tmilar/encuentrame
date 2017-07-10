using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NailsFramework.IoC;

namespace NailsFramework.UserInterface
{
    /// <summary>
    /// Implementation of the <see cref="IFilterProvider"/> that will inject any dependency for the filters. It acts as a wrapper of another provider that can be 
    /// provided in the constructor.
    /// </summary>
    /// <remarks>
    /// The class is not marked as Lemming since, when using the <see cref="DependecyResolver" />, it's duplicating the calls to the filters causing unit of work to fail.
    /// </remarks>
    public class NailsApiFilterProvider : IFilterProvider
    {
        [Inject]
        public NailsFramework.IoC.IObjectFactory ObjectFactory { private get; set; }

        IFilterProvider internalProvider;
        
        /// <summary>
        /// Creates an instance of the <see cref="NailsApiFilterProvider"/> around an <see cref="ActionDescriptorFilterProvider"/>
        /// </summary>
        public NailsApiFilterProvider() : this (new ActionDescriptorFilterProvider())
        {
        }

        /// <summary>
        /// Creates an instance of the <see cref="NailsApiFilterProvider"/> around the <see cref="filterProvider"/>
        /// </summary>
        /// <param name="filterProvider">The real <see cref="IFilterProvider"/> that will be wrapped by the instance being created</param>
        public NailsApiFilterProvider(IFilterProvider filterProvider)
        {
            internalProvider = filterProvider;
        }

        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            foreach (var filterInfo in internalProvider.GetFilters(configuration, actionDescriptor))
            {
                ObjectFactory.Inject(filterInfo.Instance.GetType(), filterInfo.Instance);
                yield return filterInfo;
            }
        }

    }
}
