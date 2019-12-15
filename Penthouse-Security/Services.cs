using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Penthouse_Security
{
    class Services
    {
        private readonly DiscordSocketClient discordClient;

        public static Services Instance
        {
            get
            {
                if(Application.services == null)
                {
                    Log.Error("Fatal error: Services not initialized!");
                    throw new SystemException();
                }
                return Application.services;
            }
        }

        internal Slotmachine slotmachine { get; }
        internal WeatherService weatherService { get; }
        internal LetterParser letterParser { get; }
        internal DatabaseConnector dbConnector { get; }
        internal Random random { get; }
        internal PapaModule papaModule { get; }

        public Services(DiscordSocketClient discordClient, string mongoPasswd)
        {
            this.discordClient = discordClient;

            Log.Info("Initializing services...");

            dbConnector = new DatabaseConnector(mongoPasswd);
            letterParser = new LetterParser();
            slotmachine = new Slotmachine(dbConnector);
            weatherService = new WeatherService();
            papaModule = new PapaModule(discordClient);

            Log.Info("All services running.");
            Utils.MarkStartupTime();
        }
    }
}
