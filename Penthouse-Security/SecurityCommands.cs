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
            string path = @"Resources/_8ball.txt";
            if (File.Exists(path))
            {
                var contents = File.ReadAllText(path);
                var b64str = Convert.FromBase64String(contents);
                string[] lines = Encoding.Default.GetString(b64str).Split("\n");

                answers = lines.ToList();
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
            await Context.Message.DeleteAsync();
            string outputMessage = (new LetterParser()).parse(message);
            await Context.Channel.SendMessageAsync("**" + Context.User.Username + "**: " + outputMessage);
            
        }

        [Command("roll")]
        public async Task Roll()
        {
            string username = Context.User.Username;

            var randomValue = new Random().Next(1, 101);
            string additionalText = "";
            if (randomValue == 69) additionalText = ". Mam 5 lat i bawi mnie ta liczba.";
            if (randomValue == 100) additionalText = ". ociechuj...";
            if (randomValue == 1) additionalText = ". frajer xD.";

            await Context.Channel.SendMessageAsync(username + " rolled: " + "**" + randomValue + "**" + additionalText);
        }
        
        [Command("8ball")]
        public async Task _8ball([Remainder] string message)
        {
            await czy(message);
        }

        [Command("8ball")]
        public async Task _8ball_emptyInputOverload()
        {
            await czy_emptyInputOverload();
        }

        [Command("czy")]
        public async Task czy([Remainder] string message)
        {
            var randomLine = new Random().Next(0, answers.Count);
            await Context.Channel.SendMessageAsync(answers.ElementAt(randomLine));            
        }

        [Command("czy")]
        public async Task czy_emptyInputOverload()
        {
            await Context.Channel.SendMessageAsync("Dobrze ale może zadaj jakieś pytanie ty jebany kmieciu?");
            return;
        }
    }
}
