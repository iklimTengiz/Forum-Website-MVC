using alenen.Controllers.Base;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace alenen.Controllers
{
    public class BegenilerController : FRControllerBase
    {
        alenenDBEntities db = new alenenDBEntities();
        [Route("~/Begeniler")]
        public ActionResult Index(int? page)
        {
            var pagenumber = page ?? 1;
            var pageSize = 15;
            var data = db.Begenis.Include("Yorum").OrderBy(p => p.yorumid).Where(x => x.userid == LoginUserID).ToPagedList(pagenumber, pageSize);

            return View(data);

        }


        public ActionResult Sil(int id)
        {
            var begenisil = db.Begenis.Where(x => x.begeniid == id).FirstOrDefault();
            db.Begenis.Remove(begenisil);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult begeniEkle(int ktpid, int adet)
        {
            db.Begenis.Add(new alenen.Begeni
            {
                yorumid = ktpid,
                userid = LoginUserID,
               
            });

            var rt = db.SaveChanges();

            return Json(rt, JsonRequestBehavior.AllowGet);
        }

    }
}