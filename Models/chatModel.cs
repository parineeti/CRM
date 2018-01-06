using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Models
{
    public class chatitem
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
    }

    public class chatModel
    {
        public int xid { get; set; }
        public List<chatitem> chatList { get; set; }
        public List<msgItem> msgItem { get; set; }
        public int selectedId { get; set; }
        public int aId { get; set; }
        public string imageurl { get; set; }
        public string name { get; set; }
    }

    public class msgItem
    {
        public int userId { get; set; }
        public int sender { get; set; }
        public int messageId { get; set; }
        public string Image { get; set; }
        public string message { get; set; }
        public string  Name { get; set; }
        public string Time { get; set; }
    }
}