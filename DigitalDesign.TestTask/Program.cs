using System;
using System.Threading.Tasks;

namespace DigitalDesign.TestTask
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Task2CountWords task2CountWords = new Task2CountWords();

            await task2CountWords.ExecuteWebApiAsync();
        }
    }
}
