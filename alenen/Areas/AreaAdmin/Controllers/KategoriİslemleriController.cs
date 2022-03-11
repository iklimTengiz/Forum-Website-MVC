using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using alenen;
using PagedList;

namespace alenen.Areas.AreaAdmin.Controllers
{
    public class KategoriİslemleriController : AdminControllerBase
    {
        private alenenDBEntities db = new alenenDBEntities();


      
        // GET: AreaAdmin/Kategoriİslemleri
        public ActionResult Index(string searching, int? page)
        {
            var kategori = from k in db.Kategoris
                          select k;
            if (!String.IsNullOrEmpty(searching))
            {

                kategori = kategori.Where(k => k.kategoriad.Contains(searching));

            }
            var pagenumber = page ?? 1;
            var pageSize = 15;
            return View(kategori.OrderBy(p => p.kategoriid).ToPagedList(pagenumber, pageSize));
        }

        // GET: AreaAdmin/Kategoriİslemleri/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = db.Kategoris.Find(id);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // GET: AreaAdmin/Kategoriİslemleri/Create
        public ActionResult Create()
        {
            ViewBag.anakatid = new SelectList(db.AnaKategoris, "anakatid", "anakatad");
            ViewBag.message = "Var olan bir kategori adını kullanmaya çalışıyorsunuz.";
            return View();
        }

        // POST: AreaAdmin/Kategoriİslemleri/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "kategoriid,kategoriad,aciklama,olustarih,olusturanid,resim,anakatid")] Kategori kategori, HttpPostedFileBase imagee)
        {

            if (imagee != null)
            {
                if (Directory.Exists(Server.MapPath("/Content/images/")) == false)
                {
                    Directory.CreateDirectory(Server.MapPath("/Content/images/"));
                }
                if ((imagee.FileName.Contains("jpg") || imagee.FileName.Contains("jpeg") || imagee.FileName.Contains("png")))
                {
                    imagee.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), imagee.FileName));

                }

            }
            if (ModelState.IsValid)
            {
                kategori.olusturanid = LoginUserID;
                kategori.olustarih = DateTime.Now;
                kategori.resim = imagee.FileName;
                db.Kategoris.Add(kategori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.anakatid = new SelectList(db.AnaKategoris, "anakatid", "anakatad", kategori.anakatid);
            return View(kategori);
        }

        // GET: AreaAdmin/Kategoriİslemleri/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = db.Kategoris.Find(id);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            ViewBag.anakatid = new SelectList(db.AnaKategoris, "anakatid", "anakatad");
            return View(kategori);
        }

        // POST: AreaAdmin/Kategoriİslemleri/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "kategoriid,kategoriad,aciklama,olustarih,olusturanid,resim,anakatid")] Kategori kategori, HttpPostedFileBase imagee)
        {
            if (imagee != null)
            {
                if (Directory.Exists(Server.MapPath("/Content/images/")) == false)
                {
                    Directory.CreateDirectory(Server.MapPath("/Content/images/"));
                }
                if ((imagee.FileName.Contains("jpg") || imagee.FileName.Contains("jpeg") || imagee.FileName.Contains("png")))
                {
                    imagee.SaveAs(Path.Combine(Server.MapPath("~/Content/images/"), imagee.FileName));

                }

            }
            if (ModelState.IsValid)
            {
                if (imagee == null)
                {

                }
                else
                {
                    kategori.resim = imagee.FileName;
                }
                db.Entry(kategori).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.anakatid = new SelectList(db.AnaKategoris, "anakatid", "anakatad", kategori.anakatid);
            return View(kategori);
        }

        // GET: AreaAdmin/Kategoriİslemleri/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kategori kategori = db.Kategoris.Find(id);
            if (kategori == null)
            {
                return HttpNotFound();
            }
            return View(kategori);
        }

        // POST: AreaAdmin/Kategoriİslemleri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kategori kategori = db.Kategoris.Find(id);
            db.Kategoris.Remove(kategori);
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
