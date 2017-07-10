using System.Collections.Generic;
using System.IO;

namespace NailsFramework.Tests.Logging.Log4net
{
    class Log4netFileLogHelper
    {
        public string FileName { get; private set; }
        public Log4netFileLogHelper(string fileName)
        {
            FileName = fileName;
        }

        public IEnumerable<string> GetLines()
        {
            return File.ReadLines(FileName);
        }

        internal void DeleteFile()
        {
            if (File.Exists(FileName))
                File.Delete(FileName);
        }
    }
}
