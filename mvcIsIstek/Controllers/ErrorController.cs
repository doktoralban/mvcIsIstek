using System;
using System.Web.Mvc;

namespace mvcIsIstek.Controllers
{
	public class ErrorController : Controller
    {
		// GET: Error
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult NotFound()
        {
					return View();
        }
		public ActionResult HataOlustu()
		{
			return View();
		}
	public ActionResult BadRequest()
		{
			return View();
		}
public ActionResult Error500()
		{
			return View();
		}
		public ActionResult HataOlustur()
		{
			throw new Exception("Bu Hata Manuel Olarak email testi için  Olusturuldu");

		}




	}
}