using Backload.Bundles;
using System.Web;
using System.Web.Optimization;

namespace AmeCaseBookOrg
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"                        
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqgrid").Include(
                        "~/Scripts/jquery.jqGrid.min.js",
                        "~/Scripts/i18n/grid.locale-en.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"                   
                      ));

            bundles.Add(new StyleBundle("~/Content/amecss").Include(
                    "~/Content/base.css",
                    "~/Content/layer.css"));

            bundles.Add(new ScriptBundle("~/bundles/slidesjs").Include(
                    "~/Scripts/jquery.slides.min.js",
                    "~/Scripts/mouse.couver.js"));

            bundles.Add(new StyleBundle("~/Content/jqgridcss").Include(
                      "~/Content/jquery.jqGrid/ui.jqgrid.css",
                      "~/Content/themes/smoothness/jquery-ui-1.10.3.custom.min.css"));

            bundles.Add(new StyleBundle("~/Content/themes/start/jqgridcssstarttheme").Include(
                      "~/Content/jquery.jqGrid/ui.jqgrid.css",
                      "~/Content/themes/start/jquery-ui-1.11.4.custom.min.css"));

            bundles.Add(new StyleBundle("~/Content/jqgridcssredmontheme").Include(
                      "~/Content/jquery.jqGrid/ui.jqgrid.css",
                      "~/Content/themes/start/jquery-ui-1.11.4.custom.min.css"));

            bundles.Add(new StyleBundle("~/Content/themes/overcast/jqgridcssovercasttheme").Include(
                     "~/Content/jquery.jqGrid/ui.jqgrid.css",
                     "~/Content/themes/overcast/jquery-ui-1.11.4.custom.min.css"));

            bundles.Add(new StyleBundle("~/Content/themes/southstreet/jqgridcsssouthstreettheme").Include(
                    "~/Content/jquery.jqGrid/ui.jqgrid.css",
                    "~/Content/themes/southstreet/jquery-ui-1.11.4.custom.min.css"));

            bundles.Add(new StyleBundle("~/Content/themes/pepper/jqgridcsspeppertheme").Include(
                    "~/Content/jquery.jqGrid/ui.jqgrid.css",
                    "~/Content/themes/pepper/jquery-ui-1.11.4.custom.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
                        "~/Scripts/tinymce/tinymce.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/imageslider").Include(
                        "~/Scripts/jssor.js",
                        "~/Scripts/jssor.slider.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/imagespopup").Include(
                "~/Scripts/jquery.magnific-popup.js"
                ));
            bundles.Add(new StyleBundle("~/Content/imagespopup").Include(
                "~/Content/magnific-popup.css"
               ));
            BlueImpBundles.RegisterBundles(bundles);
           
        }
    }
}
