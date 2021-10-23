using ClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DigitalDesign.TestTask
{
    class Task2CountWords
    {
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

        private void RecordTxtFile(List<KeyValuePair<string, int>> wordsCountList, string path)
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

        private Dictionary<string, int> GetWordsCount(string txt)
        {
            Type type = typeof(TextToDictionary);
            MethodInfo methodInfos = type.GetMethod("ToDictionary", BindingFlags.NonPublic | BindingFlags.Static);
            object[] paprametrs = new object[] { txt };
            object resultInvoke = methodInfos.Invoke(type, paprametrs);
            Dictionary<string, int> wordsCount = (Dictionary<string, int>)resultInvoke;
            return wordsCount;
        }

        private Dictionary<string, int> GetUnionDictionary(List<Dictionary<string, int>> listDictionary)
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            foreach (Dictionary<string, int> dictionaryByLine in listDictionary)
            {
                foreach (KeyValuePair<string, int> item in dictionaryByLine)
                {
                    if (!result.Keys.Contains(item.Key))
                    {
                        result.Add(item.Key, item.Value);
                    }
                    else
                    {
                        result[item.Key] += item.Value;
                    }
                }
            }
            return result;
        }        

        public void Execute()
        {
            Stopwatch stopwatch = new Stopwatch();

            string path = GetFilePath();
            string txt;
            List<string> lines = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((txt = sr.ReadLine()) != null)
                    {
                        lines.Add(txt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            var resultWordsCount = new Dictionary<string, int>();
            stopwatch.Start();
            foreach (var line in lines)
            {
                var wordsCount = GetWordsCount(line);
                AddToDictionary(wordsCount, resultWordsCount);
            }
            stopwatch.Stop();

            var wordsCountList = resultWordsCount.OrderByDescending(x => x.Value).ToList();
            path = GetDirectoryPath();
            RecordTxtFile(wordsCountList, $@"{path}\result.txt");
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        private static void AddToDictionary(Dictionary<string, int> wordsCount, Dictionary<string, int> resultWordsCount)
        {
            foreach (var wordCount in wordsCount)
            {
                if (resultWordsCount.Keys.Contains(wordCount.Key))
                {
                    resultWordsCount[wordCount.Key] += wordCount.Value;
                }
                else
                {
                    resultWordsCount.Add(wordCount.Key, wordCount.Value);
                }
            }
        }

        public void ExecuteParallel()
        {
            Stopwatch stopwatch = new Stopwatch();

            string path = GetFilePath();
            List<string> lines = new List<string>();
            string txt;
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while ((txt = sr.ReadLine()) != null)
                    {
                        lines.Add(txt);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            var resultWordsCount = new Dictionary<string, int>();
            stopwatch.Start();
            Parallel.ForEach(lines,
                line =>
                {
                    lock (resultWordsCount)
                    {
                        var wordsCount = GetWordsCount(line);
                        AddToDictionary(wordsCount, resultWordsCount);
                    }
                });
            stopwatch.Stop();

            var wordsCountList = resultWordsCount.OrderByDescending(x => x.Value).ToList();
            path = GetDirectoryPath();
            RecordTxtFile(wordsCountList, $@"{path}\result.txt");
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }
    }
}