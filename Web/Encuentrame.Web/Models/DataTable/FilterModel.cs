using System;
using System.Collections;
using System.Reflection;
using Encuentrame.Model;

namespace Encuentrame.Web.Models.DataTable
{
    public abstract class FilterModel
    {
        protected IGenericSeeker seeker;
        protected SearchModel searchModel;
        private MethodInfo method;
        protected MethodInfo Method
        {
            get
            {
                if (this.method == null)
                    this.method = seeker.GetType().GetMethod(this.GetSeekerMethodName());
                return this.method;
            }
            set { this.method = value; }
        }
        protected string Value { get { return this.searchModel.Value; } }
        protected FilterModel(IGenericSeeker seeker, SearchModel searchModel)
        {
            this.seeker = seeker;
            this.searchModel = searchModel;
        }

        protected static Type GenericArgumentType(Type parameterType)
        {
            var isList = parameterType.IsGenericParameter && (parameterType as IList) != null;
            Type genericArgumentType = null;
            if (isList)
            {
                genericArgumentType = parameterType.GetType().GetGenericArguments()[0];
            }
            return genericArgumentType;
        }

        protected virtual void InvokeImplSeek(params object[] parameters)
        {
            Method.Invoke(seeker, parameters);
        }

        protected string GetSeekerMethodName()
        {
            return string.Format("By{0}", this.searchModel.Column);
        }

        public abstract void Seek();
    }
}