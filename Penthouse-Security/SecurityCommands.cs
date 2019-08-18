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
        [Command("echo")]
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
                "!echo - *convert text with style* \n" +
                "!roll - *roll 0 - 100*";

            await Context.Channel.SendMessageAsync(helpMessage);
        }     
    }
}
