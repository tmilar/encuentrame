using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NailsFramework.Persistence
{
    public abstract class IdMapper
    {
        private static readonly Func<int, int> IntId = id => id + 1;
        private static readonly Func<long, long> LongId = id => id + 1;
        private static readonly Func<Guid, Guid> GuidId = id => Guid.NewGuid();

        public abstract Type Type { get; }
        public abstract PropertyInfo Property { get; }
        public abstract void SetNewId(object model);

        public static IdMapper Default(PropertyInfo id)
        {
            if (id.PropertyType == typeof (int))
                return new IdMapper<int>(id, IntId);
            if (id.PropertyType == typeof (long))
                return new IdMapper<long>(id, LongId);
            if (id.PropertyType == typeof (Guid))
                return new IdMapper<Guid>(id, GuidId);

            throw new InvalidOperationException(string.Format( "Cannot generate an id for property {0} of type {1}. Property type is {2}. Please, specify a way to get a new id", id.PropertyType, id.DeclaringType, id.PropertyType));
        }
    }

    public class IdMapper<TId> : IdMapper where TId:struct 
    {
        private readonly Func<TId, TId> getNewId;

        public IdMapper(PropertyInfo id,Func<TId, TId> getNewId)
        {
            this.getNewId = getNewId;
            this.id = id;
        }

        private readonly PropertyInfo id;
        private TId currentId;

        public override Type Type
        {
            get { return id.ReflectedType; }
        }

        public override PropertyInfo Property
        {
            get { return id; }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void SetNewId(object model)
        {
            currentId = getNewId(currentId);
            id.SetValue(model, currentId, null);
        }
    }
}