using System.Web;
using System.Web.Optimization;

namespace QuanLyBanHang
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jscore").Include(
                        "~/Assets/client/js/jquery-3.4.1.min.js",
                        "~/Assets/client/js/jquery-ui.js",
                        "~/Assets/client/js/bootstrap-3.1.1.min.js",
                        "~/Assets/client/js/move-top.js",
                        "~/Assets/client/js/easing.js",
                        "~/Assets/client/js/startstop-slider.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jscontroller").Include(
                "~/Assets/client/js/ControllerJS/baseController.js"
                ));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.validate.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            //// Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap-dialog.min.css"));

            bundles.Add(new StyleBundle("~/bundles/core").Include(
                      "~/Assets/client/css/bootstrap.min.css",
                      "~/Assets/client/css/bootstrap-theme.min.css",
                      "~/Assets/client/css/jquery-ui.css",
                      "~/Assets/client/css/style.css",
                      "~/Assets/client/css/slider.css",
                      "~/Assets/client/css/font-awesome.min.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
