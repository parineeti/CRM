using CRM.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CRM.Database
{
    public class ChartServices
    {
        public chartViewModel GetJobByStatus(int adminId)
        {
            chartViewModel _item = new chartViewModel();
            List<chartItem> item = new List<chartItem>();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var pending = _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.IsActive == true && a.StatusId == 2).Count();
                var NewJobs = _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.IsActive == true && a.StatusId == 1).Count();
                var problem = _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.IsActive == true && a.StatusId == 3).Count();
                var Completed = _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.IsActive == true && a.StatusId == 4).Count();
                chartItem i = new chartItem();
                i.name = "Pending";
                i.value = pending;
                item.Add(i);
                chartItem i1 = new chartItem();
                i1.name = "New";
                i1.value = NewJobs;
                chartItem i2 = new chartItem();
                i2.name = "Problem";
                i2.value = problem;
                chartItem i3 = new chartItem();
                i3.name = "Completed";
                i3.value = Completed;
                item.Add(i1);
                item.Add(i2);
                item.Add(i3);
                _item.data = item;
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

        public chartViewModel GetJobByType(int adminId)
        {
            chartViewModel _item = new chartViewModel();
            List<chartItem> item = new List<chartItem>();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var query = (from post in _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.IsActive == true)
                             join t in _idc.tbljobTypes on post.TypeId equals t.Id
                             select new { Post = post, t = t }).GroupBy(a => a.t.Name).
                            Select(o => new { name = o.Key, Count = o.Count() }).ToList();
                foreach (var items in query)
                {
                    chartItem chrt = new chartItem();
                    chrt.name = items.name;
                    chrt.value = items.Count;
                    item.Add(chrt);
                }
                _item.data = item;
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

        public chartViewModel GetContactBylabel(int adminId)
        {
            chartViewModel _item = new chartViewModel();
            List<chartItem> item = new List<chartItem>();
            Global _global = new Database.Global();
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var query = (from post in _idc.tblcontacts.Where(a => a.AdminId == adminId&&a.IsActive==true)
                             join t in _idc.tbllabels on post.LabelId equals t.Id
                             select new { Post = post, t = t }).GroupBy(a => a.t.LableName).
                            Select(o => new { name = o.Key, Count = o.Count() }).ToList();
                foreach (var items in query)
                {
                    chartItem chrt = new chartItem();
                    chrt.name = items.name;
                    chrt.value = items.Count;
                    item.Add(chrt);
                }
                _item.data = item;
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

        public chartWeekModel GetJobByWeek(int adminId)
        {
            chartWeekModel _item = new chartWeekModel();
            List<chartItem> pending = new List<chartItem>();
            List<chartItem> closed = new List<chartItem>();
            List<chartItem> problem = new List<chartItem>();
            List<chartItem> newList = new List<chartItem>();
            Global _global = new Database.Global();
            DateTime mondayDate = DateTime
                       .Today
                       .AddDays(((int)(DateTime.Today.DayOfWeek) * -1) + 1);
            int monthcount = 7;
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var pjobs = _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.IsActive == true && a.StatusId == 2).ToList();
                var cjobs = _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.IsActive == true && a.StatusId == 4).ToList();
                var njobs = _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.IsActive == true && a.StatusId == 1).ToList();
                var prjobs = _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.IsActive == true && a.StatusId == 3).ToList();
                for (int i = 0; i < monthcount; i++)
                {
                    chartItem pdata = new chartItem();
                    chartItem cdata = new chartItem();
                    chartItem ndata = new chartItem();
                    chartItem prdata = new chartItem();
                    pdata.value = pjobs.Where(a=>a.CreatedDate.Value.Date==mondayDate.Date).Count();
                    cdata.value = cjobs.Where(a =>a.CreatedDate.Value.Date == mondayDate.Date ).Count();
                    ndata.value = njobs.Where(a =>  a.CreatedDate.Value.Date == mondayDate.Date).Count();
                    prdata.value = prjobs.Where(a => a.CreatedDate.Value.Date == mondayDate.Date ).Count();
                     mondayDate = mondayDate.AddDays(1);
                    pending.Add(pdata);
                    closed.Add(cdata);
                    problem.Add(prdata);
                    newList.Add(ndata);
                }
                _item.Problem = problem;
                _item.New = newList;
                _item.pending = pending;
                _item.Completed = closed;
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

        public chartWeekModel GetJobByMonth(int adminId)
        {
            chartWeekModel _item = new chartWeekModel();
            List<chartItem> pending = new List<chartItem>();
            List<chartItem> closed = new List<chartItem>();
            List<chartItem> problem = new List<chartItem>();
            List<chartItem> newList = new List<chartItem>();
            Global _global = new Database.Global(); 
            CRMClassDataContext _idc = new Database.CRMClassDataContext(_global.con);
            try
            {
                var pjobs = _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString()&&a.IsActive==true && a.StatusId == 2).ToList();
                var cjobs = _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.IsActive == true && a.StatusId == 4).ToList();
                var njobs = _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.IsActive == true && a.StatusId == 1).ToList();
                var prjobs = _idc.tblJobMasters.Where(a => a.OpenBy == adminId.ToString() && a.IsActive == true && a.StatusId == 3).ToList();
                int currentYear = DateTime.Now.Year;
                for (int i = 1; i <= 12; i++)
                {
                    chartItem pdata = new chartItem();
                    chartItem cdata = new chartItem();
                    chartItem ndata = new chartItem();
                    chartItem prdata = new chartItem();
                    pdata.value = pjobs.Where(a =>  a.CreatedDate.Value.Month == i && a.CreatedDate.Value.Year == currentYear).Count();
                    cdata.value = cjobs.Where(a =>  a.CreatedDate.Value.Month == i && a.CreatedDate.Value.Year == currentYear).Count();
                    ndata.value = njobs.Where(a =>  a.CreatedDate.Value.Month == i && a.CreatedDate.Value.Year == currentYear).Count();
                    prdata.value = prjobs.Where(a => a.CreatedDate.Value.Month == i && a.CreatedDate.Value.Year == currentYear).Count();
                    pending.Add(pdata);
                    closed.Add(cdata);
                    problem.Add(prdata);
                    newList.Add(ndata);
                }
                _item.Problem = problem;
                _item.New = newList;
                _item.pending = pending;
                _item.Completed = closed;
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

    }
}