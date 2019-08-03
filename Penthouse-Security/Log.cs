using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    public static class Log
    {
        public static void Info(string message)
        {
            LogMessage(message, "INF");
        }

        public static void Warning(string message)
        {
            LogMessage(message, "WRN");
        }

        public static void Error(string message)
        {
            LogMessage(message, "ERR");
        }

        public static void Debug(string message)
        {
            LogMessage(message, "DBG");
        }

        private static void LogMessage(string message, string severity)
        {
            Console.WriteLine("<" + severity + "> " + message);
        }

        public static async Task ClientLog(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}
