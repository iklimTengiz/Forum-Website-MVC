using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace alenen.Areas.AreaAdmin.Controllers
{
    public class AdminLoginController : AdminControllerBase
    {
        alenenDBEntities db = new alenenDBEntities();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Index(string Maik, string Sifre)
        {

            var data = db.Users.Where(x => x.kadi == Maik && x.sifre == Sifre & x.aktifmi == true & x.moderatormu == true).ToList();
            if (data.Count == 1)
            {
                Session["LoginAdminUser"] = data.FirstOrDefault();
                return Redirect("/AreaAdmin/Default");
            }
            else
            {
                return View();
            }

        }


        public ActionResult LogOut()
        {
            Session.Clear();

            return RedirectToAction("AdminLogin", "Index");
        }
    }
}