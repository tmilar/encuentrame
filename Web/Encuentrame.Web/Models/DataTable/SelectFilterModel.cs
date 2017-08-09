using System;
using System.Collections;
using System.Collections.Generic;
using Encuentrame.Model;

namespace Encuentrame.Web.Models.DataTable
{
    public class SelectFilterModel : ListFilterModel
    {
        public SelectFilterModel(IGenericSeeker seeker, SearchModel searchModel) : base(seeker, searchModel)
        {
        }

        public override void Seek()
        {
            if (!string.IsNullOrWhiteSpace(Value))
            {
                var parameters = this.OneItemMethod.GetParameters();
                if (parameters.Length > 0)
                {
                    var parameterType = parameters[0].ParameterType;

                    if (parameterType == typeof(int))
                    {
                        this.FilterIntReference();
                    }
                    else if (parameterType == typeof(string))
                    {
                        this.FilterStringReference();
                    }
                }
            }
        }

        private void FilterStringReference()
        {
            var values = this.Value.Split('|');
            if (values.Length > 1)
                this.InvokeImplSeek(values);
            else
                this.InvokeImplSeek(this.Value);
        }

        private void FilterIntReference()
        {
            int intValue;
            if (int.TryParse(this.Value, out intValue))
            {
                this.InvokeImplSeek(intValue);
            }
            else
            {
                var values = this.Value.Split('|');
                var intValues = new List<int>();
                foreach (var valueToConvert in values)
                    if (int.TryParse(valueToConvert, out intValue))
                        intValues.Add(intValue);
                if (intValues.Count > 0)
                    this.InvokeImplSeek(intValues);
            }
        }
    }
}