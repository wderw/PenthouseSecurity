using System;
using System.Collections.Generic;
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
            await Context.Channel.SendMessageAsync("anime chuj");
        }
    }
}
