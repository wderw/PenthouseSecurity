using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    public class Utils
    {
        public static Random random { get; }
        public static DateTime startupTime;

        static Utils()
        {
            random = new Random();
        }

        public static TimeSpan GetTimeToPapaHour()
        {
            var now = DateTime.Now;
            var papahour = new DateTime(now.Year, now.Month, now.Day, ToGMTHours(21), 37, 0);
            var difference = papahour - now;

            return difference;
        }

        public static int ToGMTHours(int cestHours)
        {
            return cestHours - 2;
        }

        public static TimeSpan GetUptime()
        {
            var now = DateTime.Now;
            var uptime = now - startupTime;
            return uptime;
        }

        public static void MarkStartupTime()
        {
            startupTime = DateTime.Now;
        }
    }
}
