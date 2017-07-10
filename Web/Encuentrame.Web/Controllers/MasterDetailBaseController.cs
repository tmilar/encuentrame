using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model;
using Encuentrame.Web.Models;

namespace Encuentrame.Web.Controllers
{
    public abstract class MasterDetailBaseController<TModel, TViewModel, TChildModel, TChildViewModel> : ListBaseController<TModel, TViewModel>
        where TModel : class
        where TViewModel : class
        where TChildModel : class
        where TChildViewModel : class
    {
        [Inject]
        public virtual IBag<TChildModel> ChildrenBag { get; set; }

        [Inject]
        public virtual ISeekerFactory<TChildModel> ChildrenSeekerFactory { get; set; }        

        [HttpPost]
        public JsonResult GetChildrenItems(ChildDataTableModel dataTableModel, string _)
        {
            var total = 0;
            var recordsFiltered = 0;

            IList<TChildModel> list = null;
            if (dataTableModel.IsServerSide)
            {
                var seeker = this.ChildrenSeekerFactory.Seek();
                if (seeker != null)
                {
                    var method = seeker.GetType().GetMethod("ByParentId");
                    method.Invoke(seeker, new object[] { dataTableModel.ParentId });

                    total = seeker.Count();

                    recordsFiltered = this.Filter(dataTableModel, seeker);
                    list = seeker.ToList();
                }
            }
            else
            {
                list = this.GetChildItemList();
                total = list.Count();

                recordsFiltered = list.Count();
                if (dataTableModel.Length.HasValue)
                {
                    list = list.Skip(dataTableModel.Start).Take(dataTableModel.Length.Value).ToList();
                }
            }

            var listModels = new List<TChildViewModel>();

            foreach (var item in list)
            {
                listModels.Add(this.GetChildViewModelFrom(item));
            }

            return this.JsonList(listModels, dataTableModel.Draw, total, recordsFiltered);
        }

        protected abstract TChildViewModel GetChildViewModelFrom(TChildModel item);

        protected virtual IList<TChildModel> GetChildItemList()
        {
            return null;
        }       
    }
}