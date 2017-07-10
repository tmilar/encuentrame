using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model;
using Encuentrame.Web.Models;


namespace Encuentrame.Web.Controllers
{
    public abstract class ListBaseController<TModel, TViewModel>:BaseController 
            where TModel:class 
            where TViewModel:class
    {
        [Inject]
        public virtual IBag<TModel> Bag { get; set; }

        [Inject]
        public virtual ISeekerFactory<TModel> SeekerFactory { get; set; }        

        [HttpPost]
        public JsonResult GetItems(DataTableModel dataTableModel, string _)
        {
            var total = 0;
            var recordsFiltered = 0;

            IList<TModel> list = null;
            if (dataTableModel.IsServerSide)
            {
                var seeker = SeekerFactory.Seek();
                if (seeker != null)
                {
                    total = seeker.Count();
                    recordsFiltered = Filter(dataTableModel, seeker);
                    
                    list = seeker.ToList();       
                }
            }
            else
            {
                list = GetItemList();
                total = list.Count();

                recordsFiltered = list.Count();
                if (dataTableModel.Length.HasValue && dataTableModel.Length.Value > 0)
                {
                    list = list.Skip(dataTableModel.Start).Take(dataTableModel.Length.Value).ToList();
                }
            }

            var listModels = new List<TViewModel>();
            
            foreach (var item in list)
            {
                listModels.Add(GetViewModelFrom(item));
            }

            return JsonList(listModels, dataTableModel.Draw, total, recordsFiltered);
        }

        protected int Filter<TTableModel, TModelClass>(TTableModel dataTableModel, IGenericSeeker<TModelClass> seeker) where TTableModel : DataTableModel
                                                                                                                where TModelClass:class
        {
            foreach (var searchModel in dataTableModel.SearchData)
            {
                MethodInfo method = seeker.GetType().GetMethod(string.Format("By{0}", searchModel.Column));
                method.Invoke(seeker, new object[] {searchModel.Value});
            }

            var recordsFiltered = seeker.Count();

            if (!string.IsNullOrEmpty(dataTableModel.SortColumn))
            {
                var sortOrder = dataTableModel.SortOrder == "asc" ? SortOrder.Asc : SortOrder.Desc;
                MethodInfo method = seeker.GetType().GetMethod(string.Format("OrderBy{0}", dataTableModel.SortColumn));
                method.Invoke(seeker, new object[] {sortOrder});
            }

            if (dataTableModel.Length.HasValue && dataTableModel.Length.Value>0)
            {
                seeker.Skip(dataTableModel.Start).Take(dataTableModel.Length.Value);
            }
            return recordsFiltered;
        }

        protected abstract TViewModel GetViewModelFrom(TModel item);

        protected virtual IList<TModel> GetItemList()
        {
            return null;
        }

        protected JsonResult JsonList<T>(List<T> items, int draw, int? recordsTotal = null, int? recordsFiltered = null) where T: class
        {
            var dataToReturn = new
            {
                draw = draw,
                recordsTotal = recordsTotal ?? items.Count,
                recordsFiltered = recordsFiltered ?? items.Count,
                data = items
            };
            return this.Json(dataToReturn, JsonRequestBehavior.AllowGet);
        }
    }
}