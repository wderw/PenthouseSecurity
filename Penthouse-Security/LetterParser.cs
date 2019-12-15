using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Penthouse_Security
{
    class LetterParser
    {
        //TODO: Add VerySpecialStrings -> eg. letter P becomes :parking: ; letter o becomes :o2:

        private readonly static string regInd = "regional_indicator_";
        private readonly static string[] numbers = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        private readonly static List<string> specialStrings = new List<string>
        {
            "cool",  "free",  "abcd", "new", "abc", "atm", "sos", "ab", "ok", "vs", "cl", "id", "wc", "ng", "up", "a", "b", "m"
        };

        public LetterParser() {}

        private bool CharIsANumber(char c)
        {
            return c >= 48 && c <= 57;
        }

        private string WrapString(string s)
        {
            return ":" + s + ":";
        }

        private string GetCharacterRepresentation(char c)
        {
            if (CharIsANumber(c))
                return numbers[c - 48];
            else
                return regInd + c;
        }

        private List<int> FindAllIndexesOf(string input, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("String to find is empty", "value");
            List<int> allIndexes = new List<int>();
            for (int index = 0; ; index += value.Length)
            {
                index = input.IndexOf(value, index);
                if (index == -1)
                    return allIndexes;
                allIndexes.Add(index);
            }
        }

        private bool IsLuckOnMySide()
        {
            Random rnd = Utils.random;
            int lottery = rnd.Next(100);
            return lottery >= 50;
        }

        private string ConvertWord(string word)
        {
            string output = "";

            Dictionary<int, string> parsedCharacters = new Dictionary<int, string>();

            string wordNoSpecials = word;
            foreach (var special in specialStrings)
            {
                string wordWithoutCurrentSpecial = wordNoSpecials;
                foreach (var index in FindAllIndexesOf(wordWithoutCurrentSpecial, special))
                {
                    if (IsLuckOnMySide())
                    {
                        parsedCharacters[index] = special;
                        var stringbuilder = new StringBuilder(wordWithoutCurrentSpecial);
                        stringbuilder.Remove(index, special.Length);
                        stringbuilder.Insert(index, " ", special.Length);
                        wordWithoutCurrentSpecial = stringbuilder.ToString();
                    }
                }
                wordNoSpecials = wordWithoutCurrentSpecial;
            }

            for (int i = 0; i < wordNoSpecials.Length; i++)
            {
                var character = wordNoSpecials[i];
                if (character != ' ')
                    parsedCharacters[i] = GetCharacterRepresentation(character);
            }

            SortedDictionary<int, string> sortedParsedCharacters = new SortedDictionary<int, string>(parsedCharacters);

            foreach (var character in sortedParsedCharacters)
            {
                if (character.Value == " ")
                    continue;

                output += WrapString(character.Value);
            }

            return output;
        }

        public string Parse(string inputString)
        {
            var s = Regex.Replace(inputString, "[A-Z]", m => m.ToString().ToLower());
            var fixedInput = Regex.Replace(s, "[^a-zA-Z0-9 ]", string.Empty);

            var splittedInput = fixedInput.Split(' ');
            
            string output = "";

            foreach (var word in splittedInput)
            {
                output += ConvertWord(word);
                output += "  ";
            }

            return output;
        }
    }
}
