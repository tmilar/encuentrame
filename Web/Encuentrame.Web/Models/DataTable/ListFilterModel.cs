using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Encuentrame.Model;

namespace Encuentrame.Web.Models.DataTable
{
    public abstract class ListFilterModel : FilterModel
    {
        private IEnumerable<MethodInfo> methods;

        protected MethodInfo OneItemMethod
        {
            get
            {
                foreach (var method in Methods)
                {
                    var genericDefinition = GenericDefinitionFirstParameter(method);
                    if (genericDefinition==null)
                        return method;
                }
                return null;
            }
        }

        private static Type GenericDefinitionFirstParameter(MethodInfo method)
        {
            return method.GetParameters()[0].ParameterType.IsGenericType? method.GetParameters()[0].ParameterType.GetGenericTypeDefinition(): null;
        }

        protected MethodInfo ManyItemsMethod
        {
            get
            {
                foreach (var method in Methods)
                {
                    var genericDefinition = GenericDefinitionFirstParameter(method);
                    if (genericDefinition != null && typeof(IList<>).IsAssignableFrom(genericDefinition))
                        return method;
                }

                return null;
            }
        }

        protected IEnumerable<MethodInfo> Methods
        {
            get
            {
                if (this.methods == null)
                {
                    this.methods = seeker.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => x.Name == this.GetSeekerMethodName() && x.GetParameters().Length > 0);
                }
                return this.methods;
            }
            set { this.methods = value; }
        }

        protected override void InvokeImplSeek(params object[] parameters)
        {
            if (parameters.Length > 1)
                this.ManyItemsMethod.Invoke(seeker, parameters);
            else
            {
                this.OneItemMethod.Invoke(seeker, parameters);
            }
        }

        protected virtual void InvokeImplSeek<T>(IList<T> parameters)
        {
            this.ManyItemsMethod.Invoke(seeker, new object[] { parameters });
        }

        public ListFilterModel(IGenericSeeker seeker, SearchModel searchModel)
            : base(seeker, searchModel)
        {
        }
    }
}