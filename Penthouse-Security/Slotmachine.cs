using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    class Slotmachine
    {
        private int[] scores = new int[] { 1, 10, 100, 1000 };

        private static Dictionary<int, string> icons = new Dictionary<int, string>
        {
            { 0, ":onion:" },
            { 1, ":cherries:" },
            { 2, ":gem:" },
            { 3, ":seven:" }
        };

        public static async Task<Embed> SlotDescription()
        {
            var description = new EmbedBuilder();

            description.WithTitle("**Slut machine (v0.2 alpha af):**");
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

        public string Spin()
        {
            var result = new StringBuilder();
            var random = new Random();

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
                        break;
                    case 2:
                        result.Append("Twoje combo to: **gemixowy full**");
                        break;
                    case 3:
                        result.Append("Twoje combo to: **karoca generala pedala**");
                        break;
                    default:
                        result.Append("Twoje combo to: **pełny cebularski frajer**");
                        break;
                }
                result.AppendLine();
                return result.ToString();
            } // triple

            result.AppendLine();
            if (score == 210) { result.Append("Twoje combo to: **full retard**"); return result.ToString(); }
            if (score == 1200) { result.Append("Twoje combo to: **mala sciera**"); return result.ToString(); }
            if (score == 2100) { result.Append("Twoje combo to: **duza sciera**"); return result.ToString(); }
            if (score == 21 || score == 120 || score == 1020) { result.Append("Twoje combo to: **participation trophy**"); return result.ToString(); }

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
    }
}
