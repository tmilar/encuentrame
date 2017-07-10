using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NailsFramework.IoC;

namespace NailsFramework.Persistence
{
    public abstract class Model
    {
        [Inject]
        public static BagFactory BagFactory { private get; set; }

        [Inject]
        public static IPersistenceContext PersistenceContext { protected get; set; }

        protected IQueryable<TResult> QueryAll<TResult>() where TResult : class
        {
            return BagFactory.GetBag<TResult>();
        }
    }

    public abstract class Model<TModel> : Model where TModel : Model<TModel>
    {
        protected IQueryable<TResult> QueryCollection<TResult>(Expression<Func<TModel, IEnumerable<TResult>>> query)
        {
            //.SelectMany(query, (x, y) => y) is a workaround for NHibernate not supporting the simplier signature
            return QueryAll().Where(GetIdEqualityExpression(this)).SelectMany(query, (x, y) => y);
        }

        protected IQueryable<TModel> QueryAll()
        {
            return QueryAll<TModel>();
        }

        private static Expression<Func<TModel, bool>> GetIdEqualityExpression(Model<TModel> obj)
        {
            var parameter = Expression.Parameter(typeof (TModel), "x");

            var idProperty = PersistenceContext.GetIdPropertyOf<TModel>();

            var objectValue = Expression.Constant(obj);
            var idOfObject = Expression.Property(objectValue, idProperty);

            var equal = Expression.Equal(Expression.Property(parameter, idProperty.Name), idOfObject);

            return Expression.Lambda<Func<TModel, bool>>(equal, parameter);
        }
    }
}