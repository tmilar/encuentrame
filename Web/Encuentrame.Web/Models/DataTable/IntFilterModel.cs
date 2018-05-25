using Encuentrame.Model;

namespace Encuentrame.Web.Models.DataTable
{
    public class IntFilterModel : FilterModel
    {
        public IntFilterModel(IGenericSeeker seeker, SearchModel searchModel)
            : base(seeker, searchModel)
        {
        }

        public override void Seek()
        {
            int intValue;
            if (int.TryParse(Value, out intValue))
                this.InvokeImplSeek(intValue);
        }
    }
}