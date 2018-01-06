using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Snowmicro.Framework.Database;
using System.Data.SqlClient;

namespace CRM.Database
{
    public class chatServices
    {

        public string AddChat(int userId, string text, int AdminId)
        {
            string mobile = "";
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var contacts = _idc.tblcontacts.Where(a => a.ContactId == userId).FirstOrDefault();
                mobile = contacts.phone;
                tblinbox tbl = new tblinbox
                {
                    Body = text,
                    userId = userId,
                    CreatedDate = DateTime.Now,
                    sender = AdminId,
                    reciver = userId,
                    isRead = true
                };
                _idc.tblinboxes.InsertOnSubmit(tbl);
                _idc.SubmitChanges();
                mobile = mobile + "_" + tbl.MsgId.ToString();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return mobile;

        }

        public chatModel getContactsByAdminid(int adminId)
        {
            chatModel _item = new chatModel();
            List<chatitem> item = new List<Models.chatitem>();
            List<msgItem> msgItem = new List<Models.msgItem>();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                int selected = 0;
                var contacts = _idc.tblcontacts.Where(a => a.AdminId == adminId && a.IsActive == true).OrderByDescending(a => a.ContactId).ToList();
                foreach (var i in contacts)
                {
                    chatitem tem = new chatitem();
                    tem.id = i.ContactId;
                    tem.image = string.IsNullOrEmpty(i.ImagePath) ? "/assets/images/icon/staff.png" : i.ImagePath;
                    tem.name = i.Name;
                    selected = selected == 0 ? i.ContactId : selected;
                    item.Add(tem);
                }
                _item.selectedId = selected;
                _item.chatList = item;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _item;
        }

        public chatModel getChatByContactId(int contactId, int adminId)
        {
            chatModel _item = new chatModel();
            List<chatitem> item = new List<Models.chatitem>();
            List<msgItem> msgItem = new List<Models.msgItem>();
            Global _global = new Database.Global();
            try
            {
                string image = "/assets/images/icon/staff.png"; 
                SqlParameter[] sqlParams = new SqlParameter[] {
            new SqlParameter("@ContactId",contactId),
               new SqlParameter("@adminId",adminId)
            };
                bool isAdmin = false;
                var dt = SqlHelper.ExecuteDataset(_global.con, System.Data.CommandType.StoredProcedure, "GetChatById", sqlParams).Tables[0];
                var contactImage = ""; 
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    msgItem tem = new msgItem();
                    tem.userId = contactId;
                    isAdmin = Convert.ToBoolean(dt.Rows[i]["IsRead"].ToString());
                    contactImage = string.IsNullOrEmpty(dt.Rows[i]["AdminImage"].ToString()) ? "/assets/images/icon/staff.png" : dt.Rows[i]["AdminImage"].ToString();
                    tem.sender =Convert.ToInt16(dt.Rows[i]["sender"].ToString()) ;
                    tem.message = dt.Rows[i]["Body"].ToString();
                    tem.messageId = Convert.ToInt16(dt.Rows[i]["MsgId"].ToString());  
                    tem.Image = isAdmin ? contactImage : string.IsNullOrEmpty(dt.Rows[i]["ImagePath"].ToString()) ? image : dt.Rows[i]["ImagePath"].ToString();
                    tem.Name = isAdmin ? dt.Rows[i]["AdminName"].ToString() : dt.Rows[i]["Name"].ToString();
                    tem.Time = Convert.ToDateTime(dt.Rows[i]["CreatedDate"].ToString()).ToString("hh:mm tt");
                    msgItem.Add(tem);
                } 
                dt.Dispose(); 
                _item.msgItem = msgItem;
                _item.xid = contactId;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _global.con.Dispose();
            }
            return _item;
        }

        public chatModel getChatBychatId(int chatid, int adminId)
        {
            chatModel _item = new chatModel();
            List<chatitem> item = new List<Models.chatitem>();
            List<msgItem> msgItem = new List<Models.msgItem>();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var contacts = _idc.tbllogins.Where(a => a.UserId == adminId && a.IsActive == "y").FirstOrDefault();
                var contactImage = string.IsNullOrEmpty(contacts.Image) ? "/assets/images/icon/staff.png" : contacts.Image;
                var query =
               from post in _idc.tblinboxes.Where(a => a.MsgId == chatid)
               join meta in _idc.tblcontacts on post.userId equals meta.ContactId
               select new { Post = post, Meta = meta };
                string image = "/assets/images/icon/staff.png";
                foreach (var i in query)
                {
                    msgItem tem = new msgItem();
                    _item.xid = i.Post.userId.Value;
                    tem.userId = i.Post.userId.Value;
                    tem.sender = i.Post.sender.Value;
                    tem.message = i.Post.Body;
                    tem.messageId = i.Post.MsgId;
                    tem.Image = i.Post.sender != i.Meta.ContactId ? contactImage : string.IsNullOrEmpty(i.Meta.ImagePath) ? image : i.Meta.ImagePath;
                    tem.Name = i.Post.sender != i.Meta.ContactId ? contacts.FirstName : i.Meta.Name;
                    tem.Time = i.Post.CreatedDate.Value.ToString("hh:mm tt");
                    msgItem.Add(tem);
                }
                _item.msgItem = msgItem;

            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _item;
        }


        public chatModel getChatByContactId(int contactId, int adminId, int maxId)
        {
            chatModel _item = new chatModel();
            List<chatitem> item = new List<Models.chatitem>();
            List<msgItem> msgItem = new List<Models.msgItem>();
            Global _global = new Database.Global();
              try
            {
                string image = "/assets/images/icon/staff.png";
                SqlParameter[] sqlParams = new SqlParameter[] {
            new SqlParameter("@ContactId",contactId),
               new SqlParameter("@adminId",adminId),
               new SqlParameter("@maxId",maxId)
            };
                bool isAdmin = false;
                var dt = SqlHelper.ExecuteDataset(_global.con, System.Data.CommandType.StoredProcedure, "GetChatById2", sqlParams).Tables[0];
                var contactImage = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                   
                    msgItem tem = new msgItem();
                    isAdmin = Convert.ToBoolean(dt.Rows[i]["IsRead"].ToString());

                    tem.userId = contactId;
                    contactImage = string.IsNullOrEmpty(dt.Rows[i]["AdminImage"].ToString()) ? "/assets/images/icon/staff.png" : dt.Rows[i]["AdminImage"].ToString();
                    tem.sender = Convert.ToInt16(dt.Rows[i]["sender"].ToString());
                    tem.message = dt.Rows[i]["Body"].ToString();
                    tem.messageId = Convert.ToInt16(dt.Rows[i]["MsgId"].ToString());
                    tem.Image = isAdmin ? contactImage : string.IsNullOrEmpty(dt.Rows[i]["ImagePath"].ToString()) ? image : dt.Rows[i]["ImagePath"].ToString();
                    tem.Name = isAdmin ? dt.Rows[i]["AdminName"].ToString() : dt.Rows[i]["Name"].ToString();
                    tem.Time = Convert.ToDateTime(dt.Rows[i]["CreatedDate"].ToString()).ToString("hh:mm tt");
                    msgItem.Add(tem);
                }
                _item.msgItem = msgItem;
                _item.xid = contactId;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _global.con.Dispose();
            }
            return _item;
        }
    }
}