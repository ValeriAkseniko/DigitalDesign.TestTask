using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DigitalDesign.TestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            Task2CountWords task2CountWords = new Task2CountWords();
            task2CountWords.Execute();
            Console.ReadKey();
        }
    }
}
