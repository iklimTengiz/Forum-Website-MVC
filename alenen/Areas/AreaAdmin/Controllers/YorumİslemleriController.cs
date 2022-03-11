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
    public class YorumİslemleriController : AdminControllerBase
    {
        private alenenDBEntities db = new alenenDBEntities();

        // GET: AreaAdmin/Yorumİslemleri
        public ActionResult Index(string searching, int? page)
        {
            var yorum = from k in db.Yorums
                           select k;
            if (!String.IsNullOrEmpty(searching))
            {

                yorum = yorum.Where(k => k.icerik.Contains(searching));

            }
            var pagenumber = page ?? 1;
            var pageSize = 15;
            var yorums = db.Yorums.Include(y => y.Konular).Include(y => y.User); ;
            return View(yorum.OrderBy(p => p.yorumid).ToPagedList(pagenumber, pageSize));
        }

        // GET: AreaAdmin/Yorumİslemleri/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorum yorum = db.Yorums.Find(id);
            if (yorum == null)
            {
                return HttpNotFound();
            }
            return View(yorum);
        }

        // GET: AreaAdmin/Yorumİslemleri/Create
        public ActionResult Create()
        {
            ViewBag.konuid = new SelectList(db.Konulars, "konuid", "yorum");
            ViewBag.userid = new SelectList(db.Users, "userid", "kadi");
            return View();
        }

        // POST: AreaAdmin/Yorumİslemleri/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "yorumid,icerik,onay,resim,userid,konuid,olustarih")] Yorum yorum)
        {
            if (ModelState.IsValid)
            {
                db.Yorums.Add(yorum);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.konuid = new SelectList(db.Konulars, "konuid", "yorum", yorum.konuid);
            ViewBag.userid = new SelectList(db.Users, "userid", "kadi", yorum.userid);
            return View(yorum);
        }

        // GET: AreaAdmin/Yorumİslemleri/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorum yorum = db.Yorums.Find(id);
            if (yorum == null)
            {
                return HttpNotFound();
            }
            ViewBag.konuid = new SelectList(db.Konulars, "konuid", "yorum", yorum.konuid);
            ViewBag.userid = new SelectList(db.Users, "userid", "kadi", yorum.userid);
            return View(yorum);
        }

        // POST: AreaAdmin/Yorumİslemleri/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "yorumid,icerik,onay,resim,userid,konuid,olustarih")] Yorum yorum)
        {
            if (ModelState.IsValid)
            {
                db.Entry(yorum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.konuid = new SelectList(db.Konulars, "konuid", "yorum", yorum.konuid);
            ViewBag.userid = new SelectList(db.Users, "userid", "kadi", yorum.userid);
            return View(yorum);
        }

        // GET: AreaAdmin/Yorumİslemleri/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Yorum yorum = db.Yorums.Find(id);
            if (yorum == null)
            {
                return HttpNotFound();
            }
            return View(yorum);
        }

        // POST: AreaAdmin/Yorumİslemleri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Yorum yorum = db.Yorums.Find(id);
            db.Yorums.Remove(yorum);
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




        public ActionResult Onaybekleyenyorum(string searching, int? page)
        {
            var yorum = from k in db.Yorums
                        select k;
            if (!String.IsNullOrEmpty(searching))
            {

                yorum = yorum.Where(k => k.icerik.Contains(searching));

            }
            var pagenumber = page ?? 1;
            var pageSize = 20;

            var data = db.Yorums.Where(x => x.onay == false).OrderByDescending(p => p.olustarih).ToPagedList(pagenumber, pageSize);
            return View(data);
        }
    }
}
