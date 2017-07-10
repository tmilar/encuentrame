using System;

namespace Encuentrame.Support
{
    
    public static class SystemDateTime
    {
        private static IDateProvider provider;

        public static void Use(IDateProvider dateProvider)
        {
            provider = dateProvider;
        }
        static SystemDateTime()
        {
            UseDefault();
        }
        public static DateTime Today
        {
            get
            {
                return provider.Today;
            }
        }
        public static DateTime Now
        {
            get
            {
                return provider.Now;
            }
        }

        public static void UseDefault()
        {
            provider = new DateTimeProvider();
        }
    }
    public interface IDateProvider
    {
        DateTime Today { get; }
        DateTime Now { get; }
    }
    public class Delorean : IDateProvider
    {
        private Delorean()
        {
        }

        public static Delorean StoppedIn(DateTime moment)
        {
            return new Delorean
            {
                Now = moment
            };
        }

        public DateTime Today
        {
            get { return Now.Date; }
        }

        public DateTime Now { get; private set; }

        public void TravelTo(DateTime time)
        {
            Now = time;
        }

    }
    public class DateTimeProvider : IDateProvider
    {
        public DateTime Today
        {
            get { return DateTime.Today; }
        }

        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}