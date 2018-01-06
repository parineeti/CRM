using CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Snowmicro.Framework.Database;
using System.Data;
using System.Data.SqlClient;

namespace CRM.Database
{
    public class jobServices
    {
        public string GetClassbyid(int id)
        {
            string status = "";
            switch (id)
            {
                case 1:
                    status = "New";
                    break;
                case 2:
                    status = "pending";
                    break;
                case 3:
                    status = "problem";
                    break;
                case 4:
                    status = "complete";
                    break;
            }
            return status;
        }
        public void DeleteJobType(int jobtypeId)
        {  Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var p = _idc.tbljobTypes.Where(a => a.Id == jobtypeId).FirstOrDefault();
                p.IsActive = false;
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            } 
        }

        public List<Events> GetEvents(int adminid, DateTime start, DateTime end)
        {
            List<Events> _profile = new List<Events>();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                SqlParameter[] sqlParams = new SqlParameter[] {
                     new SqlParameter("@adminId", adminid),
                     new SqlParameter("@date", start),
                      new SqlParameter("@endate", end)
                    };
                var dt = SqlHelper.ExecuteDataset(_global.con, CommandType.StoredProcedure, "getTodaysTaskByDate", sqlParams).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Events newEvent = new Events
                    {
                        id = dt.Rows[i]["Id"].ToString(),
                        start = Convert.ToDateTime(dt.Rows[i]["StartDate"]).ToString("s"),
                        end = Convert.ToDateTime(dt.Rows[i]["EndDate"]).ToString("s"),
                        title = dt.Rows[i]["Name"].ToString(), 
                        className = GetClassbyid(Convert.ToInt16(dt.Rows[i]["StatusId"].ToString()))
                    };
                    _profile.Add(newEvent);
                } 
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _profile;
        }

        public StatusViewModel getAllStatus()
        {
            StatusViewModel _profile = new Models.StatusViewModel();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var p = _idc.tblstatus.ToList();
                _profile.statusList = p;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _profile;
        }

        public CalenderJobs GetAllTodayTskname(int adminid, DateTime startDate)
        {
            CalenderJobs _taskList = new Models.CalenderJobs();
            List<calnderitems> _profile = new List<calnderitems>();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                  SqlParameter[] sqlParams = new SqlParameter[] {
                     new SqlParameter("@adminId", adminid),
                     new SqlParameter("@date", startDate)
                    };
                var dt= SqlHelper.ExecuteDataset(_global.con, CommandType.StoredProcedure, "getTodaysTask", sqlParams);
                for (int i = 0; i < dt.Tables[0].Rows.Count; i++)
                {
                    calnderitems _items = new Models.calnderitems();
                    _items.Name = dt.Tables[0].Rows[i]["Name"].ToString();
                    _items.Statusclass = GetClassbyid(Convert.ToInt32(dt.Tables[0].Rows[i]["StatusId"]));
                    _profile.Add(_items); 
                } 
                _taskList.jobs = _profile;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _taskList;
        }

        public void DeleteJob(int id)
        {
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var p = _idc.tblJobMasters.Where(a => a.Id == id).FirstOrDefault();
                p.IsActive = false;
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }

        }

        public JobViewModel getAlljobs(int adminId,int? statusid)
        {
            JobViewModel _profile = new Models.JobViewModel();
            List<jobViewItem> _itemList = new List<jobViewItem>();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var jobs = _idc.tblJobMasters.Where(a => a.IsActive == true && a.OpenBy == adminId.ToString());
                _profile.pending = jobs.ToList().Where(a => a.StatusId == 2).ToList().Count();
                _profile.New = jobs.ToList().Where(a => a.StatusId == 1).ToList().Count();
                _profile.problem = jobs.ToList().Where(a => a.StatusId == 3).ToList().Count();
                _profile.complete = jobs.ToList().Where(a => a.StatusId == 4).ToList().Count();
                var contacts = _idc.tbllogins.Where(a => a.UserId == adminId).FirstOrDefault();
                var contactImage = string.IsNullOrEmpty(contacts.Image) ? "/assets/images/icon/staff.png" : contacts.Image;
                var query = from post in jobs.Where(a =>(!statusid.HasValue ||a.StatusId.Value==statusid))
                            join meta in _idc.tblcontacts on post.AssignTo equals meta.ContactId
                            join s in _idc.tblstatus on post.StatusId equals s.Id
                            join t in _idc.tbljobTypes on post.TypeId equals t.Id
                            select new { Post = post, Meta = meta, s = s, t = t };
                string image = "/assets/images/icon/staff.png";
                foreach (var i in query)
                {
                    jobViewItem _items = new Models.jobViewItem();
                    _items.AssignTo = i.Meta.Name;
                    _items.AssignToImage = !string.IsNullOrEmpty(i.Meta.ImagePath) ? i.Meta.ImagePath : image;
                    _items.OpenBy = contacts.FirstName;
                    _items.OpenByImage = contactImage;
                    _items.Date = i.Post.CreatedDate.ToString();
                    _items.Detail = i.Post.Detail;
                    _items.JobId = i.Post.Id;
                    _items.name = i.Post.Name;
                    _items.Po = i.Post.PoNumber;
                    _items.StatusId = i.Post.StatusId.Value;
                    _items.Status = i.s.StatusName;
                    _items.Type = i.t.Name;
                    _itemList.Add(_items);
                }
                _profile._jobs = _itemList;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _profile;
        }

        public DashboardViewModel getAllTodayjobs(int adminId)
        {

            DashboardViewModel _profile = new Models.DashboardViewModel();
            List<jobViewItem> _itemList = new List<jobViewItem>();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var contacts = _idc.tbllogins.Where(a => a.UserId == adminId).FirstOrDefault();
                var contactImage = string.IsNullOrEmpty(contacts.Image) ? "/assets/images/icon/staff.png" : contacts.Image;
                var query = from post in _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.CreatedDate.Value.Date == DateTime.Now.Date && a.IsActive == true)
                            join meta in _idc.tblcontacts on post.AssignTo equals meta.ContactId
                            join s in _idc.tblstatus on post.StatusId equals s.Id
                            join t in _idc.tbljobTypes on post.TypeId equals t.Id
                            select new { Post = post, Meta = meta, s = s, t = t };
                string image = "/assets/images/icon/staff.png";
                foreach (var i in query)
                {
                    jobViewItem _items = new Models.jobViewItem();
                    _items.AssignTo = i.Meta.Name;
                    _items.AssignToImage = !string.IsNullOrEmpty(i.Meta.ImagePath) ? i.Meta.ImagePath : image;
                    _items.OpenBy = contacts.FirstName;
                    _items.OpenByImage = contactImage;
                    _items.Date = i.Post.CreatedDate.ToString();
                    _items.Detail = i.Post.Detail;
                    _items.JobId = i.Post.Id;
                    _items.name = i.Post.Name;
                    _items.Po = i.Post.PoNumber;
                    _items.StatusId = i.Post.StatusId.Value;
                    _items.Status = i.s.StatusName;
                    _items.Type = i.t.Name;
                    _itemList.Add(_items);
                }
                _profile._jobs = _itemList;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _profile;
        }

        public string AddNewStatus(string color, string name)
        {
            string message = "Status Added Succesfully";
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                tblstatus tbl = new tblstatus { Color = color, StatusName = name };
                _idc.tblstatus.InsertOnSubmit(tbl);
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {
                message = "Error " + ex.Message;
            }
            finally
            {
                _idc.Dispose();
            }
            return message;
        }

        public string UpdateStatus(string color, string name, int id)
        {
            string message = "Status Updated Succesfully";
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var tbl = _idc.tblstatus.Where(a => a.Id == id).FirstOrDefault();
                tbl.Color = color;
                tbl.StatusName = name;
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {
                message = "Error " + ex.Message;
            }
            finally
            {
                _idc.Dispose();
            }
            return message;
        }

        public tblstatus GetStatusbyId(int id)
        {
            tblstatus _profile = new tblstatus();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var p = _idc.tblstatus.Where(a => a.Id == id).FirstOrDefault();
                return p;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _profile;
        }

        public jobtypeViewModal getAllJobtypes(int adminId)
        {
            jobtypeViewModal _profile = new Models.jobtypeViewModal();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var p = _idc.tbljobTypes.Where(a => a.AdminId == adminId && a.IsActive == true).ToList();
                _profile.jobtype = p;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _profile;
        }

        public string AddNewjobtype(string description, string name, int adminId)
        {
            string message = "Job Type Added Succesfully";
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                tbljobType job = new Database.tbljobType { AdminId = adminId, CReatedDate = DateTime.Now, Description = description, IsActive = true, Name = name };
                _idc.tbljobTypes.InsertOnSubmit(job);
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {
                message = "Error " + ex.Message;
            }
            finally
            {
                _idc.Dispose();
            }
            return message;
        }

        public string UpdatejobType(string description, string name, int id, bool isActive)
        {
            string message = "Job type Updated Succesfully";
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var tbl = _idc.tbljobTypes.Where(a => a.Id == id).FirstOrDefault();
                tbl.Name = name;
                tbl.Description = description;
                tbl.IsActive = isActive;
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {
                message = "Error " + ex.Message;
            }
            finally
            {
                _idc.Dispose();
            }
            return message;
        }

        public tbljobType GetjobtypebyId(int id)
        {
            tbljobType _profile = new tbljobType();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var p = _idc.tbljobTypes.Where(a => a.Id == id).FirstOrDefault();
                return p;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _profile;
        }

        public tblJobMaster GetjobbyId(int id)
        {
            tblJobMaster _profile = new tblJobMaster();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var p = _idc.tblJobMasters.Where(a => a.Id == id).FirstOrDefault();
                return p;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                _idc.Dispose();
            }
            return _profile;
        }

        public string AddNewjob(tblJobMaster jobs, int adminId)
        {
            string message = "Job Created Succesfully";
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                jobs.CreatedDate = DateTime.Now;
                jobs.OpenBy = adminId.ToString();
                jobs.IsActive = true;
                _idc.tblJobMasters.InsertOnSubmit(jobs);
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {
                message = "Error " + ex.Message;
            }
            finally
            {
                _idc.Dispose();
            }
            return message;
        }
        public void UpdateEventDate(int id, string startDate, string EndDate)
        {
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var job = _idc.tblJobMasters.Where(a => a.Id == id).FirstOrDefault();
                job.StartDate = startDate.ToDateTime();
                job.EndDate = EndDate.ToDateTime();
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _idc.Dispose();
            }
        }

        public string updatejobbyId(tblJobMaster jobs)
        {
            string message = "Job Updated Succesfully";
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var job = _idc.tblJobMasters.Where(a => a.Id == jobs.Id).FirstOrDefault();
                job.AssignTo = jobs.AssignTo;
                job.Detail = jobs.Detail;
                job.EndDate = jobs.EndDate;
                job.Name = jobs.Name;
                job.PoNumber = jobs.PoNumber;
                job.StartDate = jobs.StartDate;
                job.StatusId = jobs.StatusId;
                job.TypeId = jobs.TypeId;
                job.IsActive = jobs.IsActive;
                _idc.SubmitChanges();
            }
            catch (Exception ex)
            {
                message = "Error " + ex.Message;
            }
            finally
            {
                _idc.Dispose();
            }
            return message;
        }
    }
}