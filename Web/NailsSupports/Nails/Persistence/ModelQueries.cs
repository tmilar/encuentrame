using System;
using System.Linq;
using System.Linq.Expressions;
using NailsFramework.IoC;

namespace NailsFramework.Persistence
{
    public static class ModelQueries
    {
        [Inject]
        public static IPersistenceContext PersistenceContext { private get; set; }
        
        public static T GetById<T>( this IQueryable<T> self, object id) where T : class
        {
            return self.SingleOrDefault(IdEqualsExpression<T>(id));
        }
        
        private static Expression<Func<T, bool>> IdEqualsExpression<T>(object id) where T : class
        {
            var type = typeof(T);

            var idProperty = PersistenceContext.GetIdPropertyOf<T>();

            var getByIdparameter = Expression.Parameter(type, "x");

            var idEqualParameter = Expression.Equal(Expression.Property(getByIdparameter, idProperty.Name),
                                                    Expression.Constant(id, idProperty.PropertyType));

            var lambdaGetById = Expression.Lambda<Func<T, bool>>(idEqualParameter, getByIdparameter);

            return lambdaGetById;
        }
    }
}