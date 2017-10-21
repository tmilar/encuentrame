using System.Collections.Generic;

namespace Encuentrame.Model.AreYouOks
{
    public interface IAreYouOkCommand
    {
        AreYouOkActivity Get(int id);
        IList<AreYouOkActivity> List();
        void Delete(int id);
        void Reply(AreYouOkCommand.ReplyParameters parameters);
        void Ask(AreYouOkCommand.AskParameters parameters);
    }
}