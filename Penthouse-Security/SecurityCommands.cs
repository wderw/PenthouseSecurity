using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Penthouse_Security
{
    public class SecurityCommands : ModuleBase<SocketCommandContext>
    {
        [Command("anime")]
        public async Task Execute()
        {
            await Context.Channel.SendMessageAsync("anime super ekstra");
        }

        [Command("repeat")]
        public async Task Repeat([Remainder] string message)
        {
            await Context.Channel.SendMessageAsync(message);
        }

        [Command("time")]
        public async Task GetTimeToPapaHour()
        {
            var now = DateTime.Now;
            var papahour = new DateTime(now.Year, now.Month, now.Day, 21, 37, 0);
            var difference = papahour - now;

            string verdict;

            if (difference.TotalSeconds < 0)
                verdict = "Papatime is long overdue. You're late by:";
            else
                verdict = "Its gonna be papatime in:";

            await Context.Channel.SendMessageAsync(verdict + difference.ToString());
        }

        [Command("parse")]
        public async Task Parse([Remainder] string message)
        {            
            string outputMessage = (new LetterParser()).parse(message);
            await Context.Channel.SendMessageAsync(outputMessage);
        }
    }
}
