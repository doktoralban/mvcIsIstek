using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mvcIsIstek.App_Data;
using System.IO;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace mvcIsIstek.Controllers
{

	//[Authorize(Roles = "Admin")]
	public class MakinalarController : Controller
    {
        private dbOnemliIstekMakinaBakimEntities db = new dbOnemliIstekMakinaBakimEntities();

		// GET: Makinalar
		//[Route("Anasayfa")]
		public ActionResult Index( )
        {
            var tbMakinalar = db.tbMakinalar.Include(t => t.tbLokasyonTanimlari);
 
            return View(tbMakinalar.ToList()  );
        }

		//.........................................................
		public JsonResult SecilenIdlereGoreMakinalar(string sSecilenidler)
		{
 			Session["tmptmaks"] = "";

			if (sSecilenidler.Substring(sSecilenidler.Length-1)==",")
			{
				sSecilenidler = sSecilenidler.Remove(sSecilenidler.Length - 1);
			}

			SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
			sqlBuilder.DataSource =			"LOCALHOST";
			sqlBuilder.InitialCatalog = "dbOnemliIstekMakinaBakim";
			sqlBuilder.IntegratedSecurity = false;
			sqlBuilder.UserID = "sa";
			sqlBuilder.Password = "sapass";
			string providerString = sqlBuilder.ToString();
			//.....................
			SqlConnection sqlConnection1 = new SqlConnection(providerString);
			SqlCommand cmd = new SqlCommand();
			SqlDataReader reader;

			cmd.CommandText = "sp_TestMakinalar1";
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("SeciliIdler", sSecilenidler);
			cmd.Connection = sqlConnection1;

			sqlConnection1.Open(); 

			List<tbMakinalar> tmaks = new List<tbMakinalar>();
			try
			{
				reader = cmd.ExecuteReader();

				while (reader.Read())
				{
 					tmaks.Add(new tbMakinalar
					{ 
						MakinaID = Convert.ToInt32(reader["MakinaID"])
						,
						 Kodu = reader["Kodu"].ToString(),
						 Tanımı = reader["Tanımı"].ToString()

					 }); 
					}

				cmd.Dispose();
				sqlConnection1.Close();
			}
			catch (Exception ex)
			{
				var ss = ex.Message;
			}

			Session["tmptmaks"] = tmaks;

			return Json(JsonRequestBehavior.AllowGet);

 		}

		public ViewResult SecilenMakinalarListesiEkrani()
		{
			List<tbMakinalar> tmaks = new List<tbMakinalar>();
			if (Session["tmptmaks"] != null)
			{
				if (Session["tmptmaks"].ToString().Length >0 )
				{
					tmaks = (List<tbMakinalar>)Session["tmptmaks"];
 				}
			}

			return View("SecilenIdlereGoreMakinalar", tmaks);
		}
		public JsonResult Bul(int? mid)
		{
			tbMakinalar tbMakinalar = db.tbMakinalar.Find(mid);
			return Json(mid.ToString() + ",", JsonRequestBehavior.AllowGet);
			//return Json(mid.ToString() +	" - " +  tbMakinalar.Tanımı, JsonRequestBehavior.AllowGet);

		}
		//.........................................................

		private string TESTsf()
		{
			//ClsKullanici.Fn_KullanicininYetkileriControllerVeAction("Makinalar", "Sayfala", ClsKullanici.pr_KullaniciID) ;
			return "";
		}
		//[Authorize(Users = "User1,User2")]
		[Authorize]
		//[Authorize(Users = "a.turker")]
		//[Authorize(Users = "") ]
		public ActionResult Sayfala(int? SayfaNo)
		{
			var tbMakinalar = db.tbMakinalar.Include(t => t.tbLokasyonTanimlari);
			int xSayfadakiSatir = 999999; 
			int xSkip = 0;

			ViewBag.SecilenSayfaNo = "0";

			if (SayfaNo == 1)
			{
				xSayfadakiSatir = 10;
				xSkip = 0;

				ViewBag.SecilenSayfaNo = SayfaNo.ToString();
			}
			else if (SayfaNo > 1)
			{
				xSayfadakiSatir = 10;
				xSkip = (xSayfadakiSatir * Convert.ToInt32(SayfaNo-1));

				ViewBag.SecilenSayfaNo = SayfaNo.ToString();
			}
			else 
			{
				ViewBag.SecilenSayfaNo = "0";
				xSayfadakiSatir = 999999;
				xSkip = 0;
			}

			ViewBag.TumMakinalarSayisi = tbMakinalar.ToList().Count;

			return View(tbMakinalar.ToList().Skip(xSkip).Take(xSayfadakiSatir).ToList());
		}

 		public ViewResult LokasyonaGoreMakinalar(string sLokasyonTanimi)
		{
			//lokasyon tanımına göre makinalar
 		var sd =	 (from tMak in db.tbMakinalar
			 from tLks in db.tbLokasyonTanimlari
			 where tMak.LokasyonID == tLks.LokasyonID
								where tLks.Lokasyon == sLokasyonTanimi
								select tMak); 

			return View("Sayfala", sd.ToList()); 
		}

		public ViewResult MakinalarSorguEkrani ( string Kodu,string Tipi)
		{
			string sSorguWhereCumlesi = "";
			if (!string.IsNullOrEmpty(Kodu))
			{
				sSorguWhereCumlesi = " (Kodu='" + Kodu + "') "; 
			}
			if (!string.IsNullOrEmpty(Tipi))
			{
				if (sSorguWhereCumlesi =="")
				{
					sSorguWhereCumlesi = " (Tipi='" + Tipi + "') ";
 				}
				else
				{
					sSorguWhereCumlesi += " AND (Tipi='" + Tipi + "') "; 
				} 
			}
			//.....................
			if (sSorguWhereCumlesi != "")
			{
				sSorguWhereCumlesi = " WHERE " + sSorguWhereCumlesi;
			}


 			SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
			sqlBuilder.DataSource = "LOCALHOST";
			sqlBuilder.InitialCatalog = "dbOnemliIstekMakinaBakim";
			sqlBuilder.IntegratedSecurity = false;
			sqlBuilder.UserID = "sa";
			sqlBuilder.Password = "sapass";
			string providerString = sqlBuilder.ToString();
			//.....................
			SqlConnection sqlConnection1 = new SqlConnection(providerString);
			SqlCommand cmd = new SqlCommand();
			SqlDataReader reader;

			cmd.CommandText = "sp_TestMakinalar2";
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.AddWithValue("SorguWhereCumlesi", sSorguWhereCumlesi);
			cmd.Connection = sqlConnection1;

			sqlConnection1.Open();

			List<tbMakinalar> tmaks = new List<tbMakinalar>();
			try
			{
				reader = cmd.ExecuteReader();

				while (reader.Read())
				{
					tmaks.Add(new tbMakinalar
					{
						MakinaID = Convert.ToInt32(reader["MakinaID"])
					 ,
						Kodu = reader["Kodu"].ToString(),
						Tanımı = reader["Tanımı"].ToString(),
						Grubu = reader["Grubu"].ToString(),
						Tipi = reader["Tipi"].ToString()

					});
				}

				cmd.Dispose();
				sqlConnection1.Close();
			}
			catch (Exception ex)
			{
				var ss = ex.Message;
			}

			return View("MakinalarSorguEkrani", tmaks);
		}


		// GET: Makinalar/Details/5
		//[Authorize(Roles  = "Tümü,Teknik")]
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbMakinalar tbMakinalar = db.tbMakinalar.Find(id);
            if (tbMakinalar == null)
            {
                return HttpNotFound();
            }
            return View(tbMakinalar);
        }
		//........................................
		public ActionResult MakinaKarti(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tbMakinalar tbMakinalar = db.tbMakinalar.Find(id);
			if (tbMakinalar == null)
			{
				return HttpNotFound();
			}
			return View(tbMakinalar);
		}
		//........................................
		[HttpGet]
		public ActionResult DetayPopUp(int id)
		{
			tbMakinalar mkn = db.tbMakinalar.Find(id);

			return PartialView("PartialPopUpDetay",mkn);
		}
		//........................................
		[HttpGet]
		public ActionResult SilPopUp(int id)
		{
			tbMakinalar mkn = db.tbMakinalar.Find(id);

			return PartialView("PartialPopUpSil", mkn);
		}
		//........................................
		public ActionResult DetayMakinaKodu(string mKod)
		{
			if (mKod == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tbMakinalar tbMakinalar = db.tbMakinalar.Where(p => p.Kodu == mKod).FirstOrDefault();
			if (tbMakinalar == null)
			{
				return HttpNotFound();
			}
			return View(tbMakinalar);
		}
		//........................................

		//..............XXXXXXXXXXXXXXXXXXXXXXX..........................
		// GET: Makinalar/Create
	
		public ActionResult Create()
        {
            ViewBag.LokasyonIDs = new SelectList(db.tbLokasyonTanimlari, "LokasyonID", "Lokasyon");
			//..........................................
			var st = Enum.GetNames(typeof(En_MakinaTipi));

			ViewBag.Tipis = new SelectList(st);
			////.......................................
			////var mtS = (from u in db.tbMakinalar
			////					 select new { u.Tipi }).Distinct();

			////ViewData["MakinaTipleri"] = new SelectList(mtS, "Tipi", "Tipi");

			////.......................................
		 
			ViewBag.YeniAktif = true;

			return View();
        }


		public enum En_MakinaTipi
		{
			Makina,
			Ekipman
		}


		// POST: Makinalar/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MakinaID,Kodu,Tanımı,Grubu,Tipis,SeriNo" +
					",Markası,Modeli,Açıklaması,LokasyonIDs,BakimTalimatKodu,ÖzelKod1,ÖzelKod2,ÖzelKod3" +
					",AlindigiTarih,AlındığıFirmaBilgileri,SorumluPersonel,PlanlıBakımYapılabilir" +
					",KalibrasyonYapılabilir,İşİsteklerindeGörüntülenebilir,GünlükÇalışmaSaati" +
					",Fotoğrafı,Aktif")] tbMakinalar tbMakinalar
					, HttpPostedFileBase uploadYeni)
        {

			if (string.IsNullOrEmpty(tbMakinalar.Kodu))
			{
				ModelState.AddModelError("Kodu", "Makina Kodu Olmalıdır..."); 
			}
			if (string.IsNullOrEmpty(tbMakinalar.Tanımı))
			{
				ModelState.AddModelError("Kodu", "Makina Tanımı Olmalıdır...");
			}
			if (string.IsNullOrEmpty(tbMakinalar.Tipi))
			{
				ModelState.AddModelError("Kodu", "Makina Tanımı Olmalıdır...");
			}
			if (tbMakinalar.LokasyonID <=0)
			{
				ModelState.AddModelError("Kodu", "Makina Lokasyonu Olmalıdır...");
			}
			//...............................................................
			//if (!ModelState.IsValid )
			//{
			//	return View(model: tbMakinalar); 
			//}
			//.................................................................
			if (ModelState.IsValid)
            {
 					if (uploadYeni != null && uploadYeni.ContentLength > 0)
					{
						var allowedExtensions = new[] { ".jpg", ".png", ".gif" };
						var checkextension = Path.GetExtension(uploadYeni.FileName).ToLower();
						if (!allowedExtensions.Contains(checkextension) || (uploadYeni.ContentLength > 5500000))
						{
							TempData["msgDosyaProblemiYeni"] = " Eklenecek Dosya, .jpg,  .png veya  .gif dosyaları olmalı ve 5Mb yi geçmemelidir....";

							return View("Create");
						}
					//...................................................................
					var fileName = Path.GetFileName(uploadYeni.FileName);
					var path = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
					uploadYeni.SaveAs(path);
					//................................
					// aşağıdaki 3 bloktaki kodda aynı şekilde çalışıyor 
					//1.
					//var fileBytes = new byte[filex.ContentLength];
					//filex.InputStream.Read(fileBytes, 0, fileBytes.Length);
					//.......................................
					//2.
					//using (BinaryReader b = new BinaryReader(filex.InputStream))
					//{
					//	byte[] binData = b.ReadBytes(filex.ContentLength);

					//	dbcategories.Picture = binData;
					//}
					//.......................................
					//3.
					byte[] data;
					using (Stream inputStream = uploadYeni.InputStream)
					{
						MemoryStream memoryStream = inputStream as MemoryStream;
						if (memoryStream == null)
						{
							memoryStream = new MemoryStream();
							inputStream.CopyTo(memoryStream);

						}
						data = memoryStream.ToArray();

						tbMakinalar.Fotoğrafı = data;

					}
				}

				//...................................

				db.tbMakinalar.Add(tbMakinalar);
				db.SaveChanges();
				return RedirectToAction("Sayfala");
            }

			ViewBag.LokasyonID = new SelectList(db.tbLokasyonTanimlari, "LokasyonID", "Lokasyon", tbMakinalar.LokasyonID);
			return View(tbMakinalar);
		}



		// GET: Makinalar/Edit/5
		public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
						if (id <=0)
						{

				return RedirectToAction("Error500", "Error");
				//Response.Redirect("~/Error/Error500");
				//return new HttpStatusCodeResult(HttpStatusCode.NotFound);
			}
			tbMakinalar tbMakinalar = db.tbMakinalar.Find(id);
            if (tbMakinalar == null)
            {
                return HttpNotFound();
            } 


						ViewBag.LokasyonID = new SelectList(db.tbLokasyonTanimlari, "LokasyonID", "Lokasyon", tbMakinalar.LokasyonID);

			//.......................................
			//var mtS = (from u in db.tbMakinalar
			//					select new { u.Tipi }).Distinct();

			//ViewData["MakinaTipleri"] = new SelectList(mtS, "Tipi", "Tipi");
			//...........................................
			var st = Enum.GetNames(typeof(En_MakinaTipi));

			ViewBag.Tipis = new SelectList(st);

			//.......................................

			return View(tbMakinalar);
        }

        // POST: Makinalar/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MakinaID,Kodu,Tanımı,Grubu,Tipi,SeriNo,Markası,Modeli,Açıklaması" +
					",LokasyonID,BakimTalimatKodu,ÖzelKod1,ÖzelKod2,ÖzelKod3,AlındığıTarih,AlındığıFirmaBilgileri" +
					",SorumluPersonel,PlanlıBakımYapılabilir,KalibrasyonYapılabilir,İşİsteklerindeGörüntülenebilir" +
					",Aktif")] tbMakinalar degistirilenMakinaTanimi, HttpPostedFileBase uploadDegistir)
        {
			//**************************************
			if (degistirilenMakinaTanimi.Kodu.Trim().Length < 4)
			{
				//ViewBag.MakinaKoduHatalı = "1";
				TempData["MakinaKoduHatalı"] = "1";
				return RedirectToAction("Edit", degistirilenMakinaTanimi.MakinaID);
			}
			TempData["MakinaKoduHatalı"] = "0";
			//**************************************
			if (ModelState.IsValid)
            {
                db.Entry(degistirilenMakinaTanimi).State = EntityState.Modified; 

				if (uploadDegistir != null && uploadDegistir.ContentLength > 0)
				{
					var allowedExtensions = new[] { ".jpg", ".png", ".gif" };
					var checkextension = Path.GetExtension(uploadDegistir.FileName).ToLower();
					if (!allowedExtensions.Contains(checkextension) || (uploadDegistir.ContentLength > 5500000))
					{
						TempData["msgDosyaProblemiDegistir"] = " Değiştirilecek Dosya, .jpg,  .png veya  .gif dosyaları olmalı ve 5Mb yi geçmemelidir....";

						return View("Edit");
					}
					//...................................................................
					var fileName = Path.GetFileName(uploadDegistir.FileName);
					var path = Path.Combine(Server.MapPath("~/Content/Uploads"), fileName);
					uploadDegistir.SaveAs(path);
					//................................
					// aşağıdaki 3 bloktaki kodda aynı şekilde çalışıyor 
					//1.
					//var fileBytes = new byte[filex.ContentLength];
					//filex.InputStream.Read(fileBytes, 0, fileBytes.Length);
					//.......................................
					//2.
					//using (BinaryReader b = new BinaryReader(filex.InputStream))
					//{
					//	byte[] binData = b.ReadBytes(filex.ContentLength);

					//	dbcategories.Picture = binData;
					//}
					//.......................................
					//3.
					byte[] data;
					using (Stream inputStream = uploadDegistir.InputStream)
					{
						MemoryStream memoryStream = inputStream as MemoryStream;
						if (memoryStream == null)
						{
							memoryStream = new MemoryStream();
							inputStream.CopyTo(memoryStream);

						}
						data = memoryStream.ToArray();

						degistirilenMakinaTanimi.Fotoğrafı = data;

					}
				}
 
				//.......................................................
				var iid = degistirilenMakinaTanimi.MakinaID;
							dbOnemliIstekMakinaBakimEntities xmodel = new dbOnemliIstekMakinaBakimEntities();
 							var mkf = (xmodel.tbMakinalar.Where(p => p.MakinaID == iid).FirstOrDefault());
							if (mkf.Fotoğrafı != null && degistirilenMakinaTanimi.Fotoğrafı == null)
							{
								degistirilenMakinaTanimi.Fotoğrafı = mkf.Fotoğrafı;
							}

                db.SaveChanges();
                return RedirectToAction("Sayfala");
            }
            ViewBag.LokasyonID = new SelectList(db.tbLokasyonTanimlari, "LokasyonID", "Lokasyon", degistirilenMakinaTanimi.LokasyonID);

					return View("Edit",degistirilenMakinaTanimi);
        }

        // GET: Makinalar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbMakinalar tbMakinalar = db.tbMakinalar.Find(id);
            if (tbMakinalar == null)
            {
                return HttpNotFound();
            }
            return View(tbMakinalar);
        }

        // POST: Makinalar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbMakinalar tbMakinalar = db.tbMakinalar.Find(id);
            db.tbMakinalar.Remove(tbMakinalar);
            db.SaveChanges();
            return RedirectToAction("Sayfala");
        }

		//*******************************************************************
		public JsonResult IdyeGoreMakinaBilgileri(int? secilenMakinaID)
		{
			db.Configuration.ProxyCreationEnabled = false;      

			if (secilenMakinaID == null)
			{
				secilenMakinaID = 0;
			}
			var lksmak = (from u in db.tbMakinalar
										where u.MakinaID == secilenMakinaID
										select new
										{
											u.MakinaID,
											u.Kodu,
											kodTanim = u.Kodu + " - " + u.Tanımı,
											u.Tanımı,
											u.Tipi
										}).ToList().OrderBy(p => p.kodTanim);

			return Json(lksmak, JsonRequestBehavior.AllowGet);
		}
		//*******************************************************************
		//------------------------------------------------------

		protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
