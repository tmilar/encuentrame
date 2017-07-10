using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NailsFramework;
using NailsFramework.IoC;
using NailsFramework.Persistence;

namespace Encuentrame.Model
{
    public interface ISeekerFactory<TModel>
        where TModel : class
    {
        IGenericSeeker<TModel> Seek();
    }

    [Lemming]
    public class SeekerFactory<TModel> : ISeekerFactory<TModel>
        where TModel : class
    {
        public IGenericSeeker<TModel> Seek()
        {
            var seeker = Nails.ObjectFactory.GetObject<IGenericSeeker<TModel>>();

            return seeker;
        }
    }
}
