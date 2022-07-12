using System.Web;
using System.Web.Optimization;

namespace Helpdesk
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
           

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/index.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap-datetimepicker.js",
                 "~/Scripts/moment.min.js",
                  "~/Scripts/bootstrap-datetimepicker.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/css/main.css",
                      "~/Content/css/sidebar - themes.css",
                      "~/Content/css/site.css"));

            //bundles.Add(new StyleBundle("~/Content/sidebar").Include(
            //         "~/Content/scss/_layout.scss",
            //         "~/Content/scss/_menu.scss",
            //         "~/Content/scss/_sidebar.scss",
            //         "~/Content/scss/_variable.scss",
            //         "~/Content/scss/_styles.scss"));


            bundles.Add(new ScriptBundle("~/bundles/ckeditor").Include(
                "~/Scripts/ckeditor/ckeditor.js"
               ));

            bundles.Add(new ScriptBundle("~/bundles/datePicker").Include(
                 "~/Content/bootstrap-datetimepicker.js"
                ));
            bundles.Add(new StyleBundle("~/Content/datepicker").Include(
                 "~/Content/bootstrap-datetimepicker.min.css"
             ));
            
        }

    }
}
