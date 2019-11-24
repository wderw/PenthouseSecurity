using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    class Slotmachine
    {
        private static Dictionary<int, string> icons = new Dictionary<int, string>
        {
            { 0, ":onion:" },
            { 1, ":cherries:" },
            { 2, ":lemon:" },             
            { 3, ":watermelon:" },
            { 4, ":game_die:" },
            { 5, ":moneybag:" },
            { 6, ":seven:" }
        };

        public static async Task<Embed> SlotDescription()
        {
            var description = new EmbedBuilder();

            description.WithTitle("**Slut machine:**");
            description.AddField("1) " + ":onion:" + " - frajer", "----------", false);
            description.AddField("2) " + ":cherries:", "----------", false);
            description.AddField("3) " + ":lemon:", "----------", false);
            description.AddField("4) " + ":watermelon:", "----------", false);
            description.AddField("5) " + ":game_die:", "----------", false);
            description.AddField("6) " + ":moneybag:", "----------", false);
            description.AddField("7) " + ":seven:", "----------", false);

            return description.Build();
        }

        public string Spin()
        {
            var result = new StringBuilder();
            var random = new Random();

            int value1 = ClampRoll(random.Next(100));
            int value2 = ClampRoll(random.Next(100));
            int value3 = ClampRoll(random.Next(100));

            result.Append(icons[value1] + " " + icons[value2] + " " + icons[value3]);

            if(value1 == value2 && value2 == value3)
            {
                result.AppendLine();
                result.AppendLine();
                result.Append("**Ozesz kurwasz** gdyby nie wersja demo to pewnie bys cos wygral :slight_smile:");
            }
            return result.ToString();
        }

        private int ClampRoll(int original)
        {
            if(original < 2)
            {
                return 6;
            }
            else if(original >= 2 && original < 4)
            {
                return 5;
            }
            else if(original >= 4 && original < 8)
            {
                return 4;
            }
            else if (original >= 8 && original < 16)
            {
                return 3;
            }
            else if (original >= 16 && original < 32)
            {
                return 2;
            }
            else if (original >= 32 && original < 64)
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
