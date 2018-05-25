using System.Collections.Generic;
using Encuentrame.Model.Accounts;

namespace Encuentrame.Model.Positions
{
    public interface IPositionCommand
    {
        Position Get(int id);
        void Create(PositionCommand.CreateOrEditParameters positionParameters);
        IList<Position> List();
        void Edit(int id, PositionCommand.CreateOrEditParameters positionParameters);
        void Delete(int id);
    }
}