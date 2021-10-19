using ClassLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DigitalDesign.TestTask
{
     class Task2CountWords
    {
        private static string GetFilePath()
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

        private static string TxtFile(string path)
        {
            string result;
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    result = sr.ReadToEnd();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        private static void RecordTxtFile(List<KeyValuePair<string, int>> wordsCountList,string path)
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

        private static string GetDirectoryPath()
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

        public static void Execute()
        {
            string path = GetFilePath();
            string txt = TxtFile(path);
            Type type = typeof(TextToDictionary);
            MethodInfo methodInfos = type.GetMethod("ToDictionary", BindingFlags.NonPublic);
            Dictionary<string, int> wordsCount = methodInfos.Invoke(type , txt);
            List<KeyValuePair<string, int>> wordsCountList = wordsCount.OrderByDescending(x => x.Value).ToList();
            path = GetDirectoryPath();
            RecordTxtFile(wordsCountList, $@"{path}\result.txt");
        }
    }
}