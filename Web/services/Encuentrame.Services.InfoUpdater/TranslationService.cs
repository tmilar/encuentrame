using NailsFramework.IoC;
using Encuentrame.Model.Supports.Interfaces;

namespace Encuentrame.Services.InfoUpdater
{
    [Lemming]
    public class TranslationService : ITranslationService
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