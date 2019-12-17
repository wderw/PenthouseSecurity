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
        internal static Services services;

        public static void Main(string[] args)
        => new Program().StartAsync().GetAwaiter().GetResult();

        private async Task StartAsync()
        {
            var mongoPasswd = InitializeMongoPassword();
            var token = InitializeBotToken();
            InitializeClientLogger();

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            handler = new CommandHandler();
            await handler.InitializeAsync(client);

            services = new Services(client, mongoPasswd);

            await Task.Delay(-1);            
        }

        private string InitializeMongoPassword()
        {
            string passwd = Environment.GetEnvironmentVariable(Config.vars["securityBotMongoPassword"]);

            if (passwd == "" || passwd == null)
            {
                Log.Error("Bot Mongo password is invalid!");
                throw new InvalidBotPasswordException();
            }

            return passwd;
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
        private class InvalidBotPasswordException : Exception {}

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
