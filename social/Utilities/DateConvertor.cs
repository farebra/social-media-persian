using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace social.Utilities
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc=new PersianCalendar();
            return pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00")+" - "+pc.GetHour(value)+":"+pc.GetMinute(value);

            DateTime date = value;
            string persianDateString = date.ToString("yyyy/MM/dd/hh/mm", new CultureInfo("fa-IR"));
        }
    }
}
