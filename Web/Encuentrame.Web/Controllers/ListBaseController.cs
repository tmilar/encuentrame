using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model;
using Encuentrame.Web.Models.DataTable;
using System;

namespace Encuentrame.Web.Controllers
{
    public abstract class ListBaseController<TModel, TViewModel> : BaseController
            where TModel : class
            where TViewModel : class
    {
        private static Dictionary<string, Func<IGenericSeeker, SearchModel, FilterModel>> filters = new Dictionary<string, Func<IGenericSeeker, SearchModel, FilterModel>>();
        static ListBaseController()
        {
            filters.Add("SelectFilter", (seeker, searchModel) => new SelectFilterModel(seeker, searchModel));
            filters.Add("DateRangeFilter", (seeker, searchModel) => new DateRangeFilterModel(seeker, searchModel));
            filters.Add("EnumFilter", (seeker, searchModel) => new EnumFilterModel(seeker, searchModel));
            filters.Add("BooleanFilter", (seeker, searchModel) => new BooleanFilterModel(seeker, searchModel));
            filters.Add("RangeFilter", (seeker, searchModel) => new RangeFilterModel(seeker, searchModel));
            filters.Add("InputFilter", (seeker, searchModel) => new InputFilterModel(seeker, searchModel));
            filters.Add("IntFilter", (seeker, searchModel) => new IntFilterModel(seeker, searchModel));
        }

        [Inject]
        public virtual IBag<TModel> Bag { get; set; }

        [Inject]
        public virtual ISeekerFactory<TModel> SeekerFactory { get; set; }

        public virtual void ApplyDefaultFilters(IGenericSeeker<TModel> seeker)
        {

        }

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
                    ApplyDefaultFilters(seeker);
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
                if (dataTableModel.Length.HasValue)
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

        protected int Filter<TTableModel, TModelClass>(TTableModel dataTableModel, IGenericSeeker<TModelClass> seeker)
            where TTableModel : DataTableModel
            where TModelClass : class
        {
            foreach (var searchModel in dataTableModel.SearchData)
            {
                Filter<TTableModel, TModelClass>(seeker, searchModel);
            }

            var recordsFiltered = seeker.Count();

            if (!string.IsNullOrEmpty(dataTableModel.SortColumn))
            {
                var sortOrder = dataTableModel.SortOrder == "asc" ? SortOrder.Asc : SortOrder.Desc;
                var method = seeker.GetType().GetMethod(string.Format("OrderBy{0}", dataTableModel.SortColumn));
                method.Invoke(seeker, new object[] { sortOrder });
            }

            if (dataTableModel.Length.HasValue && dataTableModel.Length.Value > 0)
                seeker.Skip(dataTableModel.Start).Take(dataTableModel.Length.Value);
            return recordsFiltered;
        }

        private static void Filter<TTableModel, TModelClass>(IGenericSeeker seeker, SearchModel searchModel)
            where TTableModel : DataTableModel
            where TModelClass : class
        {
            filters[searchModel.FilterType](seeker, searchModel).Seek();
        }

        protected abstract TViewModel GetViewModelFrom(TModel item);

        protected virtual IList<TModel> GetItemList()
        {
            if (this.Bag != null)
                return this.Bag.ToList();
            return null;
        }

        protected JsonResult JsonList<T>(List<T> items, int draw, int? recordsTotal = null, int? recordsFiltered = null) where T : class
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