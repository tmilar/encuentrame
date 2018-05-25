using System.Linq;
using Encuentrame.Model;

namespace Encuentrame.Web.Models.DataTable
{
    public class RangeFilterModel : FilterModel
    {
        public RangeFilterModel(IGenericSeeker seeker, SearchModel searchModel) : base(seeker, searchModel)
        {
        }

        public override void Seek()
        {
            if (!string.IsNullOrEmpty(Value))
            {
                var parameters = this.Method.GetParameters();
                if (parameters.Length > 0)
                {
                    var parameterType = parameters[0].ParameterType;
                    var genericArgumentType = GenericArgumentType(parameterType);

                    if (parameterType == typeof(int) || parameterType == typeof(int?) || genericArgumentType == typeof(int))
                    {
                        this.FilterIntRange();
                    }
                    else if (parameterType == typeof(decimal) || parameterType == typeof(decimal?) || genericArgumentType == typeof(decimal))
                    {
                        this.FilterRangeDecimal();
                    }
                }
            }
        }

        private void FilterRangeDecimal()
        {
            var rangeValues = this.Value.Split('|');
            if (rangeValues.Any())
            {
                var minStringValue = rangeValues[0];
                var maxStringValue = rangeValues[1];

                decimal minValue;
                decimal maxValue;
                var isMinValid = decimal.TryParse(minStringValue, out minValue);
                var isMaxValid = decimal.TryParse(maxStringValue, out maxValue);

                if (isMinValid || isMaxValid)
                    if (isMaxValid && isMinValid)
                        this.InvokeImplSeek(minValue, maxValue);
                    else if (isMaxValid)
                        this.InvokeImplSeek((decimal?) null, maxValue);
                    else
                        this.InvokeImplSeek(minValue, (decimal?) null);
            }
        }

        private void FilterIntRange()
        {
            var rangeValues = this.Value.Split('|');
            if (rangeValues.Any())
            {
                var minStringValue = rangeValues[0];
                var maxStringValue = rangeValues[1];

                int minValue;
                int maxValue;
                var isMinValid = int.TryParse(minStringValue, out minValue);
                var isMaxValid = int.TryParse(maxStringValue, out maxValue);

                if (isMinValid || isMaxValid)
                    if (isMaxValid && isMinValid)
                        this.InvokeImplSeek(minValue, maxValue);
                    else if (isMaxValid)
                        this.InvokeImplSeek((int?) null, maxValue);
                    else
                        this.InvokeImplSeek(minValue, (int?) null);
            }
        }
    }
}