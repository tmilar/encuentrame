using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Encuentrame.Web.MetadataProviders.ConditionalValidations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public abstract class ConditionalValidationAttribute : ValidationAttribute, IClientValidatable
    {
        protected readonly ValidationAttribute InnerAttribute;
        public string DependentProperty { get; set; }
        public object TargetValue { get; set; }
        protected abstract string ValidationName { get; }
        public OperationsEnum Operation { get; set; }

        protected virtual IDictionary<string, object> GetExtraValidationParameters()
        {
            return new Dictionary<string, object>();
        }

        protected ConditionalValidationAttribute(ValidationAttribute innerAttribute, string dependentProperty, object targetValue)
        {
            this.InnerAttribute = innerAttribute;
            this.DependentProperty = dependentProperty;
            this.TargetValue = targetValue;
            Operation = OperationsEnum.Equals;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // get a reference to the property this validation depends upon
            var containerType = validationContext.ObjectInstance.GetType();
            var field = containerType.GetProperty(this.DependentProperty);
            if (field != null)
            {
                // get the value of the dependent property
                var dependentvalue = field.GetValue(validationContext.ObjectInstance, null);

                // compare the value against the target value
                if ((dependentvalue == null && this.TargetValue == null) || Validate(dependentvalue,this.TargetValue))
                {
                    // match => means we should try validating this field
                    if (!InnerAttribute.IsValid(value))
                    {
                        // validation failed - return an error
                        return new ValidationResult(this.ErrorMessage, new[] { validationContext.MemberName });
                    }
                }
            }
            return ValidationResult.Success;
        }

        public virtual bool Validate(object dependentvalue, object targetValue)
        {
            var dependentStringValue = dependentvalue != null ? dependentvalue.ToString() : string.Empty;
            var targetStringValue = targetValue != null ? targetValue.ToString() : string.Empty;

            switch (Operation)
            {
                case OperationsEnum.StartsWith:
                    return dependentStringValue.StartsWith(targetStringValue);
                    break;
                case OperationsEnum.EndsWith:
                    return dependentStringValue.EndsWith(targetStringValue);
                    break;
                case OperationsEnum.Contains:
                    return dependentStringValue.Contains(targetStringValue);
                    break;
                case OperationsEnum.RegularExpression:
                    Regex regex = new Regex(targetStringValue);
                    Match match = regex.Match(dependentStringValue);
                    return match.Success;
                    break;
                case OperationsEnum.Range:
                    double dependentNumericValue;
                    double min;
                    double max;
                    var parts = targetStringValue.Split('|');
                    if (parts.Count() > 1)
                    {
                        var minIsNumeric = Double.TryParse(parts[0], out min);
                        var maxIsNumeric = Double.TryParse(parts[1], out max);
                        var dependentIsNumeric = Double.TryParse(dependentStringValue, out dependentNumericValue);
                        if (dependentIsNumeric)
                        {
                            if (minIsNumeric && maxIsNumeric)
                            {
                                return dependentNumericValue >= min && dependentNumericValue <= max;
                            }
                            else if (minIsNumeric)
                            {
                                return dependentNumericValue >= min;
                            }
                            else if (maxIsNumeric)
                            {
                                return dependentNumericValue <= max;
                            }
                        }
                    }
                    else
                        return false;
                    break;
                case OperationsEnum.HasAny:
                    return dependentvalue != null && !string.IsNullOrEmpty(dependentStringValue);
                    break;
                case OperationsEnum.Equals:
                default:
                    return (dependentvalue != null && dependentvalue.Equals(this.TargetValue));
                    break;
            }
            return false;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = ValidationName,
            };
            string depProp = BuildDependentPropertyId(metadata, context as ViewContext);
            // find the value on the control we depend on; if it's a bool, format it javascript style
            string targetValue = (this.TargetValue ?? "").ToString();
            if (this.TargetValue.GetType() == typeof(bool))
            {
                targetValue = targetValue.ToLower();
            }
            rule.ValidationParameters.Add("dependentproperty", depProp);
            rule.ValidationParameters.Add("targetvalue", targetValue);
            rule.ValidationParameters.Add("operation", Operation.ToString());
            
            // Add the extra params, if any
            foreach (var param in GetExtraValidationParameters())
            {
                rule.ValidationParameters.Add(param);
            }
            yield return rule;
        }

        private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext)
        {
            string depProp = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(this.DependentProperty);
            // This will have the name of the current field appended to the beginning, 
            //because the TemplateInfo's context has had this fieldname appended to it.
            var thisField = metadata.PropertyName + "_";
            if (depProp.Contains(thisField))
            {
                depProp = depProp.Replace(thisField, "");
            }
            return depProp;
        }
    }
}