using CRM.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class ContactViewModal:ErrorModel
    {
        public List<tblcontact> Contacts { get; set; }
    }
}