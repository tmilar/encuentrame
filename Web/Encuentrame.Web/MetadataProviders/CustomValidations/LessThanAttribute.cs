using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Encuentrame.Support;

namespace Encuentrame.Web.MetadataProviders.CustomValidations
{
    public class LessThanAttribute : ValidationAttribute, IClientValidatable
    {
        public LessThanAttribute(string otherProperty)
        {
            OtherProperty = otherProperty;
        }

        protected string ValidationName
        {
            get { return "lessthanotherproperty"; }
        }

        public string OtherProperty { get; set; }

        public string FormatErrorMessage(string name, string otherName)
        {
            return string.Format(ErrorMessageString, name, otherName);
        }

        protected override ValidationResult
            IsValid(object firstValue, ValidationContext validationContext)
        {
            var firstComparable = firstValue as IComparable;
            var secondComparable = GetSecondComparable(validationContext);

            if (firstComparable != null && secondComparable != null)
            {
                if (!firstComparable.IsLessOrEqualThan(secondComparable)) 
                {
                    object obj = validationContext.ObjectInstance;
                    var thing = obj.GetType().GetProperty(OtherProperty);
                    var displayName = (DisplayAttribute)Attribute.GetCustomAttribute(thing, typeof(DisplayAttribute));

                    return new ValidationResult(
                        FormatErrorMessage(validationContext.DisplayName, displayName.GetName()));
                }
            }

            return ValidationResult.Success;
        }

        protected IComparable GetSecondComparable(
            ValidationContext validationContext)
        {
            var propertyInfo = validationContext
                                  .ObjectType
                                  .GetProperty(OtherProperty);
            if (propertyInfo != null)
            {
                var secondValue = propertyInfo.GetValue(
                    validationContext.ObjectInstance, null);
                return secondValue as IComparable;
            }
            return null;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            Type type = Type.GetType(metadata.ContainerType.FullName);
            var model = metadata.Model;
            var provider = new DataAnnotationsModelMetadataProvider();
            var otherMetaData = provider.GetMetadataForProperty(() => model, type, this.OtherProperty);

            var otherPropertyDisplayName = otherMetaData.DisplayName;

            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName(), otherPropertyDisplayName),
                ValidationType = ValidationName,
            };
            rule.ValidationParameters.Add("otherproperty", OtherProperty);


            yield return rule;
        }
    }
}