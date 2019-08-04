using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    public class Utils
    {
        public static TimeSpan GetTimeToPapaHour()
        {
            var now = DateTime.Now;
            var papahour = new DateTime(now.Year, now.Month, now.Day, 21, 37, 0);
            var difference = papahour - now;

            return difference;
        }
    }
}
