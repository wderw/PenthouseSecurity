﻿using Discord.WebSocket;
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
        internal WebsiteScraper websiteScraper { get; }
        internal MiasmaInfoservice miasmaInfoservice { get; }

        public Services(DiscordSocketClient discordClient, string mongoPasswd)
        {
            this.discordClient = discordClient;

            Log.Info("Initializing services...");

            dbConnector = new DatabaseConnector(mongoPasswd);
            letterParser = new LetterParser();
            slotmachine = new Slotmachine(dbConnector);
            weatherService = new WeatherService();
            papaModule = new PapaModule(discordClient);
            websiteScraper = new WebsiteScraper();
            miasmaInfoservice = new MiasmaInfoservice(websiteScraper);

            Log.Info("Done.");
            Utils.MarkStartupTime();
        }
    }
}
