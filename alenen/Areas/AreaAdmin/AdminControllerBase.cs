using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace alenen.Areas.AreaAdmin
{
    public class AdminControllerBase : Controller
    {
        /// <summary>
        /// Kullanıcı Login Kontrolü
        /// </summary>
        public bool IsLogin { get; private set; }
        /// <summary>
        /// Giriş yapan kişi ID
        /// </summary>
        public int LoginUserID { get; private set; }

        /// <summary>
        /// Login User Entity
        /// </summary>
        public User LoginUSerEntity { get; private set; }




        // Session["LoginUserID"]
        // Session["LoginUser"]

        protected override void Initialize(RequestContext requestContext)
        {
            if (requestContext.HttpContext.Session["LoginUserID"] != null)
            {
                ///giriş yapılmışise
                IsLogin = true;
                LoginUserID = (int)requestContext.HttpContext.Session["LoginAdminUserID"];
                LoginUSerEntity = (alenen.User)requestContext.HttpContext.Session["LoginAdminUser"];

            }
            base.Initialize(requestContext);
        }
    }
}