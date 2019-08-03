using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Penthouse_Security
{
    class LetterParser
    {
        //TODO: Add VerySpecialStrings -> eg. letter P becomes :parking: ; letter o becomes :o2:
        //TODO: Add randomizer

        static string regInd = "regional_indicator_";
        static string[] numbers = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        static List<string> specialStrings = new List<string>
        {
            "cool",  "free",  "abcd", "new", "abc", "atm", "sos", "ab", "ok", "vs", "cl", "id", "wc", "ng", "up", "a", "b", "m"
        };

        public LetterParser() {}

        private static Boolean charIsANumber(char c)
        {
            return c >= 48 && c <= 57;
        }

        private static string wrapString(string s)
        {
            return ":" + s + ":";
        }

        private static string getCharacterRepresentation(char c)
        {
            if (charIsANumber(c))
                return numbers[c - 48];
            else
                return regInd + c;
        }

        private static string convertWord(string word)
        {
            string output = "";

            Dictionary<int, string> characters = new Dictionary<int, string>();

            string wordNoSpecials = word;
            foreach (var special in specialStrings)
            {
                while(wordNoSpecials.Contains(special))
                {
                    try
                    {
                        int index = wordNoSpecials.IndexOf(special);
                        characters[index] = special;
                        var stringbuilder = new StringBuilder(wordNoSpecials);
                        stringbuilder.Remove(index, special.Length);
                        stringbuilder.Insert(index, " ", special.Length);
                        wordNoSpecials = stringbuilder.ToString();
                    }
                    catch (ArgumentNullException e)
                    {

                    }
                }
            }

            for (int i = 0; i < wordNoSpecials.Length; i++)
            {
                var character = wordNoSpecials[i];
                if (character != ' ')
                    characters[i] = getCharacterRepresentation(character);
            }

            SortedDictionary<int, string> sortedCharacters = new SortedDictionary<int, string>(characters);

            foreach (var character in sortedCharacters)
            {
                if (character.Value == " ")
                    continue;

                output += wrapString(character.Value);
            }

            return output;
        }

        public static string parse(string inputString)
        {
            var s = Regex.Replace(inputString, "[A-Z]", m => " " + m.ToString().ToLower());
            var fixedInput = Regex.Replace(s, "[^a-zA-Z0-9% ._]", string.Empty);

            var splittedInput = fixedInput.Split(' ');
            
            string output = "";

            foreach (var word in splittedInput)
            {
                output += convertWord(word);
                output += " ";
            }

            return output;
        }
    }
}
