using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using webApi1;

namespace webApi1.Controllers
{
    public class KullanicilarController : ApiController
    {
        private dbOnemliIstekMakinaBakimEntities db = new dbOnemliIstekMakinaBakimEntities();

        // GET: api/Kullanicilar
        public IQueryable<tbKullanicilar> GettbKullanicilar()
        {
            return db.tbKullanicilar;
        }

        // GET: api/Kullanicilar/5
        [ResponseType(typeof(tbKullanicilar))]
        public IHttpActionResult GettbKullanicilar(int id)
        {
            tbKullanicilar tbKullanicilar = db.tbKullanicilar.Find(id);
            if (tbKullanicilar == null)
            {
                return NotFound();
            }

            return Ok(tbKullanicilar);
        }

        // PUT: api/Kullanicilar/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PuttbKullanicilar(int id, tbKullanicilar tbKullanicilar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbKullanicilar.KullaniciID)
            {
                return BadRequest();
            }

            db.Entry(tbKullanicilar).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbKullanicilarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Kullanicilar
        [ResponseType(typeof(tbKullanicilar))]
        public IHttpActionResult PosttbKullanicilar(tbKullanicilar tbKullanicilar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbKullanicilar.Add(tbKullanicilar);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbKullanicilarExists(tbKullanicilar.KullaniciID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbKullanicilar.KullaniciID }, tbKullanicilar);
        }

        // DELETE: api/Kullanicilar/5
        [ResponseType(typeof(tbKullanicilar))]
        public IHttpActionResult DeletetbKullanicilar(int id)
        {
            tbKullanicilar tbKullanicilar = db.tbKullanicilar.Find(id);
            if (tbKullanicilar == null)
            {
                return NotFound();
            }

            db.tbKullanicilar.Remove(tbKullanicilar);
            db.SaveChanges();

            return Ok(tbKullanicilar);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbKullanicilarExists(int id)
        {
            return db.tbKullanicilar.Count(e => e.KullaniciID == id) > 0;
        }
    }
}