using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    public class CommonUtils : ModuleBase<SocketCommandContext>
    {
        [Command("time")]
        public async Task GetTimeToPapaHour()
        {
            var now = DateTime.Now;
            var papahour = new DateTime(now.Year, now.Month, now.Day, 21, 37, 0);
            var difference = papahour - now;

            string verdict;

            if(difference.TotalSeconds >= 0)
                verdict = "Papatime is long overdue. You're late by:";
            else
                verdict = "Its gonna be papatime in:";

            await Context.Channel.SendMessageAsync(verdict + difference.ToString());
        }
    }
}
