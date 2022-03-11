using alenen.Controllers.Base;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Optimization;
using System.Web;
using System.Web.Mvc;
using alenen.ViewModels;
using System.Web.Script.Serialization;
using ASPSnippets.FaceBookAPI;

namespace alenen.Controllers
{
    public class HomeController : FRControllerBase
    {
        alenenDBEntities db = new alenenDBEntities();

        private MailMessage mail;
        private SmtpClient smp;
        // GET: Home


        /// <summary>
        /// ANASAYFA
        /// </summary>

        [Route("~/")]
        public ActionResult Index()
        {
            ViewBag.kategoriid = new SelectList(db.Kategoris, "kategoriid", "kategoriad");

            return View();
        }
        [Route("~/")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([Bind(Include = "konuid,yorum,konuad,kategoriid,resim,olustarih,olusturanid")] Konular konular)
        {
            if (ModelState.IsValid)
            {
                konular.olusturanid = LoginUserID;
                konular.olustarih = DateTime.Now;


                db.Konulars.Add(konular);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            ViewBag.kategoriid = new SelectList(db.Kategoris, "kategoriid", "kategoriad", konular.kategoriid);

            return View(konular);
        }


        /// <summary>
        /// KURALLAR
        /// </summary>

        [Route("~/kurallar")]
        public ActionResult kurallar()
        {
            return View();
        }



        /// <summary>
        /// İLETİŞİM
        /// </summary>
        [Route("~/iletisim")]
        public ActionResult iletisim()
        {
            return View();
        }

        [Route("~/iletisim")]
        [HttpPost]
        public ActionResult iletisim(string name, string email, string subject, string mesaj)
        {
            try
            {
                TempData["mesaj"] = "Sayın : " + name + " mesajınız başarıyla alındı. Sizinle en yakın zamanda iletişime geçeceğiz.";
                string subject2 = "İletişim : " + name;
                string body = "<b>Adı Soyadı : </b>" + name +
                    "<br><b>E-Posta : </b>" + email +
                    "<br><b>Mesaj : </b>" + mesaj;
                mail = new MailMessage(); //yeni bir mail nesnesi Oluşturuldu.
                mail.IsBodyHtml = true; //mail içeriğinde html etiketleri kullanılsın mı?
                mail.To.Add("MAIL"); //Kime mail gönderilecek.
                mail.From = new MailAddress("MAİL", "YOUR WEB SİTE DOMAİN", System.Text.Encoding.UTF8);
                mail.Subject = subject2;//mailin konusu
                                        //mailin içeriği.. Bu alan isteğe göre genişletilip daraltılabilir.
                mail.Body = body;
                mail.IsBodyHtml = true;

                smp = new SmtpClient();
                //mailin gönderileceği adres ve şifresi
                smp.Credentials = new NetworkCredential("MAIL", "PASSWORD");
                smp.Port = 587;
                smp.Host = "smtp.gmail.com";//gmail üzerinden gönderiliyor.
                smp.EnableSsl = true;
                smp.Send(mail);//mail isimli mail gönderiliyor.
            }
            catch (Exception)
            {
                TempData["mesaj"] = "HATA ! : Bilgileri doğru ve eksiksiz girdiğinizden emin olun !";
                throw;
            }
            return View();
        }







        /// <summary>
        ///  Anasafa menüde ki Ana KAtegori LİSTELEME SAYFASI   Pa
        /// </summary>

        [Route("~/anakategoriler")]
        public ActionResult anakategorilist(int? page)
        {
            var pagenumber = page ?? 1;
            var pageSize = 12;
            var anak = db.AnaKategoris.OrderBy(v => v.anakatid).OrderByDescending(x => x.anakatad).ToPagedList(pagenumber, pageSize);
            return View(anak);
        }



        /// <summary>
        /// KATEGORİ LİSTELEME SAYFASI   Pa
        /// </summary>

        [Route("~/kategoriler")]
        public ActionResult kategorilist(int? page)
        {
            var pagenumber = page ?? 1;
            var pageSize = 12;
            var kategori = db.Kategoris.OrderBy(v => v.kategoriid).OrderByDescending(x => x.olustarih).ToPagedList(pagenumber, pageSize);
            return View(kategori);
        }




        /// <summary>
        /// KONU LİSTELEME SAYFASI
        /// </summary>
        [Route("~/konular")]
        public ActionResult konulist(int? page)
        {
            var pagenumber = page ?? 1;
            var pageSize = 12;
            var konular = db.Konulars.OrderBy(v => v.kategoriid).OrderByDescending(x => x.olustarih).Take(30).ToPagedList(pagenumber, pageSize);
            return View(konular);

        }



        /// <summary>
        /// ÜYE GİRİŞİ
        /// </summary>

        [Route("~/uyegiris")]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("~/uyegiris")]
        public ActionResult Login(string kadi, string sifre)
        {
            var users = db.Users.Where(x => x.kadi == kadi
                  && x.sifre == sifre && x.aktifmi == true).ToList();
            if (users.Count == 1)
            {
                Session["LoginUserID"] = users.FirstOrDefault().userid;
                Session["LoginUser"] = users.FirstOrDefault();
                return Redirect("/");
            }
            else
            {
                ViewBag.Error = "Hatalı mail adresi yada şifre !";
                return View();
            }


        }


        /// <summary>
        /// ÜYE ÇIKIŞI
        /// </summary>

        [Route("~/LogOut")]
        public ActionResult LogOut()
        {
            Session.Abandon();

            return RedirectToAction("Index", "Home");
        }





        /// <summary>
        /// ÜYE OLMA
        /// </summary>
        /// 
        // GET: Registration
        [Route("~/UyeOl")]
        public ActionResult Registration()
        {
            ViewBag.cinsiyetid = new SelectList(db.Cinsiyets, "cinsiyetid", "cinsiyetad");
            return View();
        }

        [Route("~/UyeOl")]
        [HttpPost]
        public ActionResult Registration(User user)
        {

            alenenDBEntities usersEntities = new alenenDBEntities();
            user.olusumtarih = DateTime.Now;
            user.aktifmi = false;
            user.adminmi = false;
            user.moderatormu = false;
            user.sozlesme = true;
            user.ResetPasswordCode = null;

            if (user.cinsiyetid == 1)
            {
                user.resim = "Kız.png";
                if (user.cinsiyetid == 2)
                {
                    user.resim = "Erkek.png";
                }
            }
            else
            {
                user.resim = "bistemiyorum.png";
            }

            usersEntities.Users.Add(user);
            usersEntities.SaveChanges();
            string message = string.Empty;
            switch (user.userid)
            {

                case -1:

                    byte[] utf8Bytes = System.Text.Encoding.UTF8.GetBytes(message);
                    message = "Kullanıcı adı mevcut başka bir isim tercih ediniz.";
                    break;
                case -2:
                    message = "Mail Adresi  Kullanımda";
                    break;
                default:
                    message = "Kayıt başarılı , Hesabınızı aktive edebilmek için mail adresinize gelen kodu onaylayınız.";
                    SendActivationEmail(user);
                    break;
            }
            ViewBag.Message = message;
            ViewBag.cinsiyetid = new SelectList(db.Cinsiyets, "cinsiyetid", "cinsiyetad", user.cinsiyetid);
            return View(user);
        }

        public ActionResult Activation()
        {
            ViewBag.Message = "Geçersiz Aktivasyon Kodu";
            if (RouteData.Values["id"] != null)
            {
                Guid activationCode = new Guid(RouteData.Values["id"].ToString());
                alenenDBEntities usersEntities = new alenenDBEntities();
                UserActivation userActivation = usersEntities.UserActivations.Where(p => p.ActivationCode == activationCode).FirstOrDefault();
                if (userActivation != null)
                {
                    usersEntities.UserActivations.Remove(userActivation);
                    usersEntities.SaveChanges();
                    ViewBag.Message = "Hesabınızı aktive edilmiştir.Bilgileriniz adminlerimiz tarafından incelenilerek kullanıma açılacaktır.";
                }
            }

            return View();
        }

        private void SendActivationEmail(User user)
        {
            Guid activationCode = Guid.NewGuid();
            alenenDBEntities usersEntities = new alenenDBEntities();
            usersEntities.UserActivations.Add(new UserActivation
            {
                userid = user.userid,
                ActivationCode = activationCode
            });
            usersEntities.SaveChanges();

            using (MailMessage mm = new MailMessage("iletform@gmail.com", user.mail))
            {
                mm.Subject = "Alenen.com Aktivasyon Maili";
                string body = "Hello " + user.kadi + ",";
                body += "<br /><br />Hesabınızı aktive etmek için lütfen aşagıdaki linke tıklayınız";
                body += "<br /><a href = '" + string.Format("{0}://{1}/Home/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Aktivasyon için buraya tıklayınız.</a>";
                body += "<br /><br />Thanks";
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("iletform@gmail.com", "sifre123*");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }

        /*AKTİVASTON BURADA BİTİYOR*/

        /*ŞİFREMİ UNUTTUM*/
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string resetCode = Guid.NewGuid().ToString();

            var verifyUrl = "/Home/ResetPassword/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            using (var db = new alenenDBEntities())
            {
                var getUser = (from s in db.Users where s.mail == EmailID select s).FirstOrDefault();
                if (getUser != null)
                {
                    getUser.ResetPasswordCode = resetCode;

                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 

                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();

                    var subject = "Alenen.com Şifre Sıfırlama";
                    var body = "Merhaba " + getUser.kadi + ", <br/> Şifrenizi Sıfırlamk için linke tıklayınız. " +

                         " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                         "Yeni şifre talp etmediyseniz dikkate almayınız<br/><br/> Sevgiler";

                    SendEmail(getUser.mail, body, subject);

                    ViewBag.Message = "Sıfırlama maili mail adresinize gönderilmiştir.";
                }
                else
                {
                    ViewBag.Message = "Kullanıcı Bulunamadı";
                    return View();
                }
            }

            return View();
        }

        private void SendEmail(string emailAddress, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("iletform@gmail.com", emailAddress))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("iletform@gmail.com", "sifre123*");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);

            }
        }





