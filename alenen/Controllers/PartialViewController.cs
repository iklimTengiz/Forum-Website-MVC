using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using alenen.Controllers.Base;

namespace alenen.Controllers
{
    public class PartialViewController : FRControllerBase
    {
        alenenDBEntities db = new alenenDBEntities();
        // GET: PartialView
        public ActionResult SolKategoriMenu()
        {

            var data = db.Kategoris.OrderByDescending(x => x.olustarih).Take(8).ToList();
            return View(data);
        }

        public ActionResult SolKonuMenusu()
        {
            var data = db.AnaKategoris.OrderByDescending(x => x.anakatad).Take(8).ToList();
            return View(data);
        }
 


        public ActionResult ProfilKonular()
        {
            ViewBag.IsLogin = this.IsLogin;

            var data = db.Konulars.Include("User").Where(x => x.User.userid == LoginUserID).Take(10).ToList();
            return View(data);
        }
        public ActionResult Profilyorumlar()
        {
            ViewBag.IsLogin = this.IsLogin;
            var data = db.Yorums.Include("User").Where(x => x.User.userid == LoginUserID).Take(10).ToList();
            return View(data);
        }


        public ActionResult Yorumyap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Yorumyap(FormCollection frm, int id, HttpPostedFileBase imagee,HttpPostedFileBase video)
        {
            Yorum yrm = new Yorum();
          
            if (imagee != null)
            {
                if (Directory.Exists(Server.MapPath("/Content/images/uploads/yorumres")) == false)
                {
                    Directory.CreateDirectory(Server.MapPath("/Content/images/uploads/yorumres"));
                }
                if ((imagee.FileName.Contains("jpg") || imagee.FileName.Contains("jpeg") || imagee.FileName.Contains("png")))
                {
                    imagee.SaveAs(Path.Combine(Server.MapPath("~/Content/images/uploads/yorumres"), imagee.FileName));

                }

            }
           

            string icerik = frm.Get("icerik");  
            int konuid = id;

            if (ModelState.IsValid)
            {
                if (imagee==null)
                {

                }
                else
                {
                    yrm.resim = imagee.FileName;
                }
                yrm.onay = false;
                yrm.userid = LoginUserID;
                yrm.olustarih = DateTime.Now;
                yrm.konuid = id;
                
                yrm.icerik = icerik;
                db.Yorums.Add(yrm);
                db.SaveChanges();
                return RedirectToAction("YorumlarımSayfa", "Profil");
            }

            return View(yrm);

            

        }


        public ActionResult BannerSol()
        {
           
            return View();
        }
     

    }

}

