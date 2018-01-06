using CRM.Database;
using CRM.Models;
using CRM.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CRM.Controllers
{
    public class JsonController : Controller
    {
        // GET: Json
        [AuthorizeVerifiedadmin]
        public PartialViewResult getProfile() 
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            userServices _ser = new userServices();
            var _profile = _ser.getProfileByUserId(adminId);
            if(Session["name"]==null)
            {
                Session["name"] = _profile.name;
                Session["image"] = !string.IsNullOrEmpty(_profile.image)? _profile.image: "/assets/images/icon/staff.png";
            }
            return PartialView(_profile);
        }

        [HttpPost]
        public PartialViewResult getdata()
        {
            return PartialView("editLabel");
        }

        [HttpPost]
        public string GetStatusByChart()
        {
            int adminId = Convert.ToInt16(Session["AdminId"]); 
            ChartServices _ser = new ChartServices();
            var chart = _ser.GetJobByStatus(adminId);
            JavaScriptSerializer obj = new JavaScriptSerializer();
            return obj.Serialize(chart);
        }

        [HttpPost]
        public string GetcontactsByChart()
        {
            int adminid = Convert.ToInt16(Session["AdminId"]); 
            ChartServices _ser = new ChartServices();
            var chart = _ser.GetContactBylabel(adminid);
            JavaScriptSerializer obj = new JavaScriptSerializer();
            return obj.Serialize(chart);
        }


        [HttpPost]
        public string GettypeByChart()
        {
            int adminid = Convert.ToInt16(Session["AdminId"]);
            ChartServices _ser = new ChartServices();
            var chart = _ser.GetJobByType(adminid);
            JavaScriptSerializer obj = new JavaScriptSerializer();
            return obj.Serialize(chart);
        }

        [HttpPost]
        public string GetweekByChart()
        {
            int adminid = Convert.ToInt16(Session["AdminId"]);
            ChartServices _ser = new ChartServices();
            var chart = _ser.GetJobByWeek(adminid);
            JavaScriptSerializer obj = new JavaScriptSerializer();
            return obj.Serialize(chart);
        }
        [HttpPost]
        public string GetmonthByChart()
        {
            int adminid = Convert.ToInt16(Session["AdminId"]);
            ChartServices _ser = new ChartServices();
            var chart = _ser.GetJobByMonth(adminid);
            JavaScriptSerializer obj = new JavaScriptSerializer();
            return obj.Serialize(chart);
        }

        [AuthorizeVerifiedadmin]
        public ActionResult GetEvents(string start = null, string end = null)
        {
            int adminid = Convert.ToInt16(Session["AdminId"]);
            jobServices arg_5B_0 = new jobServices();
            DateTime start2 = (!string.IsNullOrEmpty(start)) ? Convert.ToDateTime(start) : DateTime.Now;
            DateTime end2 = (!string.IsNullOrEmpty(end)) ? Convert.ToDateTime(end) : DateTime.Now.AddDays(30.0);
            Events[] data = arg_5B_0.GetEvents(adminid, start2, end2).ToArray();
            return base.Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetEventsById(int id)
        {
             jobServices _ser = new jobServices();
            var model = _ser.GetjobbyId(id);
            jobModel _model = new Controllers.JsonController.jobModel();
            _model.AssignTo = model.AssignTo;
            _model.Detail = model.Detail;
             _model.EndDate = Convert.ToDateTime(model.EndDate.Value.Date).ToString("MM/dd/yyyy hh:mm:ss tt");
             _model.StartDate = Convert.ToDateTime(model.StartDate.Value.Date).ToString("MM/dd/yyyy hh:mm:ss tt");
            _model.StatusId = model.StatusId;
            _model.TypeId = model.TypeId;
            _model.PoNumber = model.PoNumber;
            _model.Name = model.Name;
            _model.IsActive = model.IsActive.Value;
            return base.Json(_model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void UpdateEvent(int id, string startDate, string EndDate)
        {
            jobServices _ser = new jobServices();
            _ser.UpdateEventDate(id,startDate,EndDate);
        }

        public class jobModel
        {
            public int Id;
            public string Name;
            public string PoNumber;
            public string Detail;
            public System.Nullable<int> TypeId;
             public System.Nullable<int> AssignTo;
            public System.Nullable<int> StatusId;
            public string StartDate;
            public string EndDate;
            public bool IsActive;
        }
    }
}