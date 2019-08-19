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
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            LogMessage(message, "WRN");
        }

        public static void Error(string message)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            LogMessage(message, "ERR");
        }

        public static void Debug(string message)
        {
            LogMessage(message, "DBG");
        }

        private static void LogMessage(string message, string severity)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("<" + severity + "> " + Config.vars["applicationLogTag"] + ": " + message);
            Console.ResetColor();
        }

        public static async Task ClientLog(LogMessage msg)
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            var severityString = FromLogSeverity(msg.Severity);
            Console.WriteLine("<" + severityString + "> " + Config.vars["discordApiLogTag"] + ": " + msg.Message);

            Console.ResetColor();
        }

        private static string FromLogSeverity(LogSeverity severity)
        {
            switch(severity)
            {
                case LogSeverity.Critical:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    return "CRI";
                case LogSeverity.Debug:
                    return "DBG";
                case LogSeverity.Error:
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    return "ERR";
                case LogSeverity.Info:
                    return "INF";
                case LogSeverity.Verbose:
                    return "VRB";
                case LogSeverity.Warning:
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    return "WRN";
                default:
                    return "Unknown";
            }
        }
    }
}
