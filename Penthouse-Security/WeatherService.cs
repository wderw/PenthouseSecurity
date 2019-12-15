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
        private struct WeatherInfo
        {
            public int temperature;
            public string region;
            public int humidity;
            public double windSpeed;
            public int pressure;
            public string city;
            public string cloudiness;
            public int id;
        }

        private struct QuickInfo
        {
            public QuickInfo(string d, string i)
            {
                description = d;
                icon = i;
            }

            public string description;
            public string icon;
        }

        private readonly static Dictionary<int, QuickInfo> skyInfos = new Dictionary<int, QuickInfo>
        {
            { 800, new QuickInfo("czysto whuj", ":sunny:")},
            { 801, new QuickInfo("no jest chmura czy dwie i ta", ":white_sun_small_cloud:")},
            { 802, new QuickInfo("chmury sa tu i tam.", ":cloud:")},
            { 803, new QuickInfo("rozjebane chmury sa podobno.", ":cloud:")},
            { 804, new QuickInfo("chuja widac.", ":cloud:")},
        };

        public async Task<string> GetWeather(string url)
        {
            using (WebClient client = new WebClient())
            {
                return client.DownloadString(url);
            }
        }

        private QuickInfo GetQuickInfoById(int id)
        {

            string icon;
            string description;
            if (id >= 200 && id < 300)
            {
                description = "przyszla ona :zaba2:";
                icon = ":cloud_lightning:";
            }
            else if (id >= 300 && id < 400)
            {
                description = "letko deszczy.";
                icon = ":cloud_rain:";
            }
            else if (id >= 500 && id < 600)
            {
                description = "pizga deszczem a ty jeszcze.";
                icon = ":cloud_rain:";
            }
            else if (id >= 600 && id < 700)
            {
                description = "śniegiem kurvi az milo.";
                icon = ":cloud_snow:";
            }
            else if (id >= 700 && id < 800)
            {
                description = "lokurwa tam to tornado maja XD RIP.";
                icon = ":cloud_tornado:";
            }
            else
            {
                description = skyInfos[id].description;
                icon = skyInfos[id].icon;
            }

            return new QuickInfo(description, icon);
        }
        private double FahrenheitToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32) * 5 / 9;
        }

        private int KelvinToCelsius(double kelvin)
        {
            return (int)(kelvin - 274.15);
        }

        public string GetForecast(JObject weather)
        {
            var forecast = new StringBuilder();

            forecast.Append("Forekast dla szithola jakim jest **" + weather.SelectToken("$.city.name").Value<string>() + "**:");
            forecast.AppendLine();
            forecast.AppendLine();

            var dayCount = 0;

            for(int i = 0; i < 40; i += 8)
            {
                var info = new WeatherInfo
                {
                    temperature = KelvinToCelsius(weather.SelectToken("$.list[" + i + "].main.temp").Value<int>()),

                    // unused in forecast
                    //info.region = weather.SelectToken("$.list[" + i + "].sys.country").Value<string>();

                    humidity = weather.SelectToken("$.list[" + i + "].main.humidity").Value<int>(),
                    windSpeed = weather.SelectToken("$.list[" + i + "].wind.speed").Value<double>(),
                    pressure = weather.SelectToken("$.list[" + i + "].main.pressure").Value<int>(),
                    city = weather.SelectToken("$.city.name").Value<string>(),
                    cloudiness = weather.SelectToken("$.list[" + i + "].clouds.all").Value<string>(),
                    id = weather.SelectToken("$.list[" + i + "].weather[0].id").Value<int>()
                };

                var today = (DateTime.Now + new TimeSpan(dayCount, 0, 0, 0)).DayOfWeek;

                var quickInfo = GetQuickInfoById(info.id);

                forecast.Append(quickInfo.icon + " ");
                forecast.Append(info.temperature + "°C ");
                forecast.Append(" `" + today + "`");                
                forecast.AppendLine();

                dayCount++;
            }

            return forecast.ToString();
        }
        
        public string GetReport(JObject weather)
        {
            WeatherInfo info;

            info.temperature = KelvinToCelsius(weather.SelectToken("$.main.temp").Value<int>());
            info.region = weather.SelectToken("$.sys.country").Value<string>();
            info.humidity = weather.SelectToken("$.main.humidity").Value<int>();
            info.windSpeed = weather.SelectToken("$.wind.speed").Value<double>();
            info.pressure = weather.SelectToken("$.main.pressure").Value<int>();
            info.city = weather.SelectToken("$.name").Value<string>();
            info.cloudiness = weather.SelectToken("$.clouds.all").Value<string>();
            info.id = weather.SelectToken("$.weather[0].id").Value<int>();

            var report = new StringBuilder();

            if (info.region == "FR")
                report.Append("Le ");

            report.AppendLine("Raport pogodowy na ździś dla wygwizdowa jakim jest **" + info.city + "**:");
            report.AppendLine();

            if (info.region == "FR")
                report.Append("Le ");

            if (info.region == "JP")
                report.AppendLine("Zobaczmy co tam u *chinoli* xD. ");

            if (info.region == "PL")
                report.AppendLine("Wykryto region: **cebulandia**. ");

            report.Append("Temperatura to wynosi to **" + info.temperature.ToString() + "** stopni celcjusza, ");

            if (info.temperature < 10 && info.temperature >= 0)
            {
                report.AppendLine("czyli troche pizga zimnem ale nie tragedia. ");
            }
            else if (info.temperature >= 10 && info.temperature < 30)
            {
                report.AppendLine("czyli nie pizga zimnem whuj ale zajebiscie tez nie jest. ");
            }
            else if (info.temperature < 0)
            {
                report.AppendLine("czyli mamy ponizej zera ojapierdole. ");
            }
            else
            {
                report.AppendLine("czyli znaczy sie ze napierdala goruncem jak nieopamientany chuj. ");
            }

            report.AppendLine("Cisnienie na ryj wynosi **" + info.pressure + "** hektorpaskali. ");
            report.AppendLine("Hujmiditi procentowo wychodzi ze **" + info.humidity + " %**. ");
            report.AppendLine("Wiater zapierdala z predkoscia **" + info.windSpeed + "** metra na sekunde kwadrat.");
            report.AppendLine("Zahmurzenie procentowe **" + info.cloudiness + " %**. ");
            report.AppendLine();
            report.AppendLine("Rysuje sie to mniej wiecej tak: ");

            var quickInfo = GetQuickInfoById(info.id);
            string icon = quickInfo.icon;
            string description = quickInfo.description;

            report.AppendLine(icon);
            report.AppendLine();
            report.AppendLine("Ogolnie to " + description);

            return report.ToString();
        }
    }
}
