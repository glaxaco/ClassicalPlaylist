using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iTunesUtils.Extensions
{
    static class TimeSpanExtensions
    {
        public static TimeSpan Days(this int value)
        {
            return TimeSpan.FromDays(value);
        }
        public static TimeSpan Weeks(this int value)
        {
            return TimeSpan.FromDays(value*7);
        }
        public static TimeSpan Months(this int value)
        {
            return TimeSpan.FromDays(value*30);
        }

        public static DateTime Ago(this TimeSpan value)
        {
            return DateTime.Now - value;
        }
    }
}
