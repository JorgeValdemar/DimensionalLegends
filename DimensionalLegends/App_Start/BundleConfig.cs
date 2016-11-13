using System.Web;
using System.Web.Optimization;

namespace DimensionalLegends
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                            "~/Scripts/Plugins/bootstrap.js",
                            "~/Scripts/Plugins/enscroll.js",
                            "~/Scripts/Plugins/parsley.js",
                            "~/Scripts/Plugins/pt-br.js",
                            "~/Scripts/Plugins/jquery-css-transform.js",
                            "~/Scripts/Plugins/rotate3Di.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/babylon").Include(
                            "~/Scripts/Babylon/cannon.js",
                            "~/Scripts/Babylon/oimo.js",
                            "~/Scripts/Babylon/babylon.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/geral").Include(
                            "~/Scripts/Game/main.js",
                            "~/Scripts/Utils/modal.js",
                            "~/Scripts/Game/login.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/Plugins/angular.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/plugins/bootstrap.css",
                "~/Content/plugins/jquery-ui.css",
                "~/Content/site.css"
            ));

            bundles.Add(new StyleBundle("~/Content/csssite").Include(
                "~/Content/plugins/bootstrap.css",
                "~/Content/plugins/jquery-ui.css",
                "~/Content/site-main.css"
            ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}