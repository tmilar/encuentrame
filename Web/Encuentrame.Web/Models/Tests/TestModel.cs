using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.MetadataProviders;
using Encuentrame.Web.MetadataProviders.ConditionalValidations;
using Encuentrame.Web.MetadataProviders.CustomValidations;
using Encuentrame.Web.Models.References.Commons;
using Encuentrame.Web.Models.Tests;

namespace Encuentrame.Web.Models.Tests
{
    public class TestModel
    {
        public List<TestSelectItemClass> FullList { get; set; }
        public List<int> SelectedTestItems { get; set; }

        [Required]
        [Cuit(ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "CuitValidationError")]
        public string Cuit { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Client")]
        [RequiredReference2(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Reference(SourceType = typeof(ListItemsHelper), SourceName = "GetClients")]
        public int Client { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Part")]
        [Reference(RelatedTo = "Client", SourceController = "ReferenceItems", SourceAction = "GetPartsFinalByClient")]
        [RequiredIf("ValueForConditionalValidation", "2", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredIfErrorValueForConditionalValidationIs2")]               
        //[RequiredReference(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public int Part { get; set; }

        #region NewReference
                   
        [Display(ResourceType = typeof(Translations), Name = "Part")]
        //[Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Reference(RelatedTo = "Client", SourceController = "ReferenceItems", SourceAction = "GetPartsFinalByClient")]        
        public int Part2 { get; set; }        

        [Display(ResourceType = typeof(Translations), Name = "Client")]
        //[Required(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [Reference(SourceType = typeof(ListItemsHelper), SourceName = "GetClients")]
        public int Client2 { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Part")]
        [Reference(RelatedTo = "Client2", SourceController = "ReferenceItems", SourceAction = "GetPartsFinalByClient")]
        //[RequiredReference2Attribute(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        //[RequiredIf("ValueForConditionalValidation", "23", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public int? Part3 { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Client")]
        //[RequiredReference(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [ReferenceMultiple(SourceType = typeof(ListItemsHelper), SourceName = "GetClients2")]
        //[RequiredList(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [RequiredIf("ValueForConditionalValidation", "23", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public List<int> Client3 { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Part")]
        [ReferenceMultiple(RelatedTo = "Client2", SourceController = "ReferenceItems", SourceAction = "GetPartsFinalByClient")]
        [RequiredList(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        //[RequiredIf("ValueForConditionalValidation", "23", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public List<int> Part4 { get; set; }

        [ReferenceMultiple(SourceType = typeof(ListItemsHelper), SourceName = "GetClients2")]
        [RequiredList(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]        
        public List<int> ClientsMultipleSelected { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "Part")]
        [Reference(MultipleType = true, SourceType = typeof(ListItemsHelper), SourceName = "GetComponents")]
        //[RequiredReference2Attribute(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [RequiredIf("ValueForConditionalValidation", "23", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public ReferenceAny Part5 { get; set; }

        [Display(Prompt = "ReferenceAnyMultiple")]
        [ReferenceMultiple(MultipleType = true, SourceType = typeof(ListItemsHelper), SourceName = "GetComponents2")]
        //[RequiredReference2Attribute(AllowEmptyStrings = false, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        [RequiredIf("ValueForConditionalValidation", "23", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public List<ReferenceAny> Part6 { get; set; }
        #endregion

        [EnumReference(ExcludeValues = new int[] { (int)TestEnum.ExcludeValue1, (int)TestEnum.ExcludeValue2})]
        public TestEnum TestExcludeEnumValues { get; set; }

        [EnumReference(IncludeValues = new int[] { (int)TestEnum.IncludeValue1, (int)TestEnum.IncludeValue3 })]
        [RequiredIf("TestExcludeEnumValues", (int)TestEnum.IncludeValue1, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public TestEnum TestIncludeEnumValues { get; set; }

        public string ValueForConditionalValidation { get; set; }

        [RequiredIf("ValueForConditionalValidation", 23, ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError")]
        public string RequiredIfEqual_23 { get; set; }


        [RequiredIf("ValueForConditionalValidation", @"^([a-z0-9]{5,})$", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError", Operation = OperationsEnum.RegularExpression)]
        public string RequiredIfRegularExpression_abc12 { get; set; }

        [RequiredIf("ValueForConditionalValidation", "Hola", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError", Operation = OperationsEnum.StartsWith)]
        public string RequiredIfStartsWith_Hola { get; set; }

        [RequiredIf("ValueForConditionalValidation", "Chau", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError", Operation = OperationsEnum.EndsWith)]
        public string RequiredIfEndsWith_Chau { get; set; }

        [RequiredIf("ValueForConditionalValidation", "Algo", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError", Operation = OperationsEnum.Contains)]
        public string RequiredIfContains_Algo { get; set; }

        [RequiredIf("ValueForConditionalValidation", "3|10", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError", Operation = OperationsEnum.Range)]
        public string RequiredIfRange_3_10 { get; set; }

        [RequiredIf("ValueForConditionalValidation", "", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RequiredError", Operation = OperationsEnum.HasAny)]
        public string RequiredIfAny { get; set; }

        [RangeIf(0, int.MaxValue, "ValueForConditionalValidation", "Chau", ErrorMessageResourceType = typeof(Translations), ErrorMessageResourceName = "RangeErrorGreaterThanZero", Operation = OperationsEnum.EndsWith)]
        public string RangedIfEndsWith_Chau { get; set; }


        [Display(ResourceType = typeof(Translations), Name = "Roles")]
        [CheckboxList(SourceType = typeof(ListItemsHelper), SourceName = "GetRolesSelectList")]
        public IList<int> Roles { get; set; }

        [Display(ResourceType = typeof(Translations), Name = "TestEnums")]
        [EnumReference( MultipleSelection =true)]
        public List<TestEnum> TestEnums { get; set; }
    }
}