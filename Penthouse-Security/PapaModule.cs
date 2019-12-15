using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Penthouse_Security
{
    class PapaModule
    {
        public PapaModule(DiscordSocketClient discordClient)
        {
            new CallbackScheduler(10000, () =>
            {
                ulong id = 0;
                foreach (SocketGuild g in discordClient.Guilds)
                {
                    id = g.Id;
                }

                ulong channelId = 0;
                foreach (SocketChannel channel in discordClient.GetGuild(id).TextChannels)
                {
                    if (channel.ToString() == "ogólny" || channel.ToString() == "general" || channel.ToString() == "okragly_stul")
                    {
                        channelId = channel.Id;
                        break;
                    }
                }

                if (DateTime.Now.Hour == Utils.ToGMTHours(21) && DateTime.Now.Minute == 37)
                {
                    discordClient.GetGuild(id).GetTextChannel(channelId).SendMessageAsync(Config.vars["okragly_stul_jp2"]);
                    System.Threading.Thread.Sleep(70000);
                }
            });
        }
    }
}
