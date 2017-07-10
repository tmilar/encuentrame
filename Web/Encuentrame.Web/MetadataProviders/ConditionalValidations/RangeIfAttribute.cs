using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Encuentrame.Web.MetadataProviders.ConditionalValidations
{
    public class RangeIfAttribute : ConditionalValidationAttribute
    {
        private readonly int minimum;
        private readonly int maximum;
        protected override string ValidationName
        {
            get { return "rangeif"; }
        }
        public RangeIfAttribute(int minimum, int maximum, string dependentProperty, object targetValue)
            : base(new RangeAttribute(minimum, maximum), dependentProperty, targetValue)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }
        protected override IDictionary<string, object> GetExtraValidationParameters()
        {
            // Set the rule Range and the rule param [minumum,maximum]
            return new Dictionary<string, object> 
            { 
                {"rule", "range"},
                { "ruleparam", string.Format("[{0},{1}]", this.minimum, this.maximum) } 
            };
        }
    }
}