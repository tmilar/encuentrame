using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Encuentrame.Model.Supports.Interfaces;
using Encuentrame.Web.Helpers;

namespace Encuentrame.Web.Support
{
    public class TranslationService : ITranslationService
    {
        public string Translate(string key)
        {
            return TranslationsHelper.Get(key);
        }

        public string Translate<T>() where T : class
        {
            return TranslationsHelper.Get<T>();
        }
    }
}