        /*Şİfre değiştirme ekranı*/
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (var context = new alenenDBEntities())
            {
                var user = context.Users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (var context = new alenenDBEntities())
                {
                    var user = context.Users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        //you can encrypt password here, we are not doing it
                        user.sifre = model.NewPassword;
                        //make resetpasswordcode empty string now
                        user.ResetPasswordCode = "";
                        //to avoid validation issues, disable it
                        context.Configuration.ValidateOnSaveEnabled = false;
                        context.SaveChanges();
                        message = "Şifreniz Başarıyla yenilendi.";
                    }
                }
            }
            else
            {
                message = "Yanlış giden bişeyler oldu.";
            }
            ViewBag.Message = message;
            return View(model);
        }



        /*ŞİFRE SIFIRLAMA BURADA BİTİYOR*/





        /// <summary>
        /// KONU ARAMA
        /// </summary>

        public ActionResult KonuArama(string searching, int? page)
        {

            var konular = from k in db.Konulars
                          select k;
            if (!String.IsNullOrEmpty(searching))
            {

                konular = konular.Where(k => k.konuad.Contains(searching));

            }
            var pagenumber = page ?? 1;
            var pageSize = 20;
            return View(konular.OrderBy(v => v.konuad).ToPagedList(pagenumber, pageSize));
        }




        /// <summary>
        /// ANA KATEGORİDEN Kategori LİSTELEME Anasayfa menü iç sayfası
        /// </summary>
        [Route("AnaKategori/{isim}/{id}")]
        public ActionResult anakategorikategori(string isim, int id, int? page)
        {

            var pagenumber = page ?? 1;
            var pageSize = 15;
            var data = db.Kategoris.Include("AnaKategori").Where(x => x.anakatid == id).OrderBy(v => v.kategoriad).ToPagedList(pagenumber, pageSize);
            ViewBag.anak = db.AnaKategoris.Where(x => x.anakatid == id).FirstOrDefault();
            return View(data);
        }



        /// <summary>
        /// KATEGORİDEN KONU LİSTELEME
        /// </summary>
        [Route("Kategori/{isim}/{id}")]
        public ActionResult kategorikonulari(string isim, int id, int? page)
        {

            var pagenumber = page ?? 1;
            var pageSize = 15;
            var data = db.Konulars.Include("Kategori").Where(x => x.kategoriid == id).OrderBy(v => v.konuad).ToPagedList(pagenumber, pageSize);
            ViewBag.kategori = db.Kategoris.Where(x => x.kategoriid == id).FirstOrDefault();
            return View(data);
        }



        /// <summary>
        /// KATEGORİDEN KONU LİSTELEME
        /// </summary>
        [Route("Konular/{isim}/{id}")]
        public ActionResult konuyorumları(string isim, int id, int? page)
        {
            ViewBag.IsLogin = this.IsLogin;
            var pagenumber = page ?? 1;
            var pageSize = 15;
            var data = db.Yorums.Include("Konular").Where(x => x.konuid == id).OrderByDescending(x => x.olustarih).ToPagedList(pagenumber, pageSize);
            ViewBag.konular = db.Konulars.Where(x => x.konuid == id).FirstOrDefault();

            return View(data);
        }

        public ActionResult AnasayfaKonular()
        {

            var data = db.Konulars.OrderByDescending(x => x.olustarih).Take(15).ToList();
            return View(data);
        }

        [Route("uyeliksozlesme")]
        public ActionResult uyeliksozlesme()
        {


            return View();
        }
        [Route("aydinlatmametni")]
        public ActionResult aydinlatmametni()
        {


            return View();
        }
        [Route("rizametni")]
        public ActionResult rizametni()
        {


            return View();
        }
        [Route("cerezpolitikasi")]
        public ActionResult cerezpolitikası()
        {


            return View();
        }






        // FacebookRegister
        public ActionResult FacebookRegister()
        {
            FaceBookConnect.API_Key = "1063721667328764";
            FaceBookConnect.API_Secret = "e6044167b1b5878261f0f9c79a7108db";

            FaceBookUser faceBookUser = new FaceBookUser();
            if (Request.QueryString["error"] == "access_denied")
            {
                ViewBag.Message = "Erişim Reddedildi";
                return RedirectToAction("Login","Home");
            }
            else
            {
                string code = Request.QueryString["code"];
                if (!string.IsNullOrEmpty(code))
                {
                    string data = FaceBookConnect.Fetch(code, "me?fields=id,name,email");
                    faceBookUser = new JavaScriptSerializer().Deserialize<FaceBookUser>(data);

                }
            }

            return View(faceBookUser);
        }
        // FacebookRegister
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FacebookRegister(User usr, FaceBookUser fb)
        {
            
            var users = db.Users.Where(x => x.mail == fb.Email).ToList();

            if (users.Count > 0)
            {

                ViewBag.Error = "Bu mail adresi ile bir hesap sitemimizde zaten bulunmakta.";
                
                return View();


            }
            var usersb = db.Users.Where(x => x.kadi == fb.kadi).ToList();
            if (usersb.Count > 0)
            {

                ViewBag.Errorb = "Bu kullanıcı adı kullanılmakta.";
                
                return View();


            }
      

            else
            {

                usr.isim = fb.Name;
                usr.resim = "bistemiyorum.png";
                usr.mail = fb.Email;
                usr.kadi = fb.kadi;
                usr.sifre = fb.sifre;
                usr.olusumtarih = DateTime.Now;
                usr.moderatormu = false;
                usr.adminmi = false;
                usr.aktifmi = true;
                usr.cinsiyetid = 3;
                usr.sozlesme = true;
                usr.ResetPasswordCode = null;
                usr.telefon = null;

                db.Users.Add(usr);
                db.SaveChanges();
                return RedirectToAction("kayitoke", "Home");
            }




        }
        [HttpPost]
        public EmptyResult FacebookLogin()
        {
            FaceBookConnect.Authorize("email", string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, "Home/FacebookRegister/"));
            return new EmptyResult();
        }

        // FacebookRegister


        public ActionResult kayitoke()
        {
            return View();
        }
    }
}