using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Encuentrame.Web.Models.References.Commons;

namespace Encuentrame.Web.MetadataProviders
{
    public class ReferenceAnyListModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;
            var referenceAnies = new List<ReferenceAny>();
            if (!string.IsNullOrEmpty(value))
            {
                var listOfValues = value.Split(',');
                foreach (var listOfValue in listOfValues)
                {
                    //Data comes with this format: RawMaterial#86,RawMaterial#52
                    var values = listOfValue.Split('#');
                    var referenceAny = new ReferenceAny();
                    referenceAny.Type = values[0];
                    referenceAny.Id = Convert.ToInt32(values[1]);
                    referenceAnies.Add(referenceAny);
                }
            }

            return referenceAnies;
        }
    }
}