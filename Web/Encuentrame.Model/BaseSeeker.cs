using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NHibernate.Cache.Entry;
using NHibernate.SqlCommand;
using Encuentrame.Support;

namespace Encuentrame.Model
{
    public enum SortOrder
    {
        Asc,
        Desc
    }

    public abstract class BaseSeeker
    {
        
    }

    [Lemming(Singleton = false)]
    public abstract class BaseGenericSeeker<TModel> : BaseSeeker, IGenericSeeker<TModel> where TModel : class
    {
        public abstract IList<TModel> ToList();
        public abstract int Count();
        public abstract IGenericSeeker<TModel> Skip(int start);
        public abstract IGenericSeeker<TModel> Take(int length);

        #region Filters
        public void FilterEnum<TEnum>(string value, Action<TEnum> byOne, Action<IList<TEnum>> byMany) where TEnum : struct, IConvertible
        {
            if (typeof(TEnum).IsEnum && !string.IsNullOrWhiteSpace(value))
            {
                int intValue;

                if (int.TryParse(value, out intValue))
                {
                    if (Enum.IsDefined(typeof(TEnum), intValue))
                    {
                        var status = EnumConverter<TEnum>.Convert(intValue);
                        byOne(status);
                    }

                }
                else
                {
                    var stringValues = value.Split('|');
                    if (stringValues.Length > 0)
                    {
                        var enumValues = new List<TEnum>();
                        foreach (var stringValue in stringValues)
                        {
                            if (int.TryParse(stringValue, out intValue))
                            {
                                if (Enum.IsDefined(typeof(TEnum), intValue))
                                {
                                    var status = EnumConverter<TEnum>.Convert(intValue);
                                    enumValues.Add(status);
                                }
                            }
                        }

                        if (enumValues.Count > 0)
                        {
                            byMany(enumValues);
                        }
                    }
                }
            }
        }

