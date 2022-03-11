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
    public class UsersController : AdminControllerBase
    {
        private alenenDBEntities db = new alenenDBEntities();

        // GET: AreaAdmin/Users
        public ActionResult Index(string searching, int?page)
        {
            var usr = from k in db.Users
                           select k;
            if (!String.IsNullOrEmpty(searching))
            {

                usr = usr.Where(k => k.kadi.Contains(searching));

            }
            var pagenumber = page ?? 1;
            var pageSize = 15;
            var users = db.Users.Include(u => u.Cinsiyet);
            return View(usr.OrderBy(p => p.userid).ToPagedList(pagenumber, pageSize));
        }

        // GET: AreaAdmin/Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: AreaAdmin/Users/Create
        public ActionResult Create()
        {
            ViewBag.cinsiyetid = new SelectList(db.Cinsiyets, "cinsiyetid", "cinsiyetad");
            return View();
        }

        // POST: AreaAdmin/Users/Create
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userid,mail,kadi,sifre,aktifmi,adminmi,resim,sozlesme,olusumtarih,isim,soyisim,telefon,cinsiyetid")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cinsiyetid = new SelectList(db.Cinsiyets, "cinsiyetid", "cinsiyetad", user.cinsiyetid);
            return View(user);
        }

        // GET: AreaAdmin/Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.cinsiyetid = new SelectList(db.Cinsiyets, "cinsiyetid", "cinsiyetad", user.cinsiyetid);
            return View(user);
        }

        // POST: AreaAdmin/Users/Edit/5
        // Aşırı gönderim saldırılarından korunmak için, lütfen bağlamak istediğiniz belirli özellikleri etkinleştirin, 
        // daha fazla bilgi için https://go.microsoft.com/fwlink/?LinkId=317598 sayfasına bakın.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userid,mail,kadi,moderatormu,sifre,aktifmi,adminmi,resim,sozlesme,olusumtarih,isim,soyisim,telefon,cinsiyetid")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cinsiyetid = new SelectList(db.Cinsiyets, "cinsiyetid", "cinsiyetad", user.cinsiyetid);
            return View(user);
        }

        // GET: AreaAdmin/Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: AreaAdmin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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



  
        public ActionResult Onaybekleyenkullanici(string searching, int? page)
        {
            var usr = from k in db.Users
                      select k;
            if (!String.IsNullOrEmpty(searching))
            {

                usr = usr.Where(k => k.kadi.Contains(searching));

            }
            var pagenumber = page ?? 1;
            var pageSize = 20;
            var users = db.Users.Include(u => u.Cinsiyet);
            var data = db.Users.Where(x => x.aktifmi == false).OrderByDescending(p => p.olusumtarih).ToPagedList(pagenumber, pageSize);
            return View(data);
        }


    }
}
