using System;
using System.Collections.Generic;
using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NailsFramework.UnitOfWork;
using Encuentrame.Model.Supports.Audits;
using Encuentrame.Model.Supports.Interfaces;
using Encuentrame.Support;

namespace Encuentrame.Model.Supports
{
    public abstract class BaseCommand
    {
        [Inject]
        public ITranslationService TranslationService { get; set; }
        [Inject]
        public INHibernateContext NHibernateContext { get; set; }
        [Inject]
        public ICurrentUnitOfWork CurrentUnitOfWork { get; set; }
        [Inject]
        public IAuditContextManager AuditContextManager { get; set; }


        protected void RefreshObject(object obj)
        {
            NHibernateContext.CurrentSession.Refresh(obj);
        }

        /// <summary>
        /// Update list database (targetList) from list parameters (sourceList)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceList">Objects source</param>
        /// <param name="targetList">Objects model in database</param>
        /// <param name="search"></param>
        protected void EditListFromIds<T>(IEnumerable<int> sourceList, IList<T> targetList, Func<int, T> search) where T : IIdentifiable
        {

            var listToDelete = new List<T>();

            foreach (var itemTarget in targetList)
            {
                var targetCount = targetList.Where(x => x.Id == itemTarget.Id).Count();
                var sourceCount = sourceList.Where(x => x == itemTarget.Id).Count();
                var deleteCount = listToDelete.Where(x => x.Id == itemTarget.Id).Count();
                if ((sourceCount + deleteCount) < targetCount)
                {
                    listToDelete.Add(itemTarget);
                }
            }
            listToDelete.ForEach((x) => targetList.Remove(x));

            foreach (var id in sourceList)
            {
                var targetCount = targetList.Where(x => x.Id == id).Count();
                if (targetCount < sourceList.Where(x => x == id).Count())
                {
                    targetList.Add(search(id));
                }
            }
        }

      

        protected void EditList<S, T>(IEnumerable<S> sourceList, IList<T> targetList, Func<S, T> creator, Func<S, int> comparer, Action<S, T> updater, Action<T> onDelete = null) where T : IIdentifiable
        {
            var listToDelete = new List<T>();

            foreach (var itemTarget in targetList)
            {
                var itemSource = sourceList.Where(x => comparer(x) == itemTarget.Id).FirstOrDefault();
                if (itemSource != null)
                {
                    if(updater!=null)
                        updater(itemSource, itemTarget);
                }
                else
                {
                    listToDelete.Add(itemTarget);
                }
            }

            listToDelete.ForEach((x) =>
            {
                if (onDelete != null)
                    onDelete(x);
                targetList.Remove(x); });

            foreach (var itemSource in sourceList)
            {
                if (!targetList.Where(x =>  x.Id>0 && x.Id == comparer(itemSource)).Any())
                {
                    targetList.Add(creator(itemSource));
                }
            }
        }
    }
}