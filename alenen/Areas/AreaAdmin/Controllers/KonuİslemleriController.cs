using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using alenen;
using PagedList;

namespace alenen.Areas.AreaAdmin.Controllers
{
    public class KonuİslemleriController : AdminControllerBase
    {
        private alenenDBEntities db = new alenenDBEntities();

        // GET: AreaAdmin/Konuİslemleri
        public ActionResult Index(string searching,int? page)
        {
            var knu = from k in db.Konulars
                           select k;
            if (!String.IsNullOrEmpty(searching))
            {

                knu = knu.Where(k => k.konuad.Contains(searching));

            }
            var pagenumber = page ?? 1;
            var pageSize = 15;
            var konulars = db.Konulars.Include(k => k.Kategori).Include(k => k.User);
            return View(knu.OrderBy(p => p.konuid).ToPagedList(pagenumber, pageSize));
        }

        // GET: AreaAdmin/Konuİslemleri/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Konular konular = db.Konulars.Find(id);
            if (konular == null)
            {
                return HttpNotFound();
            }
            return View(konular);
        }

        // GET: AreaAdmin/Konuİslemleri/Create
        public ActionResult Create()
        {
            ViewBag.kategoriid = new SelectList(db.Kategoris, "kategoriid", "kategoriad");
            ViewBag.olusturanid = new SelectList(db.Users, "userid", "mail");
            return View();
        }

        // POST: AreaAdmin/Konuİslemleri/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "konuid,yorum,konuad,kategoriid,resim,olustarih,olusturanid")] Konular konular)
        {
            if (ModelState.IsValid)
            {
                db.Konulars.Add(konular);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kategoriid = new SelectList(db.Kategoris, "kategoriid", "kategoriad", konular.kategoriid);
            ViewBag.olusturanid = new SelectList(db.Users, "userid", "mail", konular.olusturanid);
            return View(konular);
        }

        // GET: AreaAdmin/Konuİslemleri/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Konular konular = db.Konulars.Find(id);
            if (konular == null)
            {
                return HttpNotFound();
            }
            ViewBag.kategoriid = new SelectList(db.Kategoris, "kategoriid", "kategoriad", konular.kategoriid);
            ViewBag.olusturanid = new SelectList(db.Users, "userid", "mail", konular.olusturanid);
            return View(konular);
        }

        // POST: AreaAdmin/Konuİslemleri/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "konuid,yorum,konuad,kategoriid,resim,olustarih,olusturanid")] Konular konular)
        {
            if (ModelState.IsValid)
            {
                db.Entry(konular).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kategoriid = new SelectList(db.Kategoris, "kategoriid", "kategoriad", konular.kategoriid);
            ViewBag.olusturanid = new SelectList(db.Users, "userid", "mail", konular.olusturanid);
            return View(konular);
        }

        // GET: AreaAdmin/Konuİslemleri/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Konular konular = db.Konulars.Find(id);
            if (konular == null)
            {
                return HttpNotFound();
            }
            return View(konular);
        }

        // POST: AreaAdmin/Konuİslemleri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Konular konular = db.Konulars.Find(id);
            db.Konulars.Remove(konular);
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
