using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Encuentrame.Web.Models.References.Commons;

namespace Encuentrame.Web.MetadataProviders.CustomValidations
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredListAttribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var list = value as IEnumerable;
            return list != null && list.GetEnumerator().MoveNext();
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredReference2Attribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            int intValue;
            if (value!=null && int.TryParse(value.ToString(), out intValue))
            {
                //TODO: We could review the entity and validate that the ID actually exists.
                if (intValue > 0)
                    return true;
                return false;
            }

            return false;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredReferenceAny2Attribute : RequiredAttribute
    {
        public override bool IsValid(object value)
        {
            var referenceAny = value as ReferenceAny;
            if (referenceAny != null && referenceAny.Id > 0)
            {
                return true;
            }            
            return false;
        }
    }
}