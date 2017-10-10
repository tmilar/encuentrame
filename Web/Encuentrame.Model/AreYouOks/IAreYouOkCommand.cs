using System.Collections.Generic;

namespace Encuentrame.Model.AreYouOks
{
    public interface IAreYouOkCommand
    {
        AreYouOk Get(int id);
        IList<AreYouOk> List();
        void Delete(int id);
        void Reply(int id, AreYouOkCommand.ReplyParameters parameters);
        void Ask(AreYouOkCommand.AskParameters parameters);
    }
}