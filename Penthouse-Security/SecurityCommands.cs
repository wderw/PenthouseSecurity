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
            var difference = Utils.GetTimeToPapaHour();

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

        [Command("roll")]
        public async Task Roll()
        {
            string username = Context.User.Username;
            await Context.Channel.SendMessageAsync(username + " rolled: " + new Random().Next(1,101));
        }

        [Command("help")]
        public async Task Display()
        {
            string helpMessage = "**Available commands:** \n" +
                "!anime - *perform a reality check* \n" +
                "!time - *get time to papaj hours* \n" +
                "!parse - *convert text with style* \n" +
                "!repeat - *echo text* \n" +
                "!roll - *roll 0 - 100*";

            await Context.Channel.SendMessageAsync(helpMessage);
        }
    }
}
