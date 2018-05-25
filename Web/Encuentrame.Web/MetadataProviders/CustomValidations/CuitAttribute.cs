using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Encuentrame.Support;

namespace Encuentrame.Web.MetadataProviders.CustomValidations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class CuitAttribute : ValidationAttribute, IClientValidatable
    {
        protected  string ValidationName
        {
            get { return "cuit"; }
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!ValidateCuit((string)value))
            {
                return new ValidationResult(this.ErrorMessage, new[] {validationContext.MemberName});
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = ValidationName,
            };
            
           
            yield return rule;
        }

        private bool ValidateCuit(string cuit)
        {
            if (cuit.IsNullOrEmpty())
            {
                return true;    
            }
            
            var cuitNumber = cuit.Remove(" ").Remove("-");

            if (cuitNumber.Length!=11)
            {
                return false;
            }

            if (!cuitNumber.IsNumber())
            {
                return false;
            }

            var lastDigit= GetLastDigitCuit(cuitNumber);


            return cuitNumber.Substring(10) == lastDigit.ToString();

        }

        private int GetLastDigitCuit(string cuitNumber)
        {
            int[] multipliers = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

            int total = 0;

            for (int i = 0; i < multipliers.Length; i++)
            {

                total += int.Parse(cuitNumber[i].ToString()) * multipliers[i];

            }

            var rest = total % 11;

            return rest == 0 ? 0 : rest == 1 ? 9 : 11 - rest;
        }

    }
}