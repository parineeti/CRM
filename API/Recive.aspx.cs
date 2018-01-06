using CRM.Database;
using Snowmicro.Framework.Database;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CRM.API
{
    public partial class Recive : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string from = Request.Params["From"].ToString();
            string twilioTo = Request.Params["To"].ToString();
            string body = Request.Params["Body"].ToString();
            from = from.ToString().Replace('(', ' ').Replace('+', ' ').Replace('-', ' ').Replace(')', ' ').Replace(" ", "");
            twilioTo = twilioTo.Replace('+', ' ').Replace(" ", "");
            if (from.Length > 10)
            {
                from = from.Substring(from.Length - 10, 10);
            }
            Global _global = new Database.Global();
            SqlParameter[] sqlParams = new SqlParameter[] {
            new SqlParameter("@from",from),
               new SqlParameter("@body",body)
            };
            SqlHelper.ExecuteNonQuery(_global.con,System.Data.CommandType.StoredProcedure, "AddInbox", sqlParams);       }
    }
}