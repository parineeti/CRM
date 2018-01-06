using CRM.Database;
using CRM.utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult index()
        {
            return View();
        }

        [AuthorizeVerifiedadmin]
        public ActionResult MyProfile()
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            userServices _ser = new userServices();
            var _model = _ser.GetLoginUser(adminId);
            return View(_model); 
        }

        [HttpPost]
        public ActionResult MyProfile(tbllogin user,HttpPostedFileBase file)
        {
            string Image = string.Empty;
               userServices _ser = new userServices();
            if(file!=null)
            {  string path = Path.Combine(Server.MapPath("~/assets/images/users"), file.FileName);
                file.SaveAs(path);
                Image = "/assets/images/users/" + file.FileName;
                user.Image = Image;
                Session["image"] = Image;
            }
            var msg = _ser.UpdateProfile(user, Image);
            user = _ser.GetLoginUser(user.UserId);
            ViewData["msg"] = msg;
            return View(user);
        }

    }
}