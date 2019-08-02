using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    class Program
    {
        DiscordSocketClient client;
        CommandHandler handler;

        string token = Environment.GetEnvironmentVariable("BOT_TOKEN");

        public static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            if (token == "" || token == null) return;

            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
            });

            client.Log += Log;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            handler = new CommandHandler();
            await handler.InitializeAsync(client);
            await Task.Delay(-1);
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}
