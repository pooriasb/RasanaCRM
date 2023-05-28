using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;


namespace Web.Insfrastructure.Utilities
{

    public static class CalendarMngr
    {
        public static string ConvertNumbersToPersian(this string str)
        {
            return str.Replace("0", "۰").Replace("1", "۱").Replace("2", "۲").Replace("3", "۳").Replace("4", "۴").Replace("5", "۵").Replace("6", "۶").Replace("7", "v").Replace("8", "۸").Replace("9", "۹");
        }
        public static string ConvertNumbersToEnglish(this string str)
        {
            return str.Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4").Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");
        }
        public static DateTime PersianToJulian(string pdate)
        {
            PersianCalendar pcal = new PersianCalendar();
            string[] parts = pdate.Split('/', '-');
            int year = int.Parse(parts[0].ConvertNumbersToEnglish());
            int month = int.Parse(parts[1].ConvertNumbersToEnglish());
            int day = int.Parse(parts[2].ConvertNumbersToEnglish());
            
            DateTime d = pcal.ToDateTime(year,month,day, 0, 0, 0, 0);
            return d;
        }
      
        public static string JulianToPersian(DateTime dt)
        {
            PersianCalendar pcal = new PersianCalendar();
            return $"{pcal.GetYear(dt)}/{pcal.GetMonth(dt)}/{pcal.GetDayOfMonth(dt)}";
        }

        internal static object JulianToPersian(DateTime? start_Date)
        {
            throw new NotImplementedException();
        }

        public static string findday(int dt)
        {
            Dictionary<string, string[]> DayOfWeeks = new Dictionary<string, string[]>();
            DayOfWeeks.Add("en", new string[] { "Sunday", "Monday", "Tuesday", "Thursday", "Wednesday", "Friday", "Saturday" });
            DayOfWeeks.Add("fa", new string[] { "یک شنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنج شنبه", "جمعه", "شنبه" });
            /*PersianCalendar pc = new PersianCalendar()*/;
            //DateTime dtw = Convert.ToDateTime(dt);
            //string day = pc.GetDayOfWeek(dtw).ToString();
            string day1 = DayOfWeeks["fa"][dt];
            return day1;
        }
        public static string monthofyear(int dt)
        {
            Dictionary<string, string[]> DayOfWeeks = new Dictionary<string, string[]>();
            DayOfWeeks.Add("en", new string[] { "12",  "1", "2","3", "4", "5", "6", "7","8","9","10","11"});
            DayOfWeeks.Add("fa", new string[] { "", "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر","ابان","اذر","دی","بهمن", "اسفند" });
            //PersianCalendar pc = new PersianCalendar();
            //DateTime dtw = Convert.ToDateTime(dt);
            string day1 = DayOfWeeks["fa"][dt];

            return day1;
        }

        public static string JulianToPersianhorof(DateTime dt) {
            System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
            DayOfWeek a = pc.GetDayOfWeek(dt);
            int b = (int)a;
            int c = pc.GetMonth(dt);

            string day = $"{findday(b)} {pc.GetDayOfMonth(dt)} {monthofyear(c)} {pc.GetYear(dt)}";


            return day;
        }


    }
}