using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class chartViewModel
    {
        public List<chartItem> data { get; set; }
    }

    public class chartItem
    {
        public int value { get; set; }
        public string name { get; set; }
    }
    public class chartWeekModel
    {
        public List<chartItem> pending { get; set; }
        public List<chartItem> Completed { get; set; }
        public List<chartItem> Problem { get; set; }
        public List<chartItem> New { get; set; }
    }
}