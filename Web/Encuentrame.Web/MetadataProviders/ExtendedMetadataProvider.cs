using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Encuentrame.Web.Helpers;
using Encuentrame.Support;

namespace Encuentrame.Web.MetadataProviders
{
    public class ExtendedMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType,
            string propertyName)
        {
            var metadata=base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);        

            var reference2Attribute = attributes.OfType<ReferenceAttribute>().FirstOrDefault();
            if (reference2Attribute != null)
            {
                metadata.TemplateHint = reference2Attribute.TemplateHint;
                if (reference2Attribute.SourceType != null)
                {
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.SourceType, reference2Attribute.SourceType);
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.SourceName, reference2Attribute.SourceName);
                }
                if (reference2Attribute.RelatedTo.NotIsNullOrEmpty())
                {
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.RelatedTo, reference2Attribute.RelatedTo);
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.SourceController, reference2Attribute.SourceController);
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.SourceAction, reference2Attribute.SourceAction);
                }
            }

            var checkboxListAttribute = attributes.OfType<CheckboxListAttribute>().FirstOrDefault();
            if (checkboxListAttribute != null)
            {
                metadata.TemplateHint = checkboxListAttribute.TemplateHint;
                if (checkboxListAttribute.SourceType != null)
                {
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.SourceType, checkboxListAttribute.SourceType);
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.SourceName, checkboxListAttribute.SourceName);
                }
             
                if (checkboxListAttribute.Parameters.NotIsNullOrEmpty())
                {
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.Parameters, checkboxListAttribute.Parameters);
                }
            }

            var referenceMultipleAttribute = attributes.OfType<ReferenceMultipleAttribute>().FirstOrDefault();
            if (referenceMultipleAttribute != null)
            {
                metadata.TemplateHint = referenceMultipleAttribute.TemplateHint;
                if (referenceMultipleAttribute.SourceType != null)
                {
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.SourceType, referenceMultipleAttribute.SourceType);
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.SourceName, referenceMultipleAttribute.SourceName);
                }
                if (referenceMultipleAttribute.RelatedTo.NotIsNullOrEmpty())
                {
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.RelatedTo, referenceMultipleAttribute.RelatedTo);
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.SourceController, referenceMultipleAttribute.SourceController);
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.SourceAction, referenceMultipleAttribute.SourceAction);
                }
                if (referenceMultipleAttribute.Parameters.NotIsNullOrEmpty())
                {
                    metadata.AdditionalValues.Add(AdditionalMetadataKeys.Parameters, referenceMultipleAttribute.Parameters);
                }
            }

            var tree = attributes.OfType<TreeAttribute>().FirstOrDefault();
            if (tree != null)
            {
                metadata.TemplateHint = tree.TemplateHint;
                metadata.AdditionalValues.Add(AdditionalMetadataKeys.SourceType, tree.SourceType);
                metadata.AdditionalValues.Add(AdditionalMetadataKeys.SourceName, tree.SourceName);
            }

            var datetime = attributes.OfType<DateTimeAttribute>().FirstOrDefault();
            if (datetime != null)
            {
                metadata.TemplateHint = datetime.TemplateHint;
                metadata.AdditionalValues.Add(AdditionalMetadataKeys.CurrentIsMaxDateTime, datetime.CurrentIsMaxDateTime);
            }

            var enumAttribute = attributes.OfType<EnumReferenceAttribute>().FirstOrDefault();
            if (enumAttribute != null)
            {
                metadata.TemplateHint = enumAttribute.TemplateHint;
                metadata.AdditionalValues.Add(AdditionalMetadataKeys.ExcludedEnumValues, enumAttribute.ExcludeValues);
                metadata.AdditionalValues.Add(AdditionalMetadataKeys.IncludedEnumValues, enumAttribute.IncludeValues);
            }

              

            return metadata;
        }
        
    }
}