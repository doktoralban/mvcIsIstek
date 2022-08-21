using mvcIsIstek.App_Data;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace mvcIsIstek.Controllers
{
	public class KullanicilarController : Controller
	{
		private dbOnemliIstekMakinaBakimEntities db = new dbOnemliIstekMakinaBakimEntities();

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}
		//***********************************************************

		[Authorize]
		[HttpGet]
		public ActionResult ChangePass()
		{
			return View();
		}

		[Authorize]
		[HttpPost]
		public ActionResult ChangePass(string sSifre
			, string sYeniSifre)
		{
			//adımlar : 1-şimdiki şifre doğrumu
			//2-şimdiki ve yeni şifre farklı olmalı
			//üstteki kontroller client tarafta yapılabilir. ama buradada kontrol edilmeli.

			if (string.IsNullOrEmpty(sYeniSifre))
			{
				ModelState.AddModelError(string.Empty, " Yeni şifre HATALI!!!...");

				return View();
			}

			if (sSifre == sYeniSifre)
			{
				ModelState.AddModelError(string.Empty, "Şimdiki ve Yeni şifre AYNI olmamalıdır!!!...");

				return View();
			}
			//.............................................
			string sKullaniciAdi = ClsKullanici.pr_KullanıcıAdı;
			var usr = from u in db.tbKullanicilar
								where u.KullanıcıAdı == sKullaniciAdi &&
								u.Şifresi == sSifre && u.Aktif == true
								select u;

			if (usr.Count() < 1)
			{
				ModelState.AddModelError(string.Empty, "Bu kullanıcı aktif değil veya şifre hatalı...");

				return View();
			}
			else if (usr.Count() == 1)
			{
				var degisti = false;
				usr.FirstOrDefault().Şifresi = sYeniSifre;

				db.SaveChanges();

				degisti = true;
				//............ 
				if (degisti == false)
				{
					ModelState.AddModelError(string.Empty, "Şifre değiştirilirken problem!!!...");

					return View();
				}
				else if (degisti == true)
				{
					//....aşağısı şifre değiştirildikden sonra.........................

					//ViewBag.IndexSayfasiBaslangicMesajiHtml = "Şifreniz Değiştirildi.";//Alert msgbox olarak değil, bootstrap alert olarak.
					return RedirectToAction("Index", "Home");
				}
				//.............................
			}

			return View();
		}

		// GET: Kullanicilar/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Kullanicilar/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "KullaniciID,KullanıcıAdı,Şifresi,AdıSoyadı,Grubu,Tipi,Adminmi,Ünvanı,LokasyonID,FaaliyetAlaniID,Emaili,CepTelefonu,DahiliTelefonu,Fotografi,UstAmiriID,SuanSistemde,DahiliMesajlaşmadaGörünür,BakımOnarımYapabilir,YerelIPAdresi,SonGirişZamanı,SonÇikişZamanı,Aktif")] tbKullanicilar tbKullanicilar)
		{
			if (ModelState.IsValid)
			{
				db.tbKullanicilar.Add(tbKullanicilar);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(tbKullanicilar);
		}

		[Route("Kullanicilar/xyz/{sKullaniciAdSoyadi}")]
		public ActionResult Degistir(string sKullaniciAdSoyadi)
		{
			if (string.IsNullOrEmpty(sKullaniciAdSoyadi))
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			//.........................................
			//var sk = sKullaniciAdSoyadi.Replace(" ", "-");
			var ks = sKullaniciAdSoyadi.Replace("-", " ");
			//.........................................
			try
			{
				int sID = (from u in db.tbKullanicilar
									 where u.AdıSoyadı == ks
									 select u).FirstOrDefault().KullaniciID;

				tbKullanicilar tbKullanicilar = db.tbKullanicilar.Find(sID);

				if (tbKullanicilar == null)
				{
					return HttpNotFound();
				}
				return View(tbKullanicilar);
			}
			catch (Exception ex)
			{
				var ss = ex.Message;
				throw;
			}

		}

		// GET: Kullanicilar/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tbKullanicilar tbKullanicilar = db.tbKullanicilar.Find(id);
			if (tbKullanicilar == null)
			{
				return HttpNotFound();
			}
			return View(tbKullanicilar);
		}

		// POST: Kullanicilar/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			tbKullanicilar tbKullanicilar = db.tbKullanicilar.Find(id);
			db.tbKullanicilar.Remove(tbKullanicilar);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		// GET: Kullanicilar/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tbKullanicilar tbKullanicilar = db.tbKullanicilar.Find(id);
			if (tbKullanicilar == null)
			{
				return HttpNotFound();
			}
			return View(tbKullanicilar);
		}

		// GET: Kullanicilar/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tbKullanicilar tbKullanicilar = db.tbKullanicilar.Find(id);
			if (tbKullanicilar == null)
			{
				return HttpNotFound();
			}
			////...şifre - encode - decode...............

			//var sEncodedSifre = sifreci.sifreci.EncodeString(tbKullanicilar.Şifresi);
			//var sDecodedSifre = sifreci.sifreci.DecodeString(sEncodedSifre);

			//ViewData["vdDegistirilecekKullanicininEncodedSifresi"] = sEncodedSifre;
			//ViewData["vdDegistirilecekKullanicininDecodedSifresi"] = sDecodedSifre;
			////..................
			return View(tbKullanicilar);
		}

		// POST: Kullanicilar/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "KullaniciID,KullanıcıAdı,Şifresi,AdıSoyadı,Grubu,Tipi,Adminmi,Ünvanı,LokasyonID,FaaliyetAlaniID,Emaili,CepTelefonu,DahiliTelefonu,Fotografi,UstAmiriID,SuanSistemde,DahiliMesajlaşmadaGörünür,BakımOnarımYapabilir,YerelIPAdresi,SonGirişZamanı,SonÇikişZamanı,Aktif")] tbKullanicilar tbKullanicilar)
		{
			if (ModelState.IsValid)
			{
				//var sEncodedSifre = sifreci.sifreci.EncodeString(tbKullanicilar.Şifresi);

				//tbKullanicilar.Şifresi = sEncodedSifre;

				db.Entry(tbKullanicilar).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(tbKullanicilar);
		}

		// GET: Kullanicilar
		public ActionResult Index()
		{

			return View(db.tbKullanicilar.ToList());
		}

		//***************************************
		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}  

		[HttpPost]
		public ActionResult Login(string sKullaniciAdi, string sSifre)
		{
			var usr = from u in db.tbKullanicilar
								where u.KullanıcıAdı == sKullaniciAdi &&
								u.Şifresi == sSifre &&
								u.Aktif == true
								select u;

			if (usr.Count() == 1)
			{
				//..............................................
				FormsAuthentication.SetAuthCookie(sKullaniciAdi, false);
				////////////////////////////////////////////////
				//Membership.CreateUser(sKullaniciAdi, sSifre);
				//Membership.ValidateUser(sKullaniciAdi, sSifre);
				////////////////////////////////////////////////
				//..............................................
				string sad = string.Empty;
				string[] words = usr.FirstOrDefault().AdıSoyadı.Split();
				foreach (var word in words)
				{
					sad += word + "</br>";
				}
				Session["sesAdıSoyadı"] = sad;
				Session["sesKullanıcıAdı"] = sKullaniciAdi;
				Session["sesUserName"] = sKullaniciAdi;  //üst menüdeki kullanıcı 	Login status....

				Session["sesAdminmi"] = usr.FirstOrDefault().Adminmi;
				Session["sesBakımOnarımYapabilir"] = usr.FirstOrDefault().BakımOnarımYapabilir;

				Session["sesFotografi"] = usr.FirstOrDefault().Fotografi;

				Session["sesGrubu"] = usr.FirstOrDefault().Grubu ?? string.Empty;  //grubu, rol yerine geçecek ve authentication yapılacak.07 ocak 2017...
																																					 //..........................................
																																					 //FormsAuthenticationTicket ft = new FormsAuthenticationTicket(sKullaniciAdi, false, 90);
																																					 //	FormsIdentity fi = new FormsIdentity(ft);

				//......aşağısı login cookie si........................................
				if (Response.Cookies["OrnekCookieLogin"] != null)
				{
					//Cookie nesnesi oluşturuyoruz.
					HttpCookie Cookie = new HttpCookie("OrnekCookieLogin");
					//Cookie bilgilerini tanımlıyoruz.
					Cookie["ckUname"] = sKullaniciAdi;
					Cookie["ckPass"] = sSifre;
					//Cookie'nin tutulacak süresini belirtiyoruz. süre belirtilmezse tarayıcı kapandığında silinir.
					Cookie.Expires = DateTime.Now.AddDays(1);
					//Cookie'yi ekleyerek, fiziksel olarak oluşturuyoruz.
					Response.Cookies.Add(Cookie);
				}

				//..............................................
				FormsAuthentication.RedirectFromLoginPage(userName: ClsKullanici.pr_KullanıcıAdı, createPersistentCookie: false);
				//..............................................
			}
			else
			{
				ModelState.AddModelError(string.Empty, "Login bigisi problemli!");
			}

			return View();
		}

		[HttpGet]
		public ActionResult Login2()
		{
			return View();
		}
		//***********************************************************
		public JsonResult LoginKontrolJson(string prmKullaniciAdi, string prmSifre)
		{

			var usr = from u in db.tbKullanicilar
								where u.KullanıcıAdı == prmKullaniciAdi &&
								u.Şifresi == prmSifre &&
								u.Aktif == true
								select u;

			if (usr.Count() == 1)
			{
				return Json("1", JsonRequestBehavior.AllowGet);
			}

			return Json("Hatalı-Kullanıcı/Şifre", JsonRequestBehavior.AllowGet);
		}



		[HttpGet]
		public ActionResult LogOn()
		{
			ViewBag.ReturnUrl = Convert.ToString(Request.QueryString["ReturnUrl"]);
			return View();
		}
		[HttpPost]
		public ActionResult LogOn(string sKullaniciAdi, string sSifre)
		{
			var usr = from u in db.tbKullanicilar
								where u.KullanıcıAdı == sKullaniciAdi.Trim() &&
								u.Şifresi == sSifre.Trim() &&
								u.Aktif == true
								select u;

			if (usr.Count() == 1)
			{
				//..............................................
				FormsAuthentication.SetAuthCookie(sKullaniciAdi, false);
				//..............................................
				string sad = string.Empty;
				string[] words = usr.FirstOrDefault().AdıSoyadı.Split();
				foreach (var word in words)
				{
					sad += word + "</br>";
				}

				Session["sesAdıSoyadı"] = sad;
				Session["sesKullanıcıAdı"] = sKullaniciAdi;
				Session["sesUserName"] = sKullaniciAdi;  //üst menüdeki kullanıcı 	Login status....
				Session["sesKullaniciID"] = usr.FirstOrDefault().KullaniciID;

				Session["sesAdminmi"] = usr.FirstOrDefault().Adminmi;
				Session["sesBakımOnarımYapabilir"] = usr.FirstOrDefault().BakımOnarımYapabilir;

				Session["sesFotografi"] = usr.FirstOrDefault().Fotografi;

				Session["sesGrubu"] = usr.FirstOrDefault().Grubu ?? string.Empty;  //grubu, rol yerine geçecek ve authentication yapılacak.07 ocak 2017...

				//var sRoller = ClsKullanici.pr_KullanicininRolleri; //TODO: bu metod çalışıyor. gerekirse açılır.

				//......aşağısı login cookie si........................................
				if (Response.Cookies["OrnekCookieLogin"] != null)
				{
					//Cookie nesnesi oluşturuyoruz.
					HttpCookie Cookie = new HttpCookie("OrnekCookieLogin");
					//Cookie bilgilerini tanımlıyoruz.
					Cookie["ckUname"] = sKullaniciAdi;
					Cookie["ckPass"] = sSifre;
					//Cookie'nin tutulacak süresini belirtiyoruz. süre belirtilmezse tarayıcı kapandığında silinir.
					Cookie.Expires = DateTime.Now.AddDays(1);
					//Cookie'yi ekleyerek, fiziksel olarak oluşturuyoruz.
					Response.Cookies.Add(Cookie);
				}

				//..............................................
				FormsAuthentication.RedirectFromLoginPage(userName: ClsKullanici.pr_KullanıcıAdı, createPersistentCookie: false);
				//..............................................
			}
			else
			{
				ModelState.AddModelError(string.Empty, "LogOn bigisi problemli!");
			}

			return View();
		}

		public ActionResult LogOut()
		{
			////////////////////////////////////////////////
			//Membership.DeleteUser(Session["sesUserName"].ToString());
			////////////////////////////////////////////////
			FormsAuthentication.SignOut();

			//......................
			Session.Clear();
			Session.Abandon();
			//.............................
			return RedirectToAction("Index", "Home");
		}
	}
}
