using RAEI.ProgramacionDeProduccion.Model;
using RAEI.ProgramacionDeProduccion.Web.Models.DataTable;

namespace RAEI.ProgramacionDeProduccion.Web.Models.DataTable
{
    public class BooleanFilterModel: FilterModel
    {
        public BooleanFilterModel(IGenericSeeker seeker, SearchModel searchModel) : base(seeker, searchModel)
        {
        }

        public override void Seek()
        {
            bool booleanValue;
            if (bool.TryParse(Value, out booleanValue))
                this.InvokeImplSeek(booleanValue);
        }
    }
}