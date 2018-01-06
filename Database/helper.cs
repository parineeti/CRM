using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRM.Database
{
    public static class helper
    {
        public static DateTime ToDateTime(this string datetime, char dateSpliter = '/', char timeSpliter = ':', char millisecondSpliter = ',')
        {
            try
            {
                datetime = datetime.Trim();
                datetime = datetime.Replace("  ", " ");
                string[] body = datetime.Split(' ');
                string[] date = body[0].Split(dateSpliter);
                int year = Convert.ToInt32(date[2]);
                int month = Convert.ToInt32(date[0]);
                int day = Convert.ToInt32(date[1]);
                int hour = 0, minute = 0;
                if (body.Length == 2)
                {
                    string[] time = body[1].Split(timeSpliter);
                    hour = Convert.ToInt32(time[0]);
                    minute = Convert.ToInt32(time[1]);
                }
                return new DateTime(year, month, day, hour, minute, 00);
            }
            catch
            {
                return new DateTime();
            }
        }

    }
}