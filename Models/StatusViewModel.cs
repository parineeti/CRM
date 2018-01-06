using CRM.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class StatusViewModel:ErrorModel
    {
        public List<tblstatus> statusList { get; set; }
    }
}