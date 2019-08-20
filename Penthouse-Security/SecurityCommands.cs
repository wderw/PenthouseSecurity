using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Penthouse_Security
{
    public class SecurityCommands : ModuleBase<SocketCommandContext>
    {
        private static List<string> answers;

        static SecurityCommands()
        {
            answers = new List<string>();
            string path = @"Resources/_8ball.txt";
            if (File.Exists(path))
            {
                foreach (var line in File.ReadLines(path))
                {
                    answers.Add(line);
                }
            }
            else
            {
                Log.Error(path + " file does not exist!");
            }
        }

        [Command("help")]
        public async Task Display()
        {
            string helpMessage = "**Available commands:** \n" +
                "!echo - *convert text with style* \n" +
                "!roll - *roll 0 - 100* \n" +
                "!8ball - *get an answer to a question*";

            await Context.Channel.SendMessageAsync(helpMessage);
        }

        [Command("echo")]
        public async Task Parse([Remainder] string message)
        {
            string outputMessage = (new LetterParser()).parse(message);
            await Context.Channel.SendMessageAsync(outputMessage);
        }

        [Command("roll")]
        public async Task Roll()
        {
            string username = Context.User.Username;

            var randomValue = new Random().Next(1, 101);
            string additionalText = "";
            if (randomValue == 69) additionalText = ". hehehehehe.";
            if (randomValue == 100) additionalText = ". farciarski sqrviel.";
            if (randomValue == 1) additionalText = ". graty jeti xD";

            await Context.Channel.SendMessageAsync(username + " rolled: " + "**" + randomValue + "**" + additionalText);
        }
        
        [Command("8ball")]
        public async Task _8ball([Remainder] string message)
        {
            await _8ball2(message);
        }

        [Command("czy")]
        public async Task _8ball2([Remainder] string message)
        {
            var randomLine = new Random().Next(0, answers.Count);
            await Context.Channel.SendMessageAsync(answers.ElementAt(randomLine));            
        }
    }
}
