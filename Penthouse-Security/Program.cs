using Discord;
using Discord.Commands;
using Discord.API;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Penthouse_Security
{
    class Program
    {
        DiscordSocketClient client;
        CommandHandler handler;

        public static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        private async Task StartAsync()
        {            
            var token = InitializeBotToken();
            InitializeClientLogger();

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            handler = new CommandHandler();
            await handler.InitializeAsync(client);

            new CallbackScheduler(10000, () =>
            {
                ulong id = 0;
                foreach (SocketGuild g in client.Guilds)
                {
                    id = g.Id;
                }

                ulong channelId = 0;
                foreach (SocketChannel channel in client.GetGuild(id).TextChannels)
                {
                    if (channel.ToString() == "ogólny"  || channel.ToString() == "general" || channel.ToString() == "okragly_stul")
                    {
                        channelId = channel.Id;
                        break;
                    }
                }

                if (DateTime.Now.Hour == Utils.ToGMTHours(21) && DateTime.Now.Minute == 37)
                {
                    client.GetGuild(id).GetTextChannel(channelId).SendMessageAsync(Config.vars["okragly_stul_jp2"]);
                    System.Threading.Thread.Sleep(70000);
                }                
            });

            await Task.Delay(-1);            
        }

        private string InitializeBotToken()
        {
            string token = Environment.GetEnvironmentVariable(Config.vars["botTokenKey"]);

            if (token == "" || token == null)
            {
                Log.Error("Bot token is invalid!");
                throw new InvalidBotTokenException();
            }

            return token;
        }
        private class InvalidBotTokenException : Exception {}

        private void InitializeClientLogger()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
            });

            client.Log += Log.ClientLog;
        }
    }
}
