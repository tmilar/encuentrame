using NailsFramework.IoC;
using NailsFramework.Logging;

namespace NailsFramework.Tests.Logging.TestModel
{
    public class ClassWithInstanceLogger
    {
        [Inject]
        public ILog Log { get; set; }
    }
}