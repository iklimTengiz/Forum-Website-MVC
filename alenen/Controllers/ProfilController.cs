using alenen.Controllers.Base;
using alenen.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace alenen.Controllers
{
    public class ProfilController : FRControllerBase
    {
        alenenDBEntities db = new alenenDBEntities();


        [Route("~/Profilim")]
        public ActionResult Profil()
            {
                ViewBag.IsLogin = this.IsLogin;
            
            return View();
            }
        


        [Route("~/Yorumlarım")]
        public ActionResult YorumlarımSayfa(int? page)
        {
            var pagenumber = page ?? 1;
            var pageSize = 15;
            ViewBag.IsLogin = this.IsLogin;
            var data = db.Yorums.OrderBy(p => p.olustarih).Where(x => x.User.userid == LoginUserID).ToPagedList(pagenumber, pageSize);
            return View(data);
        }


        [Route("~/Konularım")]
        public ActionResult KonularımSayfa(int? page)
        {
            var pagenumber = page ?? 1;
            var pageSize = 15;
            ViewBag.IsLogin = this.IsLogin;
            var data = db.Konulars.OrderBy(p => p.olustarih).Where(x => x.olusturanid == LoginUserID).Take(10).ToPagedList(pagenumber, pageSize);
            return View(data);
        }



        [Route("Begenilenler")]
        public ActionResult Index(int? page)
        {
            var pagenumber = page ?? 1;
            var pageSize = 15;
            var data = db.Begenis.Include("Yorumlar").OrderBy(p => p.yorumid).Where(x => x.userid == LoginUserID).Take(10).ToPagedList(pagenumber, pageSize);

            return View(data);

        }


        // GET: Users/Edit/5
        [Route("~/editu")]
        public async Task<ActionResult> EditProfil(int? id)
        {
            ViewBag.IsLogin = this.IsLogin;
            id = LoginUserID;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.cinsiyetid = new SelectList(db.Cinsiyets, "cinsiyetid", "cinsiyetad", user.cinsiyetid);
            return View(user);
        }

        [Route("~/editu")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditProfil(User user, HttpPostedFileBase imagee)
        {

            if (imagee != null)
            {
                if (Directory.Exists(Server.MapPath("/Content/images/uploads/kullaniciresim")) == false)
                {
                    Directory.CreateDirectory(Server.MapPath("/Content/images/uploads/kullaniciresim"));
                }
                if ((imagee.FileName.Contains("jpg") || imagee.FileName.Contains("jpeg") ||imagee.FileName.Contains("gif") || imagee.FileName.Contains("png")))
                {
                    imagee.SaveAs(Path.Combine(Server.MapPath("~/Content/images/uploads/kullaniciresim"), imagee.FileName));

                }
                
            }
            if (ModelState.IsValid)
            {
                if (imagee == null)
                {

                }
                else
                {
                    user.resim = imagee.FileName;
                }
               
                user.olusumtarih = user.olusumtarih;




                db.Entry(user).State = EntityState.Modified;
                
                await db.SaveChangesAsync();
                /////////////Sesion içerisinde güncelleme işlemini anında yedirme
                Session["LoginUserID"] = user.userid;
                Session["LoginUser"] = user;
                /////////////Sesion içerisinde güncelleme işlemini anında yedirme
                return RedirectToAction("Index", "Home");

            }
            ViewBag.cinsiyetid = new SelectList(db.Cinsiyets, "cinsiyetid", "cinsiyetad", user.cinsiyetid);
            return View(user);
        }



        public ActionResult yorumSil(int id)
        {
            var yorumsil = db.Yorums.Where(x => x.yorumid == id).FirstOrDefault();
            db.Yorums.Remove(yorumsil);
            db.SaveChanges();

            return RedirectToAction("Profil");
        }





    }
}