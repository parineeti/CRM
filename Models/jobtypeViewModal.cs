using CRM.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class jobtypeViewModal:ErrorModel
    {
        public List<tbljobType> jobtype { get; set; }
    }

    public class jobViewModal : ErrorModel
    {
        public List<tblJobMaster> jobtype { get; set; }
    }
}