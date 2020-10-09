using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    class WeatherService
    {
        private class InvalidAppIdException : Exception {}
        private struct WeatherInfo
        {
            public int dt;
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

        public string openWeatherAppId { get; }

        public WeatherService()
        {
            openWeatherAppId = Environment.GetEnvironmentVariable(Config.vars["openWeatherAppId"]);

            if (openWeatherAppId == "" || openWeatherAppId == null)
            {
                Log.Error("Open weather AppId is invalid!");
                throw new InvalidAppIdException();
            }
        }

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

                    dt = weather.SelectToken("$.list[" + i + "].dt").Value<int>(),
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

                var datetime = DateTimeOffset.FromUnixTimeSeconds(info.dt);

                Log.Debug(datetime.Hour.ToString() + ":" + datetime.Minute.ToString());


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

            report.AppendLine("Weather - **" + info.city + "**:");
            report.AppendLine();

            var quickInfo = GetQuickInfoById(info.id);
            string icon = quickInfo.icon;

            report.Append(icon);
            report.AppendLine(" **" + info.temperature.ToString() + " °C**");

            report.AppendLine("pressure: **" + info.pressure + " hPa**");
            report.AppendLine("humidity: **" + info.humidity + " %**");
            report.AppendLine("wind: **" + info.windSpeed + " m/s**");
            report.AppendLine("cloudiness: **" + info.cloudiness + " %**");
            report.AppendLine();

            return report.ToString();
        }
    }
}
