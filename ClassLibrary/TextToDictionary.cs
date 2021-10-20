using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class TextToDictionary
    {
        const string validWordSymbols = "-`";

        private static bool IsValidWordSymbols(char symbol)
        {
            return validWordSymbols.Contains(symbol);
        }

        private static List<string> RemoveWordsConsistingOnlyOfValidSymbol(List<string> words)
        {
            words = words.Where(x => !(x.Length == 1 && IsValidWordSymbols(x.First()))).ToList();
            return words;
        }

        private static List<string> GetListWords(string line)
        {
            line = line.ToLower();
            line = line.Trim();
            char[] chars = line.ToCharArray();
            chars = Array.FindAll<char>(chars, (c => char.IsLetter(c)
                                              || char.IsWhiteSpace(c) || IsValidWordSymbols(c)));
            line = new string(chars);
            while (line.Contains("  "))
            {
                line = line.Replace("  ", " ");
            }
            List<string> words = line.Split(' ').ToList();
            words = RemoveWordsConsistingOnlyOfValidSymbol(words);
            return words;

        }

        private static Dictionary<string, int> ToDictionary(string text)
        {
            List<string> words = GetListWords(text);
            Dictionary<string, int> result = new Dictionary<string, int>();
            for (int i = 0; i < words.Count; i++)
            {
                if (!result.ContainsKey(words[i]))
                {
                    result.Add(words[i], 1);
                }
                else
                {
                    result[words[i]]++;
                }
            }
            return result;
        }
    }
}
