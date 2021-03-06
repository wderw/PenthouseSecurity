﻿using Discord.WebSocket;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Penthouse_Security
{
    class CommandHandler
    {
        DiscordSocketClient client;
        CommandService service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            this.client = client;
            service = new CommandService();

            await service.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            var msg = socketMessage as SocketUserMessage;

            if (msg == null) return;

            var context = new SocketCommandContext(client, msg);
            int argPos = 0;

            if(msg.HasStringPrefix(Config.vars["cmdPrefix"], ref argPos) ||
                msg.HasMentionPrefix(client.CurrentUser, ref argPos)
                )
            {
                IResult result;
                result = await service.ExecuteAsync(context, argPos, null);
                
                if(!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Log.Error(result.ErrorReason);
                }
            }
        }
    }
}
