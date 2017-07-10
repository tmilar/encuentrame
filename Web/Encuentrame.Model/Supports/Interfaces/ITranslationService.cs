using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encuentrame.Model.Supports.Interfaces
{
    public interface ITranslationService

    {
        string Translate(string key);
        string Translate<T>() where T : class;
    }
}
