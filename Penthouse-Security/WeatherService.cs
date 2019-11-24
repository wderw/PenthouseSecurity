using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    class WeatherService
    {
        private class SkyInfo
        {
            public SkyInfo(string d, string i)
            {
                description = d;
                icon = i;
            }

            public string description;
            public string icon;
        }

        private static Dictionary<int, SkyInfo> skyInfos = new Dictionary<int, SkyInfo>
        {
            { 800, new SkyInfo("czysto whuj", ":sunny:")},
            { 801, new SkyInfo("no jest chmura czy dwie i ta", ":white_sun_small_cloud:")},
            { 802, new SkyInfo("chmury sa tu i tam.", ":cloud:")},
            { 803, new SkyInfo("rozjebane chmury sa podobno.", ":cloud:")},
            { 804, new SkyInfo("chuja widac.", ":cloud:")},
        };

        public async Task<string> GetWeather(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }

        public string GetReport(JObject weather)
        {
            var temperature = Utils.KelvinToCelsius(weather.SelectToken("$.main.temp").Value<int>());
            var region = weather.SelectToken("$.sys.country").Value<string>();
            var humidity = weather.SelectToken("$.main.humidity").Value<int>();
            var windSpeed = weather.SelectToken("$.wind.speed").Value<double>();
            var pressure = weather.SelectToken("$.main.pressure").Value<int>();
            var city = weather.SelectToken("$.name").Value<string>();
            var cloudiness = weather.SelectToken("$.clouds.all").Value<string>();
            var id = weather.SelectToken("$.weather[0].id").Value<int>();

            var report = new StringBuilder();

            if (region == "FR")
                report.Append("Le ");

            report.AppendLine("Raport pogodowy na ździś dla wygwizdowa jakim jest **" + city + "**:");
            report.AppendLine();

            if (region == "FR")
                report.Append("Le ");

            if (region == "JP")
                report.AppendLine("Zobaczmy co tam u *chinoli* xD. ");

            if (region == "PL")
                report.AppendLine("Wykryto region: **cebulandia**. ");

            report.Append("Temperatura to wynosi to **" + temperature.ToString() + "** stopni celcjusza, ");

            if (temperature < 10 && temperature >= 0)
            {
                report.AppendLine("czyli troche pizga zimnem ale nie tragedia. ");
            }
            else if (temperature >= 10 && temperature < 30)
            {
                report.AppendLine("czyli nie pizga zimnem whuj ale zajebiscie tez nie jest. ");
            }
            else if (temperature < 0)
            {
                report.AppendLine("czyli mamy ponizej zera ojapierdole. ");
            }
            else
            {
                report.AppendLine("czyli znaczy sie ze napierdala goruncem jak nieopamientany chuj. ");
            }

            report.AppendLine("Cisnienie na ryj wynosi **" + pressure + "** hektorpaskali. ");
            report.AppendLine("Hujmiditi procentowo wychodzi ze **" + humidity + " %**. ");
            report.AppendLine("Wiater zapierdala z predkoscia **" + windSpeed + "** metra na sekunde kwadrat.");
            report.AppendLine("Zahmurzenie procentowe **" + cloudiness + " %**. ");
            report.AppendLine();
            report.AppendLine("Rysuje sie to mniej wiecej tak: ");

            string icon = "";
            string description = "";
            if(id >= 200 && id < 300)
            {
                description = "Przyszla ona :zaba2:";
                icon = ":cloud_lightning:";
            }
            else if(id >= 300 && id < 400)
            {
                description = "Letko deszczy.";
                icon = ":cloud_rain:";
            }
            else if (id >= 500 && id < 600)
            {
                description = "Pizga deszczem a ty jeszcze.";
                icon = ":cloud_rain:";
            }
            else if (id >= 600 && id < 700)
            {
                description = "Śniegiem kurvi az milo.";
                icon = ":cloud_snow:";
            }
            else if (id >= 700 && id < 800)
            {
                description = "Lokurwa tam to tornado maja XD RIP.";
                icon = ":cloud_tornado:";
            }
            else
            {
                description = skyInfos[id].description;
                icon = skyInfos[id].icon;  
            }

            report.AppendLine(icon);
            report.AppendLine();
            report.AppendLine("Ogolnie to " + description);

            return report.ToString();
        }
    }
}
