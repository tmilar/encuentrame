using System;
using Encuentrame.Model.Accounts;
using Encuentrame.Support;

namespace Encuentrame.Model.SoughtPersons
{
    public class SoughtPersonAnswer:IIdentifiable
    {
        public virtual int Id { get; protected set; }
        public virtual DateTime? When { get; set; }
        public virtual decimal? Latitude { get; set; }
        public virtual decimal? Longitude { get; set; }
        public virtual User SourceUser { get; set; }
        public virtual User TargetUser { get; set; }
        public virtual bool Seen { get; set; }
        public virtual bool? IsOk { get; set; }
        

    }
}