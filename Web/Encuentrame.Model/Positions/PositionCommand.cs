using System.Collections.Generic;
using System.Linq;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Supports;
using Encuentrame.Support;
using NailsFramework.IoC;
using NailsFramework.Persistence;

namespace Encuentrame.Model.Positions
{
    [Lemming]
    public class PositionCommand : BaseCommand, IPositionCommand
    {
        [Inject]
        public IBag<Position> Positions { get; set; }


        public class CreateOrEditParameters
        {
            public int UserId { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }

        }

        public Position Get(int id)
        {
            return Positions[id];
        }

        public void Create(CreateOrEditParameters positionParameters)
        {
            var position = new Position()
            {
                Latitude = positionParameters.Latitude,
                Longitude = positionParameters.Longitude,
                UserId = positionParameters.UserId,
                Creation = SystemDateTime.Now,
            };


            Positions.Put(position);

        }

        public void Edit(int id, CreateOrEditParameters positionParameters)
        {
            var position = Positions[id];
            position.Latitude = positionParameters.Latitude;
            position.Longitude = positionParameters.Longitude;
            position.UserId = positionParameters.UserId;
        }

        public void Delete(int id)
        {
            var position = Positions[id];

            Positions.Remove(position);

        }

        public IList<Position> List()
        {
            return Positions.OrderByDescending(x=>x.Id).ToList();
        }

    }
}
