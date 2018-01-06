using CRM.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class JobViewModel:ErrorModel
    {
        public List<jobViewItem> _jobs { get; set; }
        public int status { get; set; }
        public int New { get; set; }
        public int pending { get; set; }
        public int complete { get; set; }
        public int problem { get; set; }
    }

    public class jobViewItem
    {
        public int JobId { get; set; }
        public string name { get; set; }
        public string Po { get; set; }
        public string Type { get; set; }
        public string OpenBy { get; set; }
        public string OpenByImage { get; set; }
        public string Date { get; set; }
        public string AssignToImage { get; set; }
        public string AssignTo { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Detail { get; set; }
    }

    public class CalenderJobs
    {
        public List<calnderitems> jobs { get; set; }
    }

    public class calnderitems
    {
        public string Name { get; set; } 
        public string  Statusclass { get; set; }
    }
}