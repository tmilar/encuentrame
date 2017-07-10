using System;
using System.Resources;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers
{
    public static class TranslationsHelper
    {
        private static readonly ResourceManager ResourceManager = new ResourceManager(typeof (Translations));

        private static string GetResource(string key)
        {
            if (key == " ")
            {
                return " ";
            }

            var traslate=ResourceManager.GetString(key);
            if (traslate.IsNullOrEmpty())
            {
                traslate = "key:{0}".FormatWith(key);
            }
            return traslate;
        }
        public static string Get(string name)
        {
            return GetResource(name);
        }
        public static string Get<T>() where T: class
        {
            return GetResource(typeof(T).Name);
        }
         public static string Get(string prefix,string name)
         {
             var key = "{0}{1}".FormatWith(prefix, name);
             return GetResource(key);
        }

        /// <summary>
        /// Las key de diccionario para los tipos enumerados se guardan como type+name con camelcase, por ejemplo PowerUnitEnumTons.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Get(Enum value)
        {
            string typeName = value.GetType().Name;
            var itemName = Enum.GetName(value.GetType() ,value);
            return Get(typeName, itemName);
        }


        
    
    }
}