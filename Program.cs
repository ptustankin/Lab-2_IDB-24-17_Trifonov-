using Lab2_Task11;
using Lab2_Task12;
using Lab2_Task2_Async;
using Lab2_Task2_Sync;
using System;
using System.Text;

namespace Lab2
{
    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Task11.Run();
            // Task2Async.Run().GetAwaiter().GetResult();
        }
    }
}