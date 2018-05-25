using System;
using System.Collections.Generic;
using System.Reflection;
using Encuentrame.Model;
using Encuentrame.Support;

namespace Encuentrame.Web.Models.DataTable
{
    public class EnumFilterModel : ListFilterModel
    {
        public EnumFilterModel(IGenericSeeker seeker, SearchModel searchModel) : base(seeker, searchModel)
        {
        }

        protected void ApplyEnumFilter<TEnum>() where TEnum : struct, IConvertible
        {
            var value = this.searchModel.Value;
            if (typeof(TEnum).IsEnum && !string.IsNullOrWhiteSpace(value))
            {
                int intValue;

                if (int.TryParse(value, out intValue))
                {
                    if (Enum.IsDefined(typeof(TEnum), intValue))
                    {
                        var status = EnumConverter<TEnum>.Convert(intValue);
                        this.InvokeImplSeek(status);
                    }
                }
                else
                {
                    var stringValues = value.Split('|');
                    if (stringValues.Length > 0)
                    {
                        var enumValues = new List<TEnum>();
                        foreach (var stringValue in stringValues)
                            if (int.TryParse(stringValue, out intValue))
                                if (Enum.IsDefined(typeof(TEnum), intValue))
                                {
                                    var status = EnumConverter<TEnum>.Convert(intValue);
                                    enumValues.Add(status);
                                }

                        if (enumValues.Count > 0)
                            this.InvokeImplSeek(enumValues);
                    }
                }
            }
        }

        public override void Seek()
        {
            var parameters = this.OneItemMethod.GetParameters();
            if (parameters.Length > 0)
            {
                var applyFilter = this.GetType().GetMethod("ApplyEnumFilter", BindingFlags.NonPublic | BindingFlags.Instance);
                MethodInfo genericMethod = applyFilter.MakeGenericMethod(parameters[0].ParameterType);
                genericMethod.Invoke(this, null);
            }
        }
    }
}