        protected virtual void FilterReference(string value, Action<int> byOne, Action<IList<int>> byMany)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var intValue = 0;
                if (Int32.TryParse(value, out intValue))
                {
                    byOne(intValue);
                }
                else
                {
                    var values = value.Split('|');
                    var intValues = new List<int>();
                    foreach (var valueToConvert in values)
                    {
                        if (Int32.TryParse(valueToConvert, out intValue))
                        {
                            intValues.Add(intValue);
                        }
                    }
                    if (intValues.Count > 0)
                    {
                        byMany(intValues);
                    }
                }
            }
        }

        protected virtual void FilterReference(string value, Action<string> byOne, Action<IList<string>> byMany)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var values = value.Split('|');
                if (values.Length > 1)
                {
                    byMany(values.ToList());
                }
                else
                {
                    byOne(value);
                }               
            }
        }

        protected virtual void FilterReference(string value, Action<int> byOne)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var intValue = 0;
                if (Int32.TryParse(value, out intValue))
                {
                    byOne(intValue);
                }
            }
        }

        protected virtual void FilterBoolean(string value, Action<bool> byBoolean)
        {
            var booleanValue = false;
            if (Boolean.TryParse(value, out booleanValue))
            {
                byBoolean(booleanValue);
            }            
        }

        protected virtual void FilterDecimalRange(string value, Action<decimal, decimal> inRange, Action<decimal> lessOrEqual, Action<decimal> greaterOrEqual)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var rangeValues = value.Split('|');
                if (rangeValues.Any())
                {
                    var nanValue = "NaN";
                    var minStringValue = rangeValues[0];
                    var maxStringValue = rangeValues[1];

                    decimal minValue = 0;
                    decimal maxValue = 0;
                    var isMinValid = decimal.TryParse(minStringValue, out minValue);
                    var isMaxValid = decimal.TryParse(maxStringValue, out maxValue);

                    if (isMinValid || isMaxValid)
                    {
                        if (isMaxValid && isMinValid)
                            inRange(minValue, maxValue);
                        else if (isMaxValid)
                        {
                            lessOrEqual(maxValue);
                        }
                        else
                        {
                            greaterOrEqual(minValue);
                        }
                    }
                }
            }
        }

        protected virtual void FilterDateRange(string value, Action<DateTime, DateTime> inRange, Action<DateTime> lessOrEqual, Action<DateTime> greaterOrEqual)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var rangeValues = value.Split('|');
                if (rangeValues.Any())
                {
                    var minStringValue = rangeValues[0];
                    var maxStringValue = rangeValues[1];

                    DateTime minValue;
                    DateTime maxValue;
                    var isMinValid = DateTime.TryParseExact(minStringValue, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out minValue);
                    var isMaxValid = DateTime.TryParseExact(maxStringValue, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out maxValue);

                    if (isMinValid || isMaxValid)
                    {
                        if (isMaxValid && isMinValid)
                            inRange(minValue, maxValue);
                        else if (isMaxValid)
                        {
                            lessOrEqual(maxValue);
                        }
                        else
                        {
                            greaterOrEqual(minValue);
                        }
                    }
                }
            }
        }

        protected virtual void FilterIntRange(string value, Action<int, int> inRange, Action<int> lessOrEqual, Action<int> greaterOrEqual)
        {
            if (!string.IsNullOrEmpty(value))
            {
                var rangeValues = value.Split('|');
                if (rangeValues.Any())
                {
                    var nanValue = "NaN";
                    var minStringValue = rangeValues[0];
                    var maxStringValue = rangeValues[1];

                    int minValue = 0;
                    int maxValue = 0;
                    var isMinValid = int.TryParse(minStringValue, out minValue);
                    var isMaxValid = int.TryParse(maxStringValue, out maxValue);

                    if (isMinValid || isMaxValid)
                    {
                        if (isMaxValid && isMinValid)
                            inRange(minValue, maxValue);
                        else if (isMaxValid)
                        {
                            lessOrEqual(maxValue);
                        }
                        else
                        {
                            greaterOrEqual(minValue);
                        }
                    }
                }
            }
        }
        #endregion
    }

    [Lemming(Singleton=false)]
    public abstract class BaseSeeker<TModel> : BaseGenericSeeker<TModel>, ISeeker<TModel> where TModel : class, IIdentifiable
    {
        private IBag<TModel> myBag;
        public IQueryable<TModel> CurrentIQueryable { get; protected set; }

        [Inject]
        public IBag<TModel> Bag
        {
            get { return this.myBag; }
            set
            {
                this.myBag = value;
                this.CurrentIQueryable = value;
                ApplyDefaultFilters();
            }
        }

        public BaseSeeker(IBag<TModel> bag)
        {
            this.Initialize(bag);
        }

        protected BaseSeeker()
        { }

        protected void Initialize(IBag<TModel> bag)
        {
            this.Bag = bag;
            this.CurrentIQueryable = bag;
            ApplyDefaultFilters();
        }

        protected virtual void ApplyDefaultFilters()
        {          
        }

        public override IList<TModel> ToList()
        {
            return CurrentIQueryable.ToList();
        }

        public override int Count()
        {
            return CurrentIQueryable.Count();
        }

        public override IGenericSeeker<TModel> Skip(int start)
        {
            CurrentIQueryable = CurrentIQueryable.Skip(start);
            return this;
        }

        public override IGenericSeeker<TModel> Take(int length)
        {
            CurrentIQueryable = CurrentIQueryable.Take(length);
            return this;
        }        

        public virtual ISeeker<TModel> OrderById(SortOrder sortOrder)
        {
            OrderBy(x => x.Id, sortOrder);
            return this;
        }

        public virtual ISeeker<TModel> ById(string value)
        {
            FilterReference(value, (id)=> Where(x=>x.Id == id));
            return this;
        }

#region Query

        public virtual IQueryable<TModel> OrderBy<TKey>(Expression<Func<TModel, TKey>> keySelector, SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Asc)
                this.CurrentIQueryable = this.CurrentIQueryable.OrderBy(keySelector);
            else
                this.CurrentIQueryable = this.CurrentIQueryable.OrderByDescending(keySelector);
                       
            return CurrentIQueryable;            
        }

        public virtual IQueryable<TModel> Where(Expression<Func<TModel, bool>> predicate)
        {
            CurrentIQueryable = CurrentIQueryable.Where(predicate);
            return CurrentIQueryable;
        }

        #endregion

    }

    [Lemming]
    public class NullSeeker<TModel> : IGenericSeeker<TModel> where TModel : class
    {
        public IList<TModel> ToList()
        {
            return null;
        }

        public int Count()
        {
            return 0;
        }

        public IGenericSeeker<TModel> Skip(int start)
        {
            return null;
        }

        public IGenericSeeker<TModel> Take(int length)
        {
            return null;
        }
    }
}
