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
    public class AnaKategoriController : Controller
    {
        private alenenDBEntities db = new alenenDBEntities();

        // GET: AreaAdmin/AnaKategori
        public ActionResult Index(string searching, int? page)
        {
            var anakat = from k in db.AnaKategoris
                           select k;
            if (!String.IsNullOrEmpty(searching))
            {

                anakat = anakat.Where(k => k.anakatad.Contains(searching));

            }
            var pagenumber = page ?? 1;
            var pageSize = 15;
            return View(anakat.OrderBy(p => p.anakatid).ToPagedList(pagenumber, pageSize));
        
        }

        // GET: AreaAdmin/AnaKategori/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnaKategori anaKategori = db.AnaKategoris.Find(id);
            if (anaKategori == null)
            {
                return HttpNotFound();
            }
            return View(anaKategori);
        }

        // GET: AreaAdmin/AnaKategori/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AreaAdmin/AnaKategori/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "anakatid,anakatad,resim")] AnaKategori anaKategori, HttpPostedFileBase imagee)
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
                anaKategori.resim = imagee.FileName;
                db.AnaKategoris.Add(anaKategori);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(anaKategori);
        }

        // GET: AreaAdmin/AnaKategori/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnaKategori anaKategori = db.AnaKategoris.Find(id);
            if (anaKategori == null)
            {
                return HttpNotFound();
            }
            return View(anaKategori);
        }

        // POST: AreaAdmin/AnaKategori/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "anakatid,anakatad,resim")] AnaKategori anaKategori, HttpPostedFileBase imagee)
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
                    anaKategori.resim = imagee.FileName;
                }

                db.Entry(anaKategori).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(anaKategori);
        }

        // GET: AreaAdmin/AnaKategori/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnaKategori anaKategori = db.AnaKategoris.Find(id);
            if (anaKategori == null)
            {
                return HttpNotFound();
            }
            return View(anaKategori);
        }

        // POST: AreaAdmin/AnaKategori/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AnaKategori anaKategori = db.AnaKategoris.Find(id);
            db.AnaKategoris.Remove(anaKategori);
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
