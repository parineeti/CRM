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
    public class jobController : Controller
    {
        // GET: job
        [AuthorizeVerifiedadmin]
        public ActionResult statusList()
        {
            StatusViewModel _model = new Models.StatusViewModel();
            jobServices _services = new jobServices();
            _model = _services.getAllStatus();
            if (TempData["error"] != null)
            {
                _model.Error = TempData["error"].ToString();
            }
            return View(_model);
        }

        [AuthorizeVerifiedadmin]
        public ActionResult status(int? id)
        {
            tblstatus _model = new tblstatus();
            jobServices _services = new jobServices();
            if (id.HasValue)
            {
                _model = _services.GetStatusbyId(id.Value);
                return View("editStatus", _model);
            }
            if (TempData["error"] != null)
            {
                ViewData["error"] = TempData["error"].ToString();
            }
            return View(_model);
        }

        [HttpPost]
        public ActionResult status(tblstatus model)
        {
            jobServices _services = new jobServices();
            var msg = _services.AddNewStatus(model.StatusName, model.Color);
            if (msg.Contains("Error"))
            {
                ViewData["error"] = msg;
                return View(model);
            }
            else
            {
                TempData["error"] = msg;
                return RedirectToAction("statusList");
            }
        }

        [HttpPost]
        public ActionResult editstatus(tblstatus model)
        {
            jobServices _services = new jobServices();
            var msg = _services.UpdateStatus(model.Color, model.StatusName, model.Id);
            if (msg.Contains("Error"))
            {
                ViewData["error"] = msg;
                return View(model);
            }
            else
            {
                TempData["error"] = msg;
                return RedirectToAction("statusList");
            }
        }

        [AuthorizeVerifiedadmin]
        public ActionResult JobTypeList()
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            jobtypeViewModal _model = new jobtypeViewModal();
            jobServices _services = new jobServices();
            _model = _services.getAllJobtypes(adminId);
            if (TempData["error"] != null)
            {
                _model.Error = TempData["error"].ToString();
            }
            return View(_model);
        }

        [AuthorizeVerifiedadmin]
        public ActionResult jobType(int? id)
        {
            tbljobType _model = new tbljobType();
            jobServices _services = new jobServices();
            if (id.HasValue)
            {
                _model = _services.GetjobtypebyId(id.Value);
                return View("edittype", _model);
            }
            if (TempData["error"] != null)
            {
                ViewData["error"] = TempData["error"].ToString();
            }
            return View(_model);
        }

        [HttpPost]
        public ActionResult jobType(tbljobType model)
        {
            int adminid = Convert.ToInt16(Session["AdminId"]);
            jobServices _services = new jobServices();
            var msg = _services.AddNewjobtype(model.Description, model.Name, adminid);
            if (msg.Contains("Error"))
            {
                ViewData["error"] = msg;
                return View(model);
            }
            else
            {
                TempData["error"] = msg;
                return RedirectToAction("JobTypeList");
            }
        }

        [HttpPost]
        public ActionResult edittype(tbljobType model)
        {
            jobServices _services = new jobServices();
            var msg = _services.UpdatejobType(model.Description, model.Name, model.Id, model.IsActive.Value);
            if (msg.Contains("Error"))
            {
                ViewData["error"] = msg;
                return View(model);
            }
            else
            {
                TempData["error"] = msg;
                return RedirectToAction("JobTypeList");
            }
        }

        [AuthorizeVerifiedadmin]
        public ActionResult index(int? id, int? statusId)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            tblJobMaster _model = new tblJobMaster();
            jobServices _services = new jobServices();
            contactServices _ser = new contactServices();
            ViewData["users"] = _ser.GetaAllContactsByAdminId(adminId);
            ViewData["type"] = _services.getAllJobtypes(adminId);
            ViewData["status"] = _services.getAllStatus();
            ViewData["statusId"] = statusId ?? 0;
            if (id.HasValue)
            {
                _model = _services.GetjobbyId(id.Value);
                return View("editjob", _model);
            }
            if (TempData["error"] != null)
            {
                ViewData["error"] = TempData["error"].ToString();
            }
            return View("job", _model);
        }

        [HttpPost]
        public string createjob(string EndDate, string StartDate, string poNumber, string name, int TypeId, int StatusId, int AssignTo, string description)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            jobServices _services = new jobServices();
            contactServices _ser = new contactServices();
            tblJobMaster _model = new tblJobMaster();
            _model.AssignTo = AssignTo;
            _model.Detail = description;
            _model.PoNumber = poNumber;
            _model.StartDate =Convert.ToDateTime(StartDate);
            _model.EndDate = Convert.ToDateTime(EndDate);
            _model.Name = name;
            _model.StatusId = StatusId;
            _model.TypeId = TypeId;
            if (_model.EndDate < _model.StartDate)
            {
                return "Error : Start Date Can't be grater then End date";
            }
            else
            {
                return _services.AddNewjob(_model, adminId);
            }
        }
        [AuthorizeVerifiedadmin]
        public ActionResult deletejobtype(int id)
        {
            jobServices _services = new jobServices();
            _services.DeleteJobType(id);
            TempData["error"] = "Job type deleted Succesfully";
            return RedirectToAction("JobTypeList");
        }

        [HttpPost]
        public string updatejob(bool IsActive, int id, string EndDate, string StartDate, string poNumber, string name, int TypeId, int StatusId, int AssignTo, string description)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            jobServices _services = new jobServices();
            contactServices _ser = new contactServices();
            tblJobMaster _model = new tblJobMaster();
            _model.AssignTo = AssignTo;
            _model.Detail = description;
            _model.PoNumber = poNumber;
            _model.StartDate = Convert.ToDateTime(StartDate);
            _model.EndDate = Convert.ToDateTime(EndDate);
            _model.Name = name;
            _model.StatusId = StatusId;
            _model.TypeId = TypeId;
            _model.IsActive = IsActive;
            _model.Id = id;
            if (_model.EndDate < _model.StartDate)
            {
                return "Error : Start Date Can't be grater then End date";
            }
            else
            {
                return _services.updatejobbyId(_model);
            }
        }


        [HttpPost]
        public ActionResult index(tblJobMaster _model ,string shour,string ehour, string smin,string emin,string sampm,string eampm)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            jobServices _services = new jobServices();
            contactServices _ser = new contactServices();
            ViewData["users"] = _ser.GetaAllContactsByAdminId(adminId);
            ViewData["type"] = _services.getAllJobtypes(adminId);
            ViewData["status"] = _services.getAllStatus();
            var date1 = _model.StartDate.Value.Date.ToShortDateString() + " " + shour + ":" + smin + ":00 " + sampm;
            var date2 = _model.EndDate.Value.Date.ToShortDateString() + " " + ehour + ":" + emin + ":00 " + eampm;
            _model.EndDate = Convert.ToDateTime(date2);
            _model.StartDate = Convert.ToDateTime(date1); 
            if (_model.EndDate < _model.StartDate)
            {
                ViewData["error"] = "Start Date Can't be grater then End date";
            }
            else
            {
                var msg = _services.AddNewjob(_model, adminId);
                if (msg.Contains("Error"))
                {
                    ViewData["error"] = msg;
                    return View("job", _model);
                }
                else
                {
                    TempData["error"] = msg;
                    return RedirectToAction("GetJobs");
                }
            }
            return View("job", _model);
        }

        [HttpPost]
        public ActionResult editjob(tblJobMaster _model, int JobStatusId, string shour, string ehour, string smin, string emin, string sampm, string eampm)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            jobServices _services = new jobServices();
            contactServices _ser = new contactServices();
            ViewData["users"] = _ser.GetaAllContactsByAdminId(adminId);
            ViewData["type"] = _services.getAllJobtypes(adminId);
            ViewData["status"] = _services.getAllStatus();
            ViewData["statusId"] = JobStatusId;
            var date1 = _model.StartDate.Value.Date.ToShortDateString() + " " + shour + ":" + smin + ":00 " + sampm;
            var date2 = _model.EndDate.Value.Date.ToShortDateString() + " " + ehour + ":" + emin + ":00 " + eampm;
            _model.EndDate = Convert.ToDateTime(date2);
            _model.StartDate = Convert.ToDateTime(date1); 
            if (_model.EndDate < _model.StartDate)
            {
                ViewData["error"] = "Start Date Can't be grater then End date";
            }
            else
            {
                var msg = _services.updatejobbyId(_model);
                if (msg.Contains("Error"))
                {
                    ViewData["error"] = msg;
                    return View("editjob", _model);
                }
                else
                {
                    TempData["error"] = msg;
                    if (JobStatusId == 0)
                    {
                        return RedirectToAction("getjobs");
                    }
                    else
                    {
                        return RedirectToAction("getjobs", "job", new { statusId = JobStatusId });
                    }
                }
            }
            return View("editjob", _model);
        }

        [AuthorizeVerifiedadmin]
        public ActionResult GetJobs(int? id, int? statusId)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            JobViewModel _model = new JobViewModel();
            jobServices _ser = new jobServices();
            if (id.HasValue)
            {
                _ser.DeleteJob(id.Value);
                _model = _ser.getAlljobs(adminId, statusId);
                _model.status = statusId ?? 0;
                _model.Error = "Job deleted Succesfully";
                return View(_model);
            }
            _model = _ser.getAlljobs(adminId, statusId);
            _model.status = statusId ?? 0;
            return View(_model);
        }

        [AuthorizeVerifiedadmin]
        public ActionResult MyJobs()
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            CalenderJobs _model = new CalenderJobs();
            jobServices _ser = new jobServices();
            jobServices _services = new jobServices();
            contactServices _conser = new contactServices();
            ViewData["users"] = _conser.GetaAllContactsByAdminId(adminId);
            ViewData["type"] = _services.getAllJobtypes(adminId);
            ViewData["status"] = _services.getAllStatus();
            _model = _ser.GetAllTodayTskname(adminId, DateTime.Now);
            return View(_model);
        }
    }
}