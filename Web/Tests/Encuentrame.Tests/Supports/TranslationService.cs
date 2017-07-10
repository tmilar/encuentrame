using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encuentrame.Model.Supports.Interfaces;

namespace Encuentrame.Tests.Supports
{
    public class TranslationService: ITranslationService
    {
        public string Translate(string key)
        {
            return key;
        }

        public string Translate<T>() where T : class
        {
            return typeof(T).Name;
        }
    }
}
