using System;
using System.Web.Mvc;
using Encuentrame.Web.Models.References.Commons;

namespace Encuentrame.Web.MetadataProviders
{
    public class ReferenceAnyModelBinder : DefaultModelBinder
    {
        protected override object CreateModel(
            ControllerContext controllerContext,
            ModelBindingContext bindingContext,
            Type modelType)
        {
            if (modelType == typeof(ReferenceAny))
            {
                var value = (string)bindingContext.ValueProvider.GetValue(string.Format("{0}.referenceAny", bindingContext.ModelName)).ConvertTo(typeof(string));
                ReferenceAny referenceAny = new ReferenceAny(); ;
                if (!string.IsNullOrEmpty(value))
                {
                    var values = value.Split('#');
                    referenceAny.Type = values[0];
                    referenceAny.Id = Convert.ToInt32(values[1]);
                }

                //bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, modelType);
                bindingContext.ModelMetadata.Model = referenceAny;
                return referenceAny;
            }

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
    }
}