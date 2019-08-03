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
