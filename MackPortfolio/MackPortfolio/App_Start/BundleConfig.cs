using System.Web;
using System.Web.Optimization;

namespace MackPortfolio
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            BundleTable.EnableOptimizations = false;

            bundles.Add(new ScriptBundle("~/libs/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            var legacyjq = new ScriptBundle("~/libs/jqueryLegacyIE").Include(
                        "~/Scripts/jquery-{version}.js");
            legacyjq.CdnPath = "//code.jquery.com/jquery-1.11.1.min.js";
            legacyjq.CdnFallbackExpression = "window.jQuery";
            bundles.Add(legacyjq);

            bundles.Add(new ScriptBundle("~/libs/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/libs/respond").Include(
                        "~/Scripts/respond.js",
                        "~/Scripts/respond.matchmedia.addlistener.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            var modernizr = new ScriptBundle("~/libs/modernizr").Include("~/Scripts/modernizr-*");
            modernizr.CdnPath = "//cdnjs.cloudflare.com/ajax/libs/modernizr/2.8.3/modernizr.min.js";
            modernizr.CdnFallbackExpression = "window.Modernizr";
            bundles.Add(modernizr);

            var knockout = new ScriptBundle("~/libs/knockout").Include("~/Scripts/knockout-{version}.js");
            knockout.CdnPath = "//cdnjs.cloudflare.com/ajax/libs/knockout/3.2.0/knockout-min.js";
            knockout.CdnFallbackExpression = "window.ko";
            bundles.Add(knockout);

            bundles.Add(new ScriptBundle("~/libs/ko").IncludeDirectory("~/Scripts/ko","*.js"));
            bundles.Add(new ScriptBundle("~/libs/site").IncludeDirectory("~/Scripts/site", "*.js", false));

            var kendoVer = "2014.2.716";
            bundles.Add(new ScriptBundle("~/libs/kendo")
                .Include("~/Scripts/kendo/" + kendoVer + "/kendo.all.min.js")
                .Include("~/Scripts/kendo/" + kendoVer + "/kendo.aspnetmvc.min.js")
                .Include("~/Scripts/kendo/" + kendoVer + "/kendo.combobox.min.js")
                .Include("~/Scripts/kendo/kendo.modernizer.custom.js")
            );

            bundles.Add(new StyleBundle("~/Content/kendo/" + kendoVer + "/css")
                .Include("~/Content/kendo/" + kendoVer + "/kendo.common.min.css")
                .Include("~/Content/kendo/" + kendoVer + "/kendo.dataviz.min.css")
                .Include("~/Content/kendo/" + kendoVer + "/kendo.metro.min.css")
                .Include("~/Content/kendo/" + kendoVer + "/kendo.dataviz.metro.min.css")
            );


            bundles.Add(new StyleBundle("~/Content/css").Include(
           "~/Content/site.css"));

            #region Foundation Bundles

            bundles.Add(Foundation.Scripts());
            #endregion
        }
    }
}
