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
    }
}
