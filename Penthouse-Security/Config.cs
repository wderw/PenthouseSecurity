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
                { "botTokenKey", "BOT_TOKEN" }
            };
    }
}
