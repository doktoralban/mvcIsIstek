using System.Web;
using System.Web.Optimization;

namespace mvcIsIstek
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
									"~/Scripts/jquery-{version}.js",
									"~/Scripts/moment.js",
									"~/Scripts/bootstrap-sortable.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
									"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
									"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
								"~/Scripts/bootstrap.js",
								"~/Scripts/respond.js"
								,"~/Scripts/bootstrap-datetimepicker.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
								"~/Content/bootstrap.css",
								"~/Content/site.css",
								"~/Content/bootstrap-sortable.css"));
		

		//.....................................................
		bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui-{version}.js",
            "~/Scripts/jquery-ui.unobtrusive-{version}.js"));


			bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
				"~/Content/themes/base/core.css",
				"~/Content/themes/base/datepicker.css",
				"~/Content/themes/base/theme.css"));






		}

	}
}
