using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class EnumFilter <T, TEnum> : MultipleValueFilter<T, List<TEnum>, TEnum>, IEnumFilter
        where T : class
        where TEnum : struct
    {
        #region Properties

        public Type EnumType { get { return typeof(TEnum); } }

        public override bool CanAddDefaultValue { get { return this.DefaultValueSet; } }

        public bool DefaultValueSet { get; set; }

        #endregion

        #region Methods

        public override ITableMulipleValueFilter<T, List<TEnum>, TEnum> AddDefaultValue(List<TEnum> values)
        {
            this.DefaultValueSet = true;
            base.AddDefaultValue(values);
            return this;
        }

        public override ITableMulipleValueFilter<T, List<TEnum>, TEnum> AddDefaultValue(TEnum value)
        {
            this.DefaultValueSet = true;
            base.AddDefaultValue(value);
            return this;
        }

        #endregion

        protected override string ElementToString(TEnum element)
        {
            var name = Enum.GetName(this.EnumType, element);
            var intValue = (int) Enum.Parse(this.EnumType, name);
            return intValue.ToString();
        }

        public override string DisplayTemplate { get { return typeof(IEnumFilter).Name; } }
    }
}