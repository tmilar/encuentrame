using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class PropertyBasedFilter<T, TProperty> : BaseFilter<T> where T : class
                                                                                  where TProperty: class                                                                          
    {      
        public Expression<Func<T, TProperty>> MemberExpression { get; set; }

        //public void Build(ColumnData columnData, HtmlHelper helper)
        //{
        //    var editor = helper.EditorForModel(MemberExpression);
        //    helper.ViewContext.Writer.Write(editor);
        //}

        public override void Build(ColumnData columnData, HtmlHelper helper)
        {
            var editor = helper.EditorForModel(MemberExpression);
            //helper.ViewContext.Writer.Write(editor);
            this.Build(editor.ToString(), "proeprty-filter", helper);
        }
    }    
}