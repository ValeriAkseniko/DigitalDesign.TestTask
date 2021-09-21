using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DigitalDesign.TestTask
{
    static class Task2CountWords
    {
        private static string GetFilePath()
        {
            bool isCorrectPath = false;
            Console.WriteLine("Введите путь для чтения");
            string path = Console.ReadLine();
            isCorrectPath = File.Exists(path);
            while (!isCorrectPath)
            {
                Console.WriteLine("Не корректный путь для чтения");
                Console.WriteLine("Введите путь");
                path = Console.ReadLine();
                isCorrectPath = File.Exists(path);
            }
            return path;
        }

        public static void GetStatisticWords()
        {
            string path = GetFilePath();
            Dictionary<string, int> wordsCount = new Dictionary<string, int>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        List<string> words = ListWords(line);
                        GetDictionary(words, wordsCount);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            RecordTxtFile(wordsCount);
        }

        private static List<string> ListWords(string line)
        {
            line = line.ToLower();
            line = line.Trim();
            char[] chars = line.ToCharArray();
            chars = Array.FindAll<char>(chars, (c => char.IsLetter(c)
                                              || char.IsWhiteSpace(c)));
            line = new string(chars);
            while (line.Contains("  "))
            {
                line = line.Replace("  ", " ");
            }
            List<string> words = line.Split(' ').ToList();
            return words;
        }

        private static void GetDictionary(List<string> words, Dictionary<string, int> wordsCount)
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

        private static void RecordTxtFile(Dictionary<string,int> wordsCount)
        {
            List<KeyValuePair<string, int>> wordsCountList = wordsCount.OrderByDescending(x => x.Value).ToList();
            Console.WriteLine("Введите путь для записи");
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
                Console.WriteLine("Запись выполнена");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
