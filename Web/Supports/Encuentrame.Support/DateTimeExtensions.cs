using System;
using System.Configuration;
using System.Threading;

namespace Encuentrame.Support
{
    public static class DateTimeExtensions
    {
        public static DateTime SetAtBeginDay(this DateTime date)
        {
            return new DateTime(date.Year,date.Month,date.Day,0,0,0); 
        }
        public static DateTime SetAtEndDay(this DateTime date)
        {
            return date.AddDays(1).SetAtBeginDay().AddSeconds(-1);
        }
        public static string AsShortTimeString(this DateTime date)
        {
            return date.ToString("HH:mm");
        }

        public static string AsTimeString(this DateTime date)
        {
            return date.ToString("HH:mm:ss");
        }

        public static string AsLongDateString(this DateTime date)
        {
            return date.ToString("dd-MMM-yyyy");
        }

        public static string AsSlashedLongDateString(this DateTime date)
        {
            return date.ToString("dd/MMM/yyyy");
        }

        public static string AsYearMonthDateString(this DateTime date)
        {
            return date.ToString("yyyyMMdd");
        }

        public static string AsSlashedYearMonthDateString(this DateTime date)
        {
            return date.ToString("yyyy/MM/dd");
        }
        public static string AsShortDatePattern(this DateTime date)
        {
            return date.ToString(Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern);
        }

        public static string AsMinutesToTimeString(this int minutes)
        {
            var timeSpan = new TimeSpan(0, minutes, 0);
            return "{0} hrs {1} mins".FormatWith(timeSpan.TotalHours.AsInt(), timeSpan.Minutes);
        }
        public static int Compare(this DateTime? x, DateTime? y)
        {
            if (x.HasValue)
            {
                if (y.HasValue)
                    return x.Value.CompareTo(y.Value);
                return 1;
            }
            if (y.HasValue)
                return -1;
            return 0;
        }
        public static bool AreEqual(this DateTime x, DateTime y)
        {
            return x.Year == y.Year && x.Month == y.Month && x.Day == y.Day && x.Hour == y.Hour && x.Minute == y.Minute &&
                   x.Second == y.Second;
        }

        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ToTimeZoneTime(this DateTime time, string timeZoneId = "Argentina Standard Time")
        {
            //https://msdn.microsoft.com/en-us/library/gg154758.aspx
            var configTimeZone = ConfigurationManager.AppSettings["TimeZone"] as string;
            if (configTimeZone.NotIsNullOrEmpty())
            {
                timeZoneId = configTimeZone;
            }
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return time.ToTimeZoneTime(tzi);
        }

        /// <summary>
        /// Returns TimeZone adjusted time for a given from a Utc or local time.
        /// Date is first converted to UTC then adjusted.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="timeZoneId"></param>
        /// <returns></returns>
        public static DateTime ToTimeZoneTime(this DateTime time, TimeZoneInfo tzi)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(time, tzi);
        }
    }
}
