using CRM.Database;
using CRM.Models;
using CRM.utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;

namespace CRM.Controllers
{
    public class chatController : Controller
    {
        // GET: chat
        TwilioRestClient client = new TwilioRestClient("AC6824f2f5b989da4bacc0159972e26532", "901831d75b9d94ee38d1fcf75b691505");
        [AuthorizeVerifiedadmin]
        public ActionResult Index()
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            chatServices _chat = new Database.chatServices();
            chatModel _model = new Models.chatModel();
            _model = _chat.getContactsByAdminid(adminId);
            if(Session["name"]==null)
            {
                 userServices _ser = new userServices(); 
                var _profile = _ser.getProfileByUserId(adminId);
                   Session["name"] = _profile.name;
                    Session["image"] = !string.IsNullOrEmpty(_profile.image) ? _profile.image : "/assets/images/icon/staff.png";
                      }
            _model.name = Session["name"].ToString();
            _model.imageurl = Session["image"].ToString();
            _model.aId = adminId;
            return View(_model);
        }

        [HttpPost]
        public PartialViewResult getChatById(int UserId,int aid)
        {   chatServices _chat = new Database.chatServices();
            chatModel _model = new Models.chatModel();
            _model = _chat.getChatByContactId(UserId, aid);
            _model.aId = aid;
            return PartialView("getChatById", _model);
        }


        [HttpPost]
        public PartialViewResult getChatBymsgId(int msgId)
        {
            int adminId = Convert.ToInt16(Session["AdminId"]);
            chatServices _chat = new Database.chatServices();
            chatModel _model = new Models.chatModel();
            _model = _chat.getChatBychatId(msgId, adminId);
            return PartialView("getChatById", _model);
        }


        [HttpPost]
        public PartialViewResult getChatnewById(int UserId, int maxid,int aid)
        {
             chatServices _chat = new Database.chatServices();
            chatModel _model = new Models.chatModel();
            _model = _chat.getChatByContactId(UserId, aid, maxid);
            return PartialView("GetNewChat", _model);
        }

        [HttpPost, ValidateInput(false)]
        public void AddChat(int userId, string Text)
        {
            chatServices _chat = new Database.chatServices();
            var mobile = _chat.AddChat(userId, Text, 1);
            var mobileNumber = mobile.Split('_')[0];
            client.SendMessage("+15076077264", mobileNumber, Text);
        }
    }
}