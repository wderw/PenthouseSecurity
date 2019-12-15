using System;
using System.Collections.Generic;
using System.Text;

namespace Penthouse_Security
{
    public class Config
    {
        public static Dictionary<string, string> vars = new Dictionary<string, string>
        {
            { "cmdPrefix", "!" },
            { "botTokenKey", "BOT_TOKEN" },
            { "securityBotMongoPassword", "MONGO_PASSWD" },
            { "okragly_stul_jp2", "<:jp2:275394520563187715>" },
            { "ogólny_jp2", "<:jp2:607634357439692800>" },
            { "discordApiLogTag", "[DISCORD API]" },
            { "applicationLogTag", "[APPLICATION]" }
        };
    }
}
