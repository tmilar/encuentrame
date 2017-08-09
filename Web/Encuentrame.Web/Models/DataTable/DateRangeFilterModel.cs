using System;
using System.Globalization;
using System.Linq;
using Encuentrame.Model;
using Encuentrame.Web.Models.DataTable;

namespace Encuentrame.Web.Models.DataTable
{
    public class DateRangeFilterModel: FilterModel
    {
        public DateRangeFilterModel(IGenericSeeker seeker, SearchModel searchModel) : base(seeker, searchModel)
        {
        }

        public override void Seek()
        {
            var value = this.searchModel.Value;
            if (!string.IsNullOrEmpty(value))
            {
                var rangeValues = value.Split('|');
                if (rangeValues.Any())
                {
                    var minStringValue = rangeValues[0];
                    var maxStringValue = rangeValues[1];

                    DateTime minValue;
                    DateTime maxValue;
                    var isMinValid = DateTime.TryParseExact(minStringValue, "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out minValue);
                    var isMaxValid = DateTime.TryParseExact(maxStringValue, "dd/MM/yyyy HH:mm",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out maxValue);

                    if (isMinValid || isMaxValid)
                        if (isMaxValid && isMinValid)
                            this.InvokeImplSeek(minValue, maxValue);
                        else if (isMaxValid)
                            this.InvokeImplSeek((DateTime?)null, maxValue);
                        else
                            this.InvokeImplSeek(minValue, (DateTime?)null);
                }
            }
        }
    }
}