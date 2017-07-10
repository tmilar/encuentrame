using System.Web.Optimization;

namespace Encuentrame.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/underscore").Include(
                "~/Scripts/underscore.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/Scripts/app/cuit-validation.js",
                        "~/Scripts/app/greater-than-other-property-validation.js",
                        "~/Scripts/app/less-than-other-property-validation.js",
                        "~/Scripts/app/conditional-validation.js",
                        "~/Scripts/app/required-list-validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/moment").Include(
                "~/Scripts/moment.min.js",
                "~/Scripts/moment-with-locales.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/form").Include(
                        "~/Scripts/app/commons.js",
                        "~/Scripts/select2.min.js",
                        "~/Scripts/i18n/es.js",
                        "~/Scripts/jquery.textareaCounter.js",
                       "~/Scripts/jquery.mask.js",
                       "~/Scripts/bootstrap-datepicker.js",
                       "~/Scripts/bootstrap-datepicker.es.js",
                       "~/Scripts/bootstrap-datetimepicker.js",
                        "~/Scripts/app/form-validation-settings.js",
                        "~/Scripts/app/form.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/treeview").Include(
                    "~/Scripts/app/arativa-bootstrap-treeview.js",
                     "~/Scripts/app/treeview.js"));
           

            bundles.Add(new ScriptBundle("~/bundles/treant").Include(
                "~/Scripts/raphael.js",
                    "~/Scripts/Treant.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatable").Include(
                    "~/Scripts/DataTables-1.10.12/media/js/jquery.dataTables.min.js",
                    "~/Scripts/DataTables-1.10.12/media/js/dataTables.bootstrap.min.js",
                     "~/Scripts/app/datatable.js"));

            bundles.Add(new ScriptBundle("~/bundles/filedata").Include(
                      "~/Scripts/app/filedata.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/modals").Include(
                       "~/Scripts/app/modals.js"
                       ));
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/select2.min.css",
                "~/Content/select2-bootstrap.css",
                "~/Content/bootstrap-datepicker3.min.css",
                "~/Content/bootstrap-datetimepicker.min.css",
                "~/Content/arativa.bootstrap.treeview.css",
                "~/Content/DataTables-1.10.12/media/css/dataTables.bootstrap.min.css",
                "~/Content/datatable-site-custom.css",
                "~/Content/Treant.css",
                "~/Content/Treant-site-custom.css",
                "~/Content/Site.css"));
        }
    }
}
