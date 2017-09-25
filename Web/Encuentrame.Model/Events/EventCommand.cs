using System;
using System.Collections.Generic;
using System.Linq;
using Encuentrame.Model.Addresses;
using Encuentrame.Model.Supports;
using Encuentrame.Support;
using NailsFramework.IoC;
using NailsFramework.Persistence;

namespace Encuentrame.Model.Events
{
    [Lemming]
    public class EventCommand : BaseCommand, IEventCommand
    {
        [Inject]
        public IBag<Event> Events { get; set; }

        public Event Get(int id)
        {
            return Events[id];
        }
        public IList<Event> List()
        {
            return Events.Where(x => x.DeletedKey == null).ToList();
        }
        public void Create(CreateOrEditParameters eventParameters)
        {
            var eventt = new Event();
            UpdateWith(eventt, eventParameters);

            Events.Put(eventt);
        }
        public void Edit(int id, CreateOrEditParameters eventParameters)
        {
            var eventt = Events[id];
            UpdateWith(eventt, eventParameters);

            Events.Put(eventt);
        }
        private void UpdateWith(Event eventt, CreateOrEditParameters eventParameters)
        {
            eventt.Name = eventParameters.Name;
            eventt.Latitude = eventParameters.Latitude;
            eventt.Longitude = eventParameters.Longitude;
            eventt.BeginDateTime = eventParameters.BeginDateTime;
            eventt.EndDateTime = eventParameters.EndDateTime;
            eventt.Address=new Address()
            {
                City = eventParameters.City,
                FloorAndDepartament = eventParameters.FloorAndDepartament,
                Number = eventParameters.Number,
                Province = eventParameters.Province,
                Street = eventParameters.Street,
                Zip = eventParameters.Zip
            };
        }
        public void Delete(int id)
        {
            var eventt=Events[id];
            eventt.DeletedKey = SystemDateTime.Now;
        }

        public class CreateOrEditParameters
        {
            public string Name { get; set; }
            public string City { get; set; }
            public string Province { get; set; }
            public string Zip { get; set; }
            public string Street { get; set; }
            public string Number { get; set; }
            public string FloorAndDepartament { get; set; }
            public decimal Latitude { get; set; }
            public decimal Longitude { get; set; }
            public DateTime BeginDateTime { get; set; }
            public DateTime EndDateTime { get; set; }
        }
    }
}