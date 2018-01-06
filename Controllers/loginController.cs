using CRM.Database;
using CRM.Models;
using CRM.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class loginController : Controller
    {
        // GET: login
        public ActionResult Index(string ReturnUrl="abc")
        {
            LoginViewModel _model = new LoginViewModel();
            Session.Abandon();
            _model.ReturnUrl = ReturnUrl;
            _model.display = "login";
            return View(_model);
        }
        [AuthorizeVerifiedadmin]
        public ActionResult Dashboard()
        {
            int adminId =Convert.ToInt16(Session["AdminId"]);
            DashboardViewModel _model = new Models.DashboardViewModel();
            jobServices _ser = new Database.jobServices();
            _model = _ser.getAllTodayjobs(adminId);
            return View(_model); 
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel _model)
        {
            loginServices _ser = new loginServices();
            _model.display = "login";
            var userId = _ser.Validatelogin(_model.Email, _model.Password);
            if (userId == 0)
            {
                _model.Error = "Wrong username or password";
                return View("index", _model);
            }
            else
            {
                Session["AdminId"] = userId;
                if (_model.ReturnUrl != "abc")
                {
                    return Redirect(_model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Dashboard", "login");
                }
            }
        }


        [HttpPost]
        public ActionResult Register(LoginViewModel _model)
        {
            _model.display = "register";
            loginServices _ser = new loginServices();
            var userId = _ser.Validateregister(_model);
            if (userId == 0)
            {
                _model.Error = "Email Addess Or Phone Number already exist";
                return View("index", _model);
            }
            else
            {
                Session["AdminId"] = userId;
                return RedirectToAction("Dashboard", "login");
            }
        }
    }
}