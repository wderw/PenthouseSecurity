using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Penthouse_Security
{
    class Services
    {
        private Dictionary<string, bool> suspensionStatus;
        private readonly DiscordSocketClient discordClient;

        public static Services Instance
        {
            get
            {
                if(Program.services == null)
                {
                    Log.Error("Fatal error: Services not initialized!");
                    throw new SystemException();
                }
                return Program.services;
            }
        }        

        internal Slotmachine slotmachine { get; }
        internal WeatherService weatherService { get; }
        internal LetterParser letterParser { get; }
        internal DatabaseConnector dbConnector { get; }
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

            suspensionStatus = new Dictionary<string, bool> {{"slotmachine", false}};

            Log.Info("All services running.");
            Utils.MarkStartupTime();
        }

        public void Suspend(string serviceName)
        {
            suspensionStatus[serviceName] = true;
            Log.Info("Service " + serviceName + " suspended.");
        }

        public void Resume(string serviceName)
        {            
            suspensionStatus[serviceName] = false;
            Log.Info("Service " + serviceName + " resumed.");
        }

        public bool IsSuspended(string serviceName)
        {
            return suspensionStatus[serviceName];
        }
    }
}
