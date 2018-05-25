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

        void SoughtPersonSeen(AreYouOkCommand.SoughtPersonSeenParameters parameters);
        void SoughtPersonDismiss(AreYouOkCommand.SoughtPersonDismissParameters parameters);

        IList<PositionWhereWasSeenInfo> PositionsWhereWasSeen(int eventId, int userId);
        SeenInfo GetSeenInfo(int eventId, int userId);
    }
}