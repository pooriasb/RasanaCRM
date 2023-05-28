using System;
using System.Globalization;

public class UTLDateTime
{
    public UTLDateTime()
    {
    }

    public static String getCurrentTime()
    {
        PersianCalendar cal = new PersianCalendar();
        Int32 h = cal.GetHour(DateTime.Now);
        Int32 m = cal.GetMinute(DateTime.Now);
        Int32 s = cal.GetSecond(DateTime.Now);
        String hour = (h < 10 ? "0" + h.ToString() : h.ToString());
        String minute = (m < 10 ? "0" + m.ToString() : m.ToString());
        String second = (s < 10 ? "0" + s.ToString() : s.ToString());
        return hour + ":" + minute + ":" + second;
    }
    public static String getCurrentDate()
    {
        PersianCalendar cal = new PersianCalendar();
        Int32 y = cal.GetYear(DateTime.Now);
        Int32 m = cal.GetMonth(DateTime.Now);
        Int32 d = cal.GetDayOfMonth(DateTime.Now);
        String year = y.ToString();
        String month = (m < 10 ? "0" + m.ToString() : m.ToString());
        String day = (d < 10 ? "0" + d.ToString() : d.ToString());
        return year + "/" + month + "/" + day;
    }
    public String getPreviousDate(Int32 previousDay)
    {
        previousDay = (-1) * previousDay;
        PersianCalendar cal = new PersianCalendar();
        Int32 y = cal.GetYear(DateTime.Now.AddDays(previousDay));
        Int32 m = cal.GetMonth(DateTime.Now.AddDays(previousDay));
        Int32 d = cal.GetDayOfMonth(DateTime.Now.AddDays(previousDay));
        String year = y.ToString();
        String month = (m < 10 ? "0" + m.ToString() : m.ToString());
        String day = (d < 10 ? "0" + d.ToString() : d.ToString());
        return year + "/" + month + "/" + day;
    }
    public String convertToPersianDate(String dateTime)
    {
        if (dateTime != "")
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(dateTime);
            PersianCalendar cal = new PersianCalendar();
            Int32 y = cal.GetYear(dt);
            Int32 mon = cal.GetMonth(dt);
            Int32 d = cal.GetDayOfMonth(dt);
            Int32 h = cal.GetHour(dt);
            Int32 min = cal.GetMinute(dt);
            Int32 s = cal.GetSecond(dt);
            String year = y.ToString();
            String month = (mon < 10 ? "0" + mon.ToString() : mon.ToString());
            String day = (d < 10 ? "0" + d.ToString() : d.ToString());
            String hour = (h < 10 ? "0" + h.ToString() : h.ToString());
            String minute = (min < 10 ? "0" + min.ToString() : min.ToString());
            String second = (s < 10 ? "0" + s.ToString() : s.ToString());
            return year + "/" + month + "/" + day;
        }
        return "-----";
    }
    public String convertToPersianTime(String dateTime)
    {
        if (dateTime != "")
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(dateTime);
            PersianCalendar cal = new PersianCalendar();
            Int32 h = cal.GetHour(dt);
            Int32 min = cal.GetMinute(dt);
            Int32 s = cal.GetSecond(dt);
            String hour = (h < 10 ? "0" + h.ToString() : h.ToString());
            String minute = (min < 10 ? "0" + min.ToString() : min.ToString());
            String second = (s < 10 ? "0" + s.ToString() : s.ToString());
            return hour + ":" + minute + ":" + second;
        }
        return "-----";
    }
    public String convertToPersianDateTime(String dateTime)
    {
        if (dateTime != "")
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(dateTime);
            PersianCalendar cal = new PersianCalendar();
            Int32 y = cal.GetYear(dt);
            Int32 mon = cal.GetMonth(dt);
            Int32 d = cal.GetDayOfMonth(dt);
            Int32 h = cal.GetHour(dt);
            Int32 min = cal.GetMinute(dt);
            Int32 s = cal.GetSecond(dt);
            String year = y.ToString();
            String month = (mon < 10 ? "0" + mon.ToString() : mon.ToString());
            String day = (d < 10 ? "0" + d.ToString() : d.ToString());
            String hour = (h < 10 ? "0" + h.ToString() : h.ToString());
            String minute = (min < 10 ? "0" + min.ToString() : min.ToString());
            String second = (s < 10 ? "0" + s.ToString() : s.ToString());
            return year + "/" + month + "/" + day + " - " + hour + ":" + minute + ":" + second;
        }
        return "-----";
    }

    public DateTime? ConvertToDateTime(string value)
    {
        try
        {
            string[] parts = value.Split('/');
            return new DateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), new PersianCalendar());
        }
        catch (Exception)
        {
            return null;
        }
    }
    public bool CheckPersainCalender(string value)
    {
        PersianCalendar pc = new PersianCalendar();
        try
        {
            string[] parts = value.Split('/');
            DateTime dt = pc.ToDateTime(Convert.ToInt32(parts[0]), Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]),
                0, 0, 0, 0);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public String Today()
    {
        PersianCalendar pc = new PersianCalendar();
        string year = pc.GetYear(DateTime.Now).ToString();
        string month = pc.GetMonth(DateTime.Now).ToString();
        string day = pc.GetDayOfMonth(DateTime.Now).ToString();
        string week = pc.GetDayOfWeek(DateTime.Now).ToString().ToLower();

        if (week.Contains("sun"))
            week = "یک شنبه";
        else if (week.Contains("mon"))
            week = "دوشنبه";
        if (week.Contains("tue"))
            week = "سه شنبه";
        else if (week.Contains("wed"))
            week = "چهار شنبه";
        if (week.Contains("thu"))
            week = "پنج شنبه";
        else if (week.Contains("fri"))
            week = "جمعه";
        else if (week.Contains("sat"))
            week = "شنبه";

        string monthName = "";

        switch (month)
        {
            case "1":
                monthName = "فروردین";
                break;
            case "2":
                monthName = "اردیبهشت";
                break;
            case "3":
                monthName = "خرداد";
                break;
            case "4":
                monthName = "تیر";
                break;
            case "5":
                monthName = "مرداد";
                break;
            case "6":
                monthName = "شهریور";
                break;
            case "7":
                monthName = "مهر";
                break;
            case "8":
                monthName = "آبان";
                break;
            case "9":
                monthName = "آذر";
                break;
            case "10":
                monthName = "دی";
                break;
            case "11":
                monthName = "بهمن";
                break;
            case "12":
                monthName = "اسفند";
                break;

        }

        String result = week + "، " + day + " " + monthName + " " + year;

        return result;
    }

    public static string GetDiffrent(DateTime dt, CalculateType type)
    {
        var start = DateTime.Now;
        int totalDays = (int)(start - dt).TotalDays;
        int totalHours = (int) (start - dt).TotalHours;
        int totalMinute = (int) (start - dt).TotalMinutes;

        DateTime calHour = start.AddHours(-totalHours);
        switch (type)
        {
            case CalculateType.minite:
                {
                    if (totalMinute < 1)
                    {
                        return "دقایقی پیش";
                    }
                    else if (totalMinute > 60)
                    {
                        int calMinutes = (int) (calHour - dt).TotalMinutes;
                        return $" {totalHours} ساعت و {calMinutes} دقیقه پیش ";
                    }
                    else
                    {
                        return $" دقیقه پیش {totalMinute}";
                    }
                }
            default:
            {
                return "";
            }
        }

    }
    public enum CalculateType
    {
        day,
        hour,
        minite,
        second
    }

}