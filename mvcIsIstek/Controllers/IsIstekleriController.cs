using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mvcIsIstek.App_Data;

namespace mvcIsIstek.Controllers
{
	public class IsIstekleriController : Controller
	{
		private dbOnemliIstekMakinaBakimEntities db = new dbOnemliIstekMakinaBakimEntities();

		// GET: IsIstekleri
		public ActionResult Index()
		{
			var tbIsIstekleri = db.tbIsIstekleri.Include(t => t.tbIstekTipleri).Include(t => t.tbLokasyonTanimlari);


			return View(tbIsIstekleri.ToList().OrderByDescending(p => p.IsIstekID));
		}

		// GET: IsIstekleri/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tbIsIstekleri tbIsIstekleri = db.tbIsIstekleri.Find(id);
			if (tbIsIstekleri == null)
			{
				return HttpNotFound();
			}
			return View(tbIsIstekleri);
		}

		// GET: IsIstekleri/Create
		public ActionResult Create()
		{
			var lst = (from u in db.tbMakinalar
								 select new { u.MakinaID, kt = u.Kodu + " - " + u.Tanımı }).ToList().OrderBy(p => p.kt);
			ViewBag.IstekLokasyonID = new SelectList(db.tbLokasyonTanimlari, "LokasyonID", "Lokasyon");

			//..........................................
			var st = Enum.GetNames(typeof(En_IsIstekKayitTipi));

			ViewBag.KayitTipis = new SelectList(st);
			//..........................................



			return View();
		}

		public enum En_IsIstekKayitTipi
		{
			İşİstek,
			Arıza
		}

		// POST: IsIstekleri/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "IstekLokasyonID,MakinaID" +
					",IstekKonusu,IstekAçıklamasıXP")] tbIsIstekleri tbIsIstekleri
					, int? sSecilenMakinaID)
		{

			//--- aşağıdaki if, combodan makina seçilirse...
			if (sSecilenMakinaID != null && tbIsIstekleri.MakinaID == null)
			{
				tbIsIstekleri.MakinaID = sSecilenMakinaID;
			}

			//*****************************************************************
			//TODO:...varsayılan değerler..........................
			tbIsIstekleri.Bitti = false;
			tbIsIstekleri.IstekAktif = true;
			tbIsIstekleri.IstekDurumu = "Yeni";
			tbIsIstekleri.KayıtTipi = "İşİstek";
			tbIsIstekleri.KayıtZamanı = System.DateTime.Now;
			tbIsIstekleri.KayıtBilgisayar = Environment.MachineName;

			tbIsIstekleri.IstekTipiID = 20;//20=Diğer
			tbIsIstekleri.IstekYapanKullaniciID = 1;//buraya session["sesKullaniciID"] gelecek.
       //*****************************************************************
				//.............................
			if (ModelState.IsValid)
			{
				db.tbIsIstekleri.Add(tbIsIstekleri);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			//ViewBag.IstekTipiID = new SelectList(db.tbIstekTipleri, "IstekTipiID", "Grubu", tbIsIstekleri.IstekTipiID);

			ViewBag.IstekLokasyonID = new SelectList(db.tbLokasyonTanimlari.OrderBy(p => p.Lokasyon), "LokasyonID", "Lokasyon", tbIsIstekleri.IstekLokasyonID);

			return View(tbIsIstekleri);
		}

		//.........+++++++++++++++++........................................................
		public JsonResult LokasyonaGoreMakinalar(int? secilenLokasyonID)
		{
			db.Configuration.ProxyCreationEnabled = false;     //TODO: buraya açıklama yazılacak.

 			if (secilenLokasyonID == null)
			{
				secilenLokasyonID = 0;
			}
			var lksmak = (from u in db.tbMakinalar
										where u.LokasyonID == secilenLokasyonID
										select new
										{
											u.MakinaID,
											kodTanim = u.Kodu + " - " + u.Tanımı
										}).ToList().OrderBy(p => p.kodTanim);

			return Json(lksmak, JsonRequestBehavior.AllowGet);
		}
		//.................................................................

		// GET: IsIstekleri/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tbIsIstekleri tbIsIstekleri = db.tbIsIstekleri.Find(id);
			if (tbIsIstekleri == null)
			{
				return HttpNotFound();
			}
			ViewBag.IstekTipiID = new SelectList(db.tbIstekTipleri, "IstekTipiID", "Grubu", tbIsIstekleri.IstekTipiID);
			ViewBag.IstekLokasyonID = new SelectList(db.tbLokasyonTanimlari, "LokasyonID", "Lokasyon", tbIsIstekleri.IstekLokasyonID);
			return View(tbIsIstekleri);
		}

		// POST: IsIstekleri/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "IsIstekID,KayıtTipi,IstekYapanKullaniciID,IstekTipiID,IstekLokasyonID,MakinaID,IstekKonusu,IstekAçıklaması,IstekAçıklamasıXP,IstekDurumu,IstekAktif,Bitti,IstekSonuçAçıklaması,IstekSonuçAçıklamasıXP,MakinaBakimOnarimID,SorunGiderilmeZamanı,IsIstekBitisZamanı,KayıtZamanı,KayıtBilgisayar,SonGuncelleyenID,SonGuncellemeZamani,SonGuncellemeBilgisayar")] tbIsIstekleri tbIsIstekleri)
		{
			if (ModelState.IsValid)
			{
				db.Entry(tbIsIstekleri).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			ViewBag.IstekTipiID = new SelectList(db.tbIstekTipleri, "IstekTipiID", "Grubu", tbIsIstekleri.IstekTipiID);
			ViewBag.IstekLokasyonID = new SelectList(db.tbLokasyonTanimlari, "LokasyonID", "Lokasyon", tbIsIstekleri.IstekLokasyonID);
			return View(tbIsIstekleri);
		}

		// GET: IsIstekleri/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			tbIsIstekleri tbIsIstekleri = db.tbIsIstekleri.Find(id);
			if (tbIsIstekleri == null)
			{
				return HttpNotFound();
			}
			return View(tbIsIstekleri);
		}

		// POST: IsIstekleri/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			tbIsIstekleri tbIsIstekleri = db.tbIsIstekleri.Find(id);
			db.tbIsIstekleri.Remove(tbIsIstekleri);
			db.SaveChanges();
			return RedirectToAction("Index");
		}





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
