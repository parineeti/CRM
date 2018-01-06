using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class UserViewModel:ErrorModel
    {
        public List<Labelitem> labels { get; set; }
        public int totalContacts { get; set; }
    }
}