using Discord;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    class Slotmachine
    {
        private Random random;
        private DatabaseConnector dbConnector;
        private readonly int[] scores = new int[] { 1, 10, 100, 1000 };
        private readonly static Dictionary<int, string> icons = new Dictionary<int, string>
        {
            { 0, ":onion:" },
            { 1, ":cherries:" },
            { 2, ":gem:" },
            { 3, ":seven:" }
        };

        public Slotmachine(DatabaseConnector dbConnector)
        {
            random = Utils.random;
            this.dbConnector = dbConnector ?? throw new ArgumentNullException("dbConnector");
        }

        public static async Task<Embed> SlotDescription()
        {
            var description = new EmbedBuilder();

            description.WithTitle("**Slut machine v0.3:**");
            description.AddField("1) " + ":onion: :onion: :onion:" + " - pełny cebularski frajer", "----------", false);
            description.AddField("2) " + ":cherries: :cherries: :grey_question: - participation trophy", "----------", false);
            description.AddField("3) " + ":cherries: :cherries: :cherries: - czeresniakowa trujca", "----------", false);
            description.AddField("4) " + ":gem: :gem: :cherries: - full retard", "----------", false);
            description.AddField("5) " + ":gem: :gem: :gem: - gemixowy full", "----------", false);
            description.AddField("6) " + ":seven: :gem: :gem: - mala sciera", "----------", false);
            description.AddField("7) " + ":seven: :seven: :gem: - duza sciera", "----------", false);
            description.AddField("8) " + ":seven: :seven: :seven: - karoca generala pedala", "----------", false);

            return description.Build();
        }

        public async Task<int> GetMetric(string metricType, string fieldName)
        {
            var spinCollection = dbConnector.GetSpinCollection();
            var filter = new BsonDocument("metricType", metricType);
            var document = await spinCollection.Find(filter).Limit(1).SingleAsync();

            return document.GetElement(fieldName).Value.AsInt32;
        }

        public async void IncrementMetric(string metricType, string fieldName)
        {
            var currentCount = await GetMetric(metricType, fieldName);
            currentCount++;

            var spinCollection = dbConnector.GetSpinCollection();
            var filter = new BsonDocument("metricType", metricType);
            var update = Builders<BsonDocument>.Update.Set(fieldName, currentCount);
            spinCollection.UpdateOne(filter, update);
        }

        public async Task<string> Spin()
        {
            IncrementMetric("general", "spinCount");

            var result = new StringBuilder();

            int value1 = ClampRoll(random.Next(100));
            int value2 = ClampRoll(random.Next(100));
            int value3 = ClampRoll(random.Next(100));

            result.AppendLine(icons[value1] + " " + icons[value2] + " " + icons[value3]);

            int score = scores[value1] + scores[value2] + scores[value3];

            if (value1 == value2 && value2 == value3)
            {
                result.AppendLine();
                switch (value1)
                {
                    case 1:
                        result.Append("Twoje combo to: **czeresniakowa trujca**");
                        IncrementMetric("combos", "czeresniakowa trujca");
                        break;
                    case 2:
                        result.Append("Twoje combo to: **gemixowy full**");
                        IncrementMetric("combos", "gemixowy full");
                        break;
                    case 3:
                        result.Append("Twoje combo to: **karoca generala pedala**");
                        IncrementMetric("combos", "karoca generala pedala");
                        break;
                    default:
                        result.Append("Twoje combo to: **pełny cebularski frajer**");
                        IncrementMetric("combos", "pełny cebularski frajer");
                        break;
                }
                result.AppendLine();
                return result.ToString();
            } // triple

            result.AppendLine();
            if (score == 210) { result.Append("Twoje combo to: **full retard**"); IncrementMetric("combos", "full retard"); return result.ToString(); }
            if (score == 1200) { result.Append("Twoje combo to: **mala sciera**"); IncrementMetric("combos", "mala sciera"); return result.ToString(); }
            if (score == 2100) { result.Append("Twoje combo to: **duza sciera**"); IncrementMetric("combos", "duza sciera"); return result.ToString(); }
            if (score == 21 || score == 120 || score == 1020) { result.Append("Twoje combo to: **participation trophy**"); IncrementMetric("combos", "participation trophy"); return result.ToString(); }

            return result.ToString();
        }

        private int ClampRoll(int original)
        {
            if(original < 8)
            {
                return 3;
            }
            else if(original >= 8 && original < 26)
            {
                return 2;
            }
            else if(original >= 26 && original < 55)
            {
                return 1;
            }
            else
            {
                return 0;
            }                
        }

        internal async Task<Embed> SpinStats()
        {
            var spinCount = await GetMetric("general", "spinCount");
            double cebulas = await GetMetric("combos", "pełny cebularski frajer"); string cebulasRatio = (cebulas / spinCount).ToString("N3");
            double participation = await GetMetric("combos", "participation trophy"); string participationRatio = (participation / spinCount).ToString("N3");
            double czeresniakowa = await GetMetric("combos", "czeresniakowa trujca"); string czeresniakowaRatio = (czeresniakowa / spinCount).ToString("N3");
            double fullretard = await GetMetric("combos", "full retard"); string fullretardRatio = (fullretard / spinCount).ToString("N3");
            double gemixowyfull = await GetMetric("combos", "gemixowy full"); string gemixowyfullRatio = (gemixowyfull / spinCount).ToString("N3");
            double malasciera = await GetMetric("combos", "mala sciera"); string malascieraRatio = (malasciera / spinCount).ToString("N3");
            double duzasciera = await GetMetric("combos", "duza sciera"); string duzascieraRatio = (duzasciera / spinCount).ToString("N3");
            double karoca = await GetMetric("combos", "karoca generala pedala"); string karocaRatio = (karoca / spinCount).ToString("N5");

            var stats = new EmbedBuilder();

            stats.WithTitle("**Statsy slutmaszina (Total spins: " + spinCount + "):**");
            stats.AddField("1) " + ":onion: :onion: :onion:" + " - pełny cebularski frajer", cebulas.ToString() + " (" + cebulasRatio + "%)", false);
            stats.AddField("2) " + ":cherries: :cherries: :grey_question: - participation trophy", participation.ToString() + " (" + participationRatio + "%)", false);
            stats.AddField("3) " + ":cherries: :cherries: :cherries: - czeresniakowa trujca", czeresniakowa.ToString() + " (" + czeresniakowaRatio + "%)", false);
            stats.AddField("4) " + ":gem: :gem: :cherries: - full retard", fullretard.ToString() + " (" + fullretardRatio + "%)", false);
            stats.AddField("5) " + ":gem: :gem: :gem: - gemixowy full", gemixowyfull.ToString() + " (" + gemixowyfullRatio + "%)", false);
            stats.AddField("6) " + ":seven: :gem: :gem: - mala sciera", malasciera.ToString() + " (" + malascieraRatio + "%)", false);
            stats.AddField("7) " + ":seven: :seven: :gem: - duza sciera", duzasciera.ToString() + " (" + duzascieraRatio + "%)", false);
            stats.AddField("8) " + ":seven: :seven: :seven: - karoca generala pedala", karoca.ToString() + " (" + karocaRatio + "%)", false);

            return stats.Build();
        }
    }
}
