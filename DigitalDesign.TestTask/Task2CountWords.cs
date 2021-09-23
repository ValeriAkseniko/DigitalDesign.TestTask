using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DigitalDesign.TestTask
{
    class Task2CountWords
    {
        const string validWordSymbols = "-`";

        private bool IsValidWordSymbols(char symbol)
        {
            if (!validWordSymbols.Contains(symbol))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private string GetFilePath()
        {
            bool isCorrectPath = false;
            Console.WriteLine(Messeges.InputPath);
            string path = Console.ReadLine();
            isCorrectPath = File.Exists(path);
            while (!isCorrectPath)
            {
                Console.WriteLine(Messeges.InvalidPath);
                Console.WriteLine(Messeges.InputPath);
                path = Console.ReadLine();
                isCorrectPath = File.Exists(path);
            }
            return path;
        }

        private Dictionary<string, int> GetStatisticWords(string path)
        {
            Dictionary<string, int> wordsCount = new Dictionary<string, int>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        List<string> words = GetListWords(line);
                        AddToCountWords(words, wordsCount);
                    }
                    return wordsCount;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private List<string> GetListWords(string line)
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
            return words;
        }

        private void AddToCountWords(List<string> words, Dictionary<string, int> wordsCount)
        {
            for (int i = 0; i < words.Count; i++)
            {
                if (!wordsCount.ContainsKey(words[i]))
                {
                    wordsCount.Add(words[i], 1);
                }
                else
                {
                    wordsCount[words[i]]++;
                }
            }
        }

        private void RecordTxtFile(List<KeyValuePair<string, int>> wordsCountList)
        {            
            Console.WriteLine(Messeges.OutputPath);
            string path = Console.ReadLine();
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    foreach (KeyValuePair<string, int> valuePair in wordsCountList)
                    {
                        sw.WriteLine($"{valuePair.Key} - {valuePair.Value}");
                    }
                }
                Console.WriteLine(Messeges.RecordingCompleted);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Execute()
        {
            string path = GetFilePath();
            Dictionary<string, int> wordsCount = GetStatisticWords(path);
            List<KeyValuePair<string, int>> wordsCountList = wordsCount.OrderByDescending(x => x.Value).ToList();
            RecordTxtFile(wordsCountList);
        }
    }
}
