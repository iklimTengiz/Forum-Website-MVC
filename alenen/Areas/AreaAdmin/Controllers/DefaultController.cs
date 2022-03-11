using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace alenen.Areas.AreaAdmin.Controllers
{
    public class DefaultController : AdminControllerBase
    {
        alenenDBEntities db = new alenenDBEntities();
        // GET: AreaAdmin/Default
        public ActionResult Index()
        {
            var usrcount = db.Users.Where(x => x.aktifmi == true).Count();
            ViewBag.count = usrcount;

            var onaykullanici = db.Users.Where(x => x.aktifmi == false).Count();
            ViewBag.countuseronay = onaykullanici;

            var onayyorum = db.Yorums.Where(x => x.onay == false).Count();
            ViewBag.countyorumonay = onayyorum;
            /*Buraya kadar ilk sıranın Count İşlemleriii*/


            /*İKİNCİ SATIR BİRİNDİ DİV*/
            DateTime gun = DateTime.Now.Date;
            var uyegun = db.Users.Where(x => x.olusumtarih == gun).Count();
            ViewBag.uyegun = uyegun;



            DateTime hafta = DateTime.Now.Date.AddDays(-7);
            var uyehafta = db.Users.Where(x => x.olusumtarih >= hafta).Count();
            ViewBag.uyehafta = uyehafta;

            DateTime ay = DateTime.Now.Date.AddDays(-30);
            var uyeay = db.Users.Where(x => x.olusumtarih >= ay).Count();
            ViewBag.uyeay = uyeay;

            DateTime yıl = DateTime.Now.Date.AddDays(-365);
            var uyeyıl = db.Users.Where(x => x.olusumtarih >= yıl).Count();
            ViewBag.uyeyıl = uyeyıl;




            return View();
        }

    
    }
}