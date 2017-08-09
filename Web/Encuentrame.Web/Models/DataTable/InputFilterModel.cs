using System;
using Encuentrame.Model;

namespace Encuentrame.Web.Models.DataTable
{
    public class InputFilterModel: FilterModel
    {
        public InputFilterModel(IGenericSeeker seeker, SearchModel searchModel) : base(seeker, searchModel)
        {
        }

        public override void Seek()
        {
            this.InvokeImplSeek(Value);
        }
    }
}