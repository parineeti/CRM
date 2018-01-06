using CRM.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class Labelitem : ErrorModel
    {
        public int LabelId { get; set; }
        public string Name { get; set; }
        public string description { get; set; }
        public int Totalcontacts { get; set; }
        public bool Active { get; set; }
    }

    public class LabelModel : ErrorModel
    {
        public List<Labelitem> _labelList { get; set; }
    }

    public class contactitem : ErrorModel
    {
        public int contactId { get; set; }
        public string Name { get; set; }
        public int LabelId { get; set; }
        public string email { get; set; }
        public bool Active { get; set; }
        public string phone { get; set; }
        public string role { get; set; }
        public int age { get; set; }
        public string joiningdate { get; set; }
        public decimal salary { get; set; }
        public int adminId { get; set; }
        public string Image { get; set; }
    }

    public class contactModel : ErrorModel
    {
        public List<tblcontact> _contactList { get; set; }
    }

}