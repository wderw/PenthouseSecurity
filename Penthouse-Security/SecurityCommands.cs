using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            var helpMessage = new StringBuilder();
            helpMessage.AppendLine("**Available commands:**");
            helpMessage.AppendLine("!echo - *convert text with style*");
            helpMessage.AppendLine("!roll - *roll 0 - 100*");
            helpMessage.AppendLine("!uptime - *show bot uptime*");
            helpMessage.AppendLine("!8ball - *get an answer to a question*");
            helpMessage.AppendLine("!weather - *check weather conditions*");

            await Context.Channel.SendMessageAsync(helpMessage.ToString());
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
            await Czy(message);
        }

        [Command("8ball")]
        public async Task _8ball_emptyInputOverload()
        {
            await Czy_emptyInputOverload();
        }

        [Command("czy")]
        public async Task Czy([Remainder] string message)
        {
            var randomLine = new Random().Next(0, answers.Count);
            await Context.Channel.SendMessageAsync(answers.ElementAt(randomLine));
        }

        [Command("czy")]
        public async Task Czy_emptyInputOverload()
        {
            await Context.Channel.SendMessageAsync("Dobrze ale może zadaj jakieś pytanie ty jebany kmieciu?");
        }

        [Command("uptime")]
        public async Task UptimeQuery()
        {
            var uptime = Utils.GetUptime();
            await Context.Channel.SendMessageAsync("Nie wyjebalem sie z rowerka od: " +
                uptime.Days + " dni, " +
                uptime.Hours + " godzin, " +
                uptime.Minutes + " minut i " +
                uptime.Seconds + " sek.");
            return;
        }

        [Command("weather")]
        public async Task WeatherReport([Remainder] string city)
        {
            var weatherService = new WeatherService();
            string jsonResponse = string.Empty;
            string url = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&APPID=53bbda84142d7bb7062a43eabe3ea303";

            try
            {
                jsonResponse = await weatherService.GetWeather(url);
            }
            catch (WebException e)
            {
                Log.Error("WebException: " + e.ToString());
                if (e.Message == "The remote server returned an error: (404) Not Found.")
                {
                    await Context.Channel.SendMessageAsync("Ale wiesz kmieciu że nie ma takiego miasta na jebanym świecie?");
                }

                return;
            }
            catch (Exception e)
            {
                Log.Error("WebClient Unknown error: " + e.Message);
                return;
            }

            JObject weather;
            try
            {
                weather = JObject.Parse(jsonResponse);
            }
            catch (JsonReaderException e)
            {
                Log.Error("Invalid json format: " + e.ToString());
                return;
            }

            string report = weatherService.GetReport(weather);

            await Context.Channel.SendMessageAsync(report);
        }

        [Command("forecast")]
        public async Task WeatherForecast([Remainder] string city)
        {
            var weatherService = new WeatherService();
            string jsonResponse = string.Empty;
            string url = "http://api.openweathermap.org/data/2.5/forecast?q=" + city + "&APPID=53bbda84142d7bb7062a43eabe3ea303";

            try
            {
                jsonResponse = await weatherService.GetWeather(url);
            }
            catch (WebException e)
            {
                Log.Error("WebException: " + e.ToString());
                if(e.Message == "The remote server returned an error: (404) Not Found.")
                {
                    await Context.Channel.SendMessageAsync("Ale wiesz kmieciu że nie ma takiego miasta na jebanym świecie?");
                }

                return;
            }
            catch (Exception e)
            {
                Log.Error("WebClient Unknown error: " + e.Message);
                return;
            }

            JObject weather;
            try
            {
                weather = JObject.Parse(jsonResponse);
            }
            catch (JsonReaderException e)
            {
                Log.Error("Invalid json format: " + e.ToString());
                return;
            }

            string forecast = weatherService.GetForecast(weather);

            await Context.Channel.SendMessageAsync(forecast);
        }
    }
}
