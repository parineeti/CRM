using CRM.Database;
using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public class TableDate<T>
        {
            public int sEcho { get; set; }
            public int iTotalRecords { get; set; }
            public int iTotalDisplayRecords { get; set; }
            public List<T> aaData { get; set; }
        }
        public ActionResult Index()
        {
            UserViewModel _model = new UserViewModel();
            return View(_model);
        }

        public JsonResult Fetchusers(int iDisplayStart = 0, int iDisplayLength = 0, int sEcho = 1, string sSearch = null, int iSortCol_0 = 0, string sSortDir_0 = "asc")
        {
            int adminId = 1;
            TableDate<tbllogin> tableDate = new TableDate<tbllogin>();
            List<string[]> list = new List<string[]>();
            int recordsTotal = 0;
            Global _global = new Database.Global();
            using (CRMClassDataContext dataClasses1DataContext = new CRMClassDataContext(_global.con))
            {
                List<tbllogin> list2 = (from a in dataClasses1DataContext.tbllogins.Where(a=>a.IsActive=="Y")
                                          select a).ToList<tbllogin>();
                recordsTotal = list2.Count<tbllogin>();
                if (!string.IsNullOrEmpty(sSearch))
                {
                    list2 = list2.Where(a => a.FirstName.Contains(sSearch) || a.Email.Contains(sSearch) || a.PhoneNumber.Contains(sSearch)).ToList<tbllogin>();
                }
                string sortby = "mDataProp_" + iSortCol_0;
                sortby = Request.QueryString[sortby].ToString();
                list2 = SortList<tbllogin>(list2, sSortDir_0, sortby).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                tableDate.sEcho = sEcho;
                tableDate.aaData = list2;
                tableDate.iTotalRecords = list2.Count<tbllogin>();
                tableDate.iTotalDisplayRecords = recordsTotal;
            }
            return base.Json(tableDate, JsonRequestBehavior.AllowGet);
        }
    
        private List<T> SortList<T>(List<T> collection, string order, string propertyName) where T : class
        {
            if (order != "asc")
            {
                return collection.OrderByDescending(cc => cc.GetType().GetProperty(propertyName).GetValue(cc, null)).ToList();
            }
            return collection.OrderBy(cc => cc.GetType().GetProperty(propertyName).GetValue(cc, null)).ToList();
        }

        public  void deleteuser(int id)
        {
            loginServices _login = new Database.loginServices();
            _login.Deletuser(id);
        } 

        public ActionResult Adduser()
        {
            LoginViewModel _model = new Models.LoginViewModel();
            return View(_model);
        } 
    }
}