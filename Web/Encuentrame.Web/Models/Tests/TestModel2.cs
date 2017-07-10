using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.MetadataProviders;
using Encuentrame.Web.MetadataProviders.CustomValidations;
using Encuentrame.Web.Models.References.Commons;

namespace Encuentrame.Web.Models.Tests
{
    public class TestModel2
    {
        [Display(Prompt = "ReferenceAnyMultiple")]
        [ReferenceMultiple(MultipleType = true, SourceType = typeof(ListItemsHelper), SourceName = "GetComponents2")]
        //[RequiredReference2Attribute(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        //[RequiredIf("ValueForConditionalValidation", "23", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public List<ReferenceAny> Part6 { get; set; }

        [Display(Name = "AmountOne", Prompt = "AmountOne")]
        [UIHint("integer")]
        public int AmountOne { get; set; }


        [Display(Name = "AmountTwo", Prompt = "AmountTwo")]
        [LessThan("AmountOne",ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "LessThanOtherPropertyValidationError")]
        [UIHint("integer")]
        public int AmountTwo { get; set; }

    }
}