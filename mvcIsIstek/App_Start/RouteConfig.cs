using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace mvcIsIstek
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			routes.MapMvcAttributeRoutes();

			routes.MapRoute("Makinalar/DetayMakinaKodu", "Makinalar/DetayMakinaKodu/{mKod}"
		, new { Controller = "Makinalar", Action = "DetayMakinaKodu", mKod = UrlParameter.Optional }
			);

			routes.MapRoute("Makinalar/LokasyonaGoreMakinalar", "Makinalar/LokasyonaGoreMakinalar/{sLokasyonTanimi}"
				, new { Controller = "Makinalar", Action = "LokasyonaGoreMakinalar", sLokasyonTanimi = UrlParameter.Optional }
					);

			routes.MapRoute("Makinalar/Sayfala", "Makinalar/Sayfala/{SayfaNo}"
				,new {Controller="Makinalar",Action="Sayfala",SayfaNo=UrlParameter.Optional}
			);

			routes.MapRoute(
					name: "Default",
					url: "{controller}/{action}/{id}",
					defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

		



		}
	}
}
