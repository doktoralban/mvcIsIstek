using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;



namespace mvcIsIstek.Filters
{
	public class HataOlursaMailAt : ActionFilterAttribute, IExceptionFilter
	{
		/// <summary>
		/// Action Calismasi esnasinda bir Exception throw edilirse bu metod calisacak.
		/// </summary>
		/// <param name="filterContext"></param>
		public void OnException(ExceptionContext filterContext)
		{

			//Exception nesnesi'mu degilmi kontrol ediyoruz. 
			if (filterContext.Exception != null)
			{
				//Basitce Mail atacak olan kodlarimizi yaziyoruz.
				SmtpClient client = new System.Net.Mail.SmtpClient("smtp.yandex.com.tr", 587);
				NetworkCredential sifreBilgileri = new NetworkCredential("aydinturker1"
					, "xxxxxburaya_sifre_gelecekxxxx");
				client.EnableSsl = true;

				client.Credentials = sifreBilgileri;

				string Mesaj = filterContext.Exception.Message;
				MailMessage mail = new MailMessage(
						"aydinturker1@yandex.com.tr", //Kimden
						"aydinturker@msn.com", //Kime
						"test - mvc email gönderme sistemi"   , //Konu
						 System.DateTime.Now.ToString() + "... mesaj   :" + Mesaj //Mesaj icerigi
						);
				try
				{
					client.Send(mail);
 				}
				catch (Exception ex) 
				{
					var ss = ex.Message;
					return;
				}
				//Olusan hatanin maili gonderilmis oldu. 
				//Peki biz bu noktada hata mesaji goruntuleme isini nasil yapacagiz.

				//Isimsiz tipleri kullanarak bir anonymus tip yarattik.
				string ErrorModel = Mesaj;

				//Hatayi yakaladiğimizi belirtiyoruz
				filterContext.ExceptionHandled = true;

				//Çalıştırmış olduğumuz Action'a bir result dondurmemız gerek. 
				//Ben View Klasörü içine bir _Hata.cshtml dosyası yazdım ve View'ın 
				//oraya düşmesini istedim Data olarakta Hata Mesajı aktardım
				//filterContext.Result = new ViewResult
				//{
				//	ViewName = "~/Views/_Hata.cshtml",
				//	ViewData = new ViewDataDictionary(ErrorModel)
				//};
			}
		}



	}
}