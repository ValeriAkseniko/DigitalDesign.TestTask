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
            return validWordSymbols.Contains(symbol);
        }

        private string GetFilePath()
        {
            bool isCorrectPath = false;
            Console.WriteLine(Messages.InputPath);
            string path = Console.ReadLine();
            isCorrectPath = File.Exists(path);
            while (!isCorrectPath)
            {
                Console.WriteLine(Messages.InvalidInputPath);
                Console.WriteLine(Messages.InputPath);
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
                    string wordOnTwoLines = null;
                    while ((line = sr.ReadLine()) != null)
                    {
                        List<string> words = GetListWords(line);
                        if (wordOnTwoLines != null)
                        {
                            wordOnTwoLines = wordOnTwoLines.Trim('-');
                            if (words.Count > 0)
                            {
                                words[0] = wordOnTwoLines + words[0];
                            }                            
                            wordOnTwoLines = null;
                        }
                        string lastWord = words.Last();
                        if (lastWord != null)
                        {
                            char lastSymbol = lastWord.Last();
                            if (IsValidWordSymbols(lastSymbol))
                            {
                                wordOnTwoLines = words.Last();
                                words.Remove(words.Last());
                            }
                        }
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
            words = RemoveWordsConsistingOnlyOfValidSymbol(words);
            return words;

        }

        private List<string> RemoveWordsConsistingOnlyOfValidSymbol(List<string> words)
        {
            words = words.Where(x => !(x.Length == 1 && IsValidWordSymbols(x.First()))).ToList();
            return words;
        }

        private void AddToCountWords(List<string> words, Dictionary<string, int> wordsCount)
        {
            foreach (var word in words)
            {
                if (!wordsCount.ContainsKey(word))
                {
                    wordsCount.Add(word, 1);
                }
                else
                {
                    wordsCount[word]++;
                }
            }
        }

        private void RecordTxtFile(List<KeyValuePair<string, int>> wordsCountList,string path)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    foreach (KeyValuePair<string, int> valuePair in wordsCountList)
                    {
                        sw.WriteLine($"{valuePair.Key} - {valuePair.Value}");
                    }
                }
                Console.WriteLine(Messages.RecordingCompleted);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private string GetDirectoryPath()
        {
            Console.WriteLine(Messages.OutputDirectoryPath);
            string path = Console.ReadLine();
            bool isCorrecPath = Directory.Exists(path);
            while (!isCorrecPath)
            {
                Console.WriteLine(Messages.InvalidOutputPath);
                Console.WriteLine(Messages.OutputDirectoryPath);
                path = Console.ReadLine();
                isCorrecPath = Directory.Exists(path);
            }
            return path;
        }

        public void Execute()
        {
            string path = GetFilePath();
            Dictionary<string, int> wordsCount = GetStatisticWords(path);
            List<KeyValuePair<string, int>> wordsCountList = wordsCount.OrderByDescending(x => x.Value).ToList();
            path = GetDirectoryPath();
            RecordTxtFile(wordsCountList,$@"{path}\result.txt");
        }
    }
}
