using CRM.Database;
using CRM.Models;
using CRM.utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public class TableDate<T>
        {
            public int sEcho { get; set; }
            public int iTotalRecords { get; set; }
            public int iTotalDisplayRecords { get; set; }
            public List<T> aaData { get; set; }
        }

        [AuthorizeVerifiedadmin]
        public ActionResult Index()
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            UserViewModel _model = new Models.UserViewModel();
            contactServices _ser = new Database.contactServices();
            _model.labels = _ser.getAllLablesByAdminid(adminId)._labelList;
            if (TempData["Error"] != null)
            {
                _model.Error = TempData["Error"].ToString();
            }
            return View(_model);
        }

        [AuthorizeVerifiedadmin]
        public ActionResult LabelList()
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            UserViewModel _model = new Models.UserViewModel();
            contactServices _ser = new Database.contactServices();
            if (TempData["Error"] != null)
            {
                _model.Error = TempData["Error"].ToString();
            }
            return View(_model);
        }

        [AuthorizeVerifiedadmin]
        public ActionResult labels(int? id)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            Labelitem _model = new Models.Labelitem();
            contactServices _ser = new Database.contactServices();
            if (id.HasValue)
            {
                _model = _ser.getLablesByLabelid(id.Value);
                return View("editLabel", _model);
            }
            return View(_model);
        }

        [AuthorizeVerifiedadmin]
        public ActionResult getContact(int? id)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            tblcontact _contact = new Database.tblcontact();
            contactServices _ser = new Database.contactServices();
            ViewData["labels"] = _ser.getAllLablesByAdminid(adminId);
            if (id.HasValue)
            {
                _contact = _ser.getContactsByContactid(id.Value);
                return View("editcontact", _contact);
            }
            return View(_contact);
        }

        [HttpPost]
        public ActionResult AddContact(tblcontact _model, HttpPostedFileBase file)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            contactServices _ser = new Database.contactServices();
            if (file != null)
            {
                string path = Path.Combine(Server.MapPath("~/assets/images/users"), file.FileName);
                file.SaveAs(path);
                _model.ImagePath = "/assets/images/users/" + file.FileName;
            }
            else
            {
                _model.ImagePath = "/assets/images/icon/staff.png";
            }
            _model.Createddate = DateTime.Now;
            _model.AdminId = adminId;
            _model.IsActive = true;
            var msg = _ser.AddNewContact(_model);
            if (!string.IsNullOrEmpty(msg))
            {
                ViewData["labels"] = _ser.getAllLablesByAdminid(adminId);
                ViewData["Error"] = msg;
                return View("getContact",_model);
            }
            else
            {
                TempData["Error"] = _model.Name + " Created Succesfully";
                return RedirectToAction("index");
            }
        }


        [HttpPost]
        public ActionResult editContact(tblcontact _model, HttpPostedFileBase file)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            contactServices _ser = new Database.contactServices();
            if (file != null)
            {
                string path = Path.Combine(Server.MapPath("~/assets/images/users"), file.FileName);
                file.SaveAs(path);
                _model.ImagePath = "/assets/images/users/" + file.FileName;
            }
            else
            {
                _model.ImagePath = "";
            }
            var msg = _ser.upDateContact(_model);
            if (!string.IsNullOrEmpty(msg))
            {
                ViewData["labels"] = _ser.getAllLablesByAdminid(adminId);
                ViewData["Error"] = msg;
                return View(_model);
            }
            else
            {
                TempData["Error"] = _model.Name + " Updated Succesfully";
                return RedirectToAction("index");
            }
        }


        [HttpPost]
        public ActionResult labels(Labelitem _model)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            contactServices _ser = new Database.contactServices();
            var msg = _ser.addNewLabel(adminId, _model.Name, _model.description);
            if (!string.IsNullOrEmpty(msg.Error))
            {
                _model.Error = msg.Error;
                return View(_model);
            }
            else
            {
                TempData["Error"] = _model.Name + " Created Succesfully";
                return RedirectToAction("labellist");
            }
        }

        [HttpPost]
        public ActionResult editlabels(Labelitem _model)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            contactServices _ser = new Database.contactServices();
            var msg = _ser.updateLabelById(_model.LabelId, _model.Active, adminId, _model.Name, _model.description);
            if (!string.IsNullOrEmpty(msg.Error))
            {
                _model.Error = msg.Error;
                return View("editLabel", _model);
            }
            else
            {
                TempData["Error"] = _model.Name + " Updated Succesfully";
                return RedirectToAction("Labellist");
            }
        }

        [HttpPost]
        public void deleteContact(int id)
        { 
            contactServices _ser = new Database.contactServices();
            _ser.deleteContact(id);
        }

        [HttpPost]
        public void deletelabel(int id)
        { 
            contactServices _ser = new Database.contactServices();
            _ser.deletelabel(id);
        }

        public JsonResult Fetchcontacts(int iDisplayStart = 0, int iDisplayLength = 0, int sEcho = 1, string sSearch = null, int iSortCol_0 = 0, string sSortDir_0 = "asc")
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            TableDate<tblcontact> tableDate = new TableDate<tblcontact>();
            List<string[]> list = new List<string[]>();
            int recordsTotal = 0;
            Global _global = new Database.Global();
            using (CRMClassDataContext dataClasses1DataContext = new CRMClassDataContext(_global.con))
            {
                List<tblcontact> list2 = (from a in dataClasses1DataContext.tblcontacts.Where(a => a.AdminId == adminId && a.IsActive == true)
                                          select a).ToList<tblcontact>();
                recordsTotal = list2.Count<tblcontact>();
                if (!string.IsNullOrEmpty(sSearch))
                {
                    list2 = list2.Where(a => a.Name.Contains(sSearch) || a.email.Contains(sSearch) || a.phone.Contains(sSearch)).ToList<tblcontact>();
                }
                string sortby = "mDataProp_" + iSortCol_0;
                sortby = Request.QueryString[sortby].ToString();
                list2 = SortList<tblcontact>(list2, sSortDir_0, sortby).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                tableDate.sEcho = sEcho;
                tableDate.aaData = list2;
                tableDate.iTotalRecords = list2.Count<tblcontact>();
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

        public JsonResult Fetchlabels(int iDisplayStart = 0, int iDisplayLength = 0, int sEcho = 1, string sSearch = null, int iSortCol_0 = 0, string sSortDir_0 = "asc")
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            TableDate<tbllabel> tableDate = new TableDate<tbllabel>();
            List<string[]> list = new List<string[]>();
            int recordsTotal = 0;
            Global _global = new Database.Global();
            using (CRMClassDataContext dataClasses1DataContext = new CRMClassDataContext(_global.con))
            {
                List<tbllabel> list2 = (from a in dataClasses1DataContext.tbllabels.Where(a => a.AdminId == adminId && a.Isactive == true)
                                        select a).ToList<tbllabel>();
                recordsTotal = list2.Count<tbllabel>();
                if (!string.IsNullOrEmpty(sSearch))
                {
                    list2 = list2.Where(a => a.LableName.Contains(sSearch) || a.Description.Contains(sSearch)).ToList<tbllabel>();
                }
                string sortby = "mDataProp_" + iSortCol_0;
                sortby = Request.QueryString[sortby].ToString();
                list2 = SortList<tbllabel>(list2, sSortDir_0, sortby).Skip(iDisplayStart).Take(iDisplayLength).ToList();
                tableDate.sEcho = sEcho;
                tableDate.aaData = list2;
                tableDate.iTotalRecords = list2.Count<tbllabel>();
                tableDate.iTotalDisplayRecords = recordsTotal;
            }
            return base.Json(tableDate, JsonRequestBehavior.AllowGet);
        } 
    }
}