using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Businesses;
using Encuentrame.Model.Supports.Audits;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers
{
    public static class ListItemsHelper
    {
        #region IBags
       
        [Inject]
        public static IBag<User> Users { get; set; }
        [Inject]
        public static IBag<Business> Businesses { get; set; }


        #endregion

        #region Constructor

        static ListItemsHelper()
        {
            var iAuditableType = typeof(IAuditable);
            var iIdentifiableTypes = typeof(Audit).Assembly.GetTypes().Where(x => iAuditableType.IsAssignableFrom(x) && x != iAuditableType);
            Entities = new List<ReferenceItem>();
            foreach (var identifiableType in iIdentifiableTypes)
            {
                var entityTypeModel = new ReferenceItem() { id = identifiableType.Name, text = TranslationsHelper.Get(identifiableType.Name) };
                Entities.Add(entityTypeModel);
            }
        }

        #endregion

        #region SelectListItems
       
        public static ICollection<SelectListItem> GetUsers(int selectedId)
        {
            return Users.Where(x => x.DeletedKey == null).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.ToDisplay(),
                Selected = selectedId == x.Id,
            }).ToArray();
        }
        public static ICollection<SelectListItem> GetEventAdministratorUsers(int selectedId)
        {
            return Users.Where(x=>x.DeletedKey==null && x.Role==RoleEnum.EventAdministrator).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.ToDisplay(),
                Selected = selectedId == x.Id,
            }).ToArray();
        }

        public static ICollection<SelectListItem> GetBusinesses(int selectedId)
        {
            return Businesses.Where(x => x.DeletedKey == null ).Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.ToDisplay(),
                Selected = selectedId == x.Id,
            }).ToArray();
        }

        #endregion

        #region ReferenceItems
        public static List<ReferenceItem> Entities { get; set; }
        
        public static IList<ReferenceItem> GetUsersList()
        {
            return Users.Where(x => x.DeletedKey == null).Select(x => new ReferenceItem
            {
                id = x.Id.ToString(),
                text = x.ToDisplay(),

            }).ToArray();
        }

        public static IList<ReferenceItem> GetEventAdministratorUsersList()
        {
            return Users.Where(x => x.DeletedKey == null && x.Role == RoleEnum.EventAdministrator).Select(x => new ReferenceItem
            {
                id = x.Id.ToString(),
                text = x.ToDisplay(),
                
            }).ToArray();
        }
        public static IList<ReferenceItem> GetBusinessesList()
        {
            return Businesses.Where(x => x.DeletedKey == null).Select(x => new ReferenceItem
            {
                id = x.Id.ToString(),
                text = x.ToDisplay(),

            }).ToArray();
        }

        public static IList<SelectListItem> GetUserSelectListItems()
        {
            return DeleteableToSelectListItems(Users, x => x.ToDisplay());
        }

        public static List<ReferenceItem> GetEntitiesList()
        {
            return Entities;
        }

     

        public static IList<ReferenceItem> DeleteableToReferenceItems<T>(IBag<T> list, Func<T, string> toText) where T : class, IIdentifiable, IDeleteable
        {
            return ToReferenceItems(list.Where(x => x.DeletedKey == null), toText);
        }

        public static IList<ReferenceItem> ToReferenceItems<T>(IQueryable<T> iQueryable, Func<T, string> toText) where T : class, IIdentifiable
        {
            var modelItems = iQueryable.ToList();
            var items = new List<ReferenceItem>();
            foreach (var listItem in modelItems)
            {
                var item = new ReferenceItem() { id = listItem.Id.ToString(), text = toText(listItem) };
                items.Add(item);
            }

            return items;
        }

        public static IList<SelectListItem> DeleteableToSelectListItems<T>(IBag<T> list, Func<T, string> toText) where T : class, IIdentifiable, IDeleteable
        {
            return ToSelectListItems(list.Where(x => x.DeletedKey == null), toText);
        }
        public static IList<SelectListItem> DeleteableToSelectListItems<T>(IBag<T> list, Func<T, string> toText, IEnumerable<int> selectedIds) where T : class, IIdentifiable, IDeleteable
        {
            return ToSelectListItems(list.Where(x => x.DeletedKey == null), toText, selectedIds);
        }
        public static IList<SelectListItem> DeleteableToSelectListItems<T>(IQueryable<T> list, Func<T, string> toText) where T : class, IIdentifiable, IDeleteable
        {
            return ToSelectListItems(list.Where(x => x.DeletedKey == null), toText);
        }

        public static IList<SelectListItem> ToSelectListItems<T>(IQueryable<T> iQueryable, Func<T, string> toText) where T : class, IIdentifiable
        {
            return ToSelectListItems(iQueryable, toText,new List<int>());
        }

        public static IList<SelectListItem> ToSelectListItems<T>(IQueryable<T> iQueryable, Func<T, string> toText, IEnumerable<int> selectedIds) where T : class, IIdentifiable
        {
            var modelItems = iQueryable.ToList();
            var items = new List<SelectListItem>();
            if (selectedIds == null)
            {
                selectedIds = new List<int>();
            }
            foreach (var listItem in modelItems)
            {
                var item = new SelectListItem() { Value = listItem.Id.ToString(), Text = toText(listItem),Selected = selectedIds.Any(x=>x==listItem.Id)};
                items.Add(item);
            }

            return items;
        }

        #endregion

        public class ReferenceItem
        {
            public string id { get; set; }

            public string text { get; set; }

            public string group { get; set; }
        }

      
    }
}