using System;

namespace Encuentrame.Support
{
    public static class DateExtensions
    {

        public static DateTime From(this DateTime date)
        {
            return date.AddSeconds(-1);
        }
        public static DateTime To(this DateTime date)
        {
            return date.AddDays(1).Date.AddSeconds(-1);
        }
        public static DateTime January(this int year, int day)
        {
            return new DateTime(year, 1, day);
        }
        public static DateTime February(this int year, int day)
        {
            return new DateTime(year, 2, day);
        }
        public static DateTime March(this int year, int day)
        {
            return new DateTime(year, 3, day);
        }
        public static DateTime April(this int year, int day)
        {
            return new DateTime(year, 4, day);
        }
        public static DateTime May(this int year, int day)
        {
            return new DateTime(year, 5, day);
        }
        public static DateTime June(this int year, int day)
        {
            return new DateTime(year, 6, day);
        }
        public static DateTime July(this int year, int day)
        {
            return new DateTime(year, 7, day);
        }
        public static DateTime August(this int year, int day)
        {
            return new DateTime(year, 8, day);
        }
        public static DateTime September(this int year, int day)
        {
            return new DateTime(year, 9, day);
        }
        public static DateTime October(this int year, int day)
        {
            return new DateTime(year, 10, day);
        }
        public static DateTime November(this int year, int day)
        {
            return new DateTime(year, 11, day);
        }
        public static DateTime December(this int year, int day)
        {
            return new DateTime(year, 12, day);
        }

        public static bool IsValidYear(this int year)
        {
            return year > 0;
        }

        public static bool IsValidMonth(this int month)
        {
            return month >= 1 && month <= 12;
        }

        public static void Substract(this DateTime d1, DateTime d2, out int years, out int months, out int days)
        {
            if (d1 < d2)
            {
                var d3 = d2;
                d2 = d1;
                d1 = d3;
            }

            months = 12 * (d1.Year - d2.Year) + (d1.Month - d2.Month);

            if (d1.Day < d2.Day)
            {
                months--;
                days = DateTime.DaysInMonth(d2.Year, d2.Month) - d2.Day + d1.Day;
            }
            else
            {
                days = d1.Day - d2.Day;
            }

            years = months / 12;
            months -= years * 12;
        }

        public static bool Between(this DateTime date, DateTime lower, DateTime upper)
        {
            return date <= upper && date >= lower;
        }
    }
}