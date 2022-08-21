using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace mvcIsIstek.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			if (Session["sesSecilenTema"] != null)
			{
				var ss = Session["sesSecilenTema"];
			}

			//..Cookie kontrolü................................
			HttpCookie Cookie = Request.Cookies["OrnekCookieLogin"];
			ViewBag.ckUname = Cookie?["ckUname"].ToString();
			Session["ckUname"] = Cookie?["ckUname"];
			//...................................................

			ViewBag.SayfaDescription = "Sayfa açıklaması 1111111111111";
			ViewBag.SayfaKeywords = "Sayfa keywords 11,22,33,44";
			ViewBag.SayfaTitle = "Sayfa Başlığı 8798545465";



			//...................................................
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			//Membership.ValidateUser("a.turker", "123");

			return View();
		}


		//[Authorize]
		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		 	public ActionResult css()
		{
		
			return View();
		}

		[HttpPost]
		public ActionResult TemaSecildi(string sTema)
		{
			var ss = Json(new { ResultValue = " ->Tamam-controllerdan gönderilen mesaj..<-)"});

			Session["sesSecilenTema"] = sTema;

			return ss ;
		}



	}
}