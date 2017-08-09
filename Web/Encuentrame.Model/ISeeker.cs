using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailsFramework.Persistence;
using Encuentrame.Support;

namespace Encuentrame.Model
{
    public interface IGenericSeeker
    {
        int Count();
    }

    public interface IGenericSeeker<TModel> : IGenericSeeker where TModel : class
    {
        IList<TModel> ToList();
        IGenericSeeker<TModel> Skip(int start);
        IGenericSeeker<TModel> Take(int length);
    }

    public interface ISeeker<TModel> : IGenericSeeker<TModel> where TModel : class, IIdentifiable
    {
    }
}
