using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lab2_Task12
{
    internal static class Task12
    {
        private const int DatasetCount = 15;
        private const int NumbersPerDataset = 100;
        private const int MaxConcurrentThreads = 4;

        private static readonly string DataFilePath = Path.Combine(AppContext.BaseDirectory, "datasets.txt");

        public static void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;

            EnsureDataFileExists(DataFilePath);

            List<int[]> datasets = LoadDatasets(DataFilePath);

            if (datasets.Count != DatasetCount)
            {
                throw new InvalidOperationException(
                    $"В файле должно быть {DatasetCount} наборов, но найдено {datasets.Count}.");
            }

            List<SetResult> results = new List<SetResult>();
            object resultsLock = new object();
            object consoleLock = new object();

            int totalSum = 0;

            Semaphore semaphore = new Semaphore(MaxConcurrentThreads, MaxConcurrentThreads);
            Mutex totalMutex = new Mutex();

            Thread[] threads = new Thread[DatasetCount];
            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < DatasetCount; i++)
            {
                int datasetNumber = i + 1;
                int[] numbers = datasets[i];

                threads[i] = new Thread(() =>
                {
                    semaphore.WaitOne();
                    try
                    {
                        int threadId = Thread.CurrentThread.ManagedThreadId;
                        int sum = 0;

                        for (int j = 0; j < numbers.Length; j++)
                        {
                            sum += numbers[j];
                        }

                        lock (resultsLock) // Monitor через lock
                        {
                            results.Add(new SetResult(datasetNumber, sum, threadId));
                        }

                        totalMutex.WaitOne();
                        try
                        {
                            totalSum += sum;
                        }
                        finally
                        {
                            totalMutex.ReleaseMutex();
                        }

                        lock (consoleLock)
                        {
                            Console.WriteLine($"Поток {threadId} обработал набор {datasetNumber}. Сумма = {sum}");
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                threads[i].Start();
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }

            stopwatch.Stop();

            results.Sort((a, b) => a.DatasetNumber.CompareTo(b.DatasetNumber));

            Console.WriteLine();
            Console.WriteLine("Результаты по наборам:");
            foreach (SetResult result in results)
            {
                Console.WriteLine(
                    $"Набор {result.DatasetNumber}: сумма = {result.Sum}, поток = {result.ThreadId}");
            }

            Console.WriteLine();
            Console.WriteLine($"Общий итог по всем наборам: {totalSum}");
            Console.WriteLine($"Время выполнения: {stopwatch.Elapsed}");
        }

        private static void EnsureDataFileExists(string path)
        {
            if (File.Exists(path))
                return;

            Random random = new Random();
            List<string> lines = new List<string>();

            for (int i = 0; i < DatasetCount; i++)
            {
                string[] numbers = new string[NumbersPerDataset];

                for (int j = 0; j < NumbersPerDataset; j++)
                {
                    numbers[j] = random.Next(1, 101).ToString();
                }

                lines.Add(string.Join(" ", numbers));
            }

            File.WriteAllLines(path, lines, Encoding.UTF8);
        }

        private static List<int[]> LoadDatasets(string path)
        {
            string[] lines = File.ReadAllLines(path, Encoding.UTF8);
            List<int[]> datasets = new List<int[]>();

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i]
                    .Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != NumbersPerDataset)
                {
                    throw new InvalidOperationException(
                        $"В наборе {i + 1} должно быть {NumbersPerDataset} чисел, но найдено {parts.Length}.");
                }

                int[] numbers = new int[NumbersPerDataset];
                for (int j = 0; j < NumbersPerDataset; j++)
                {
                    numbers[j] = int.Parse(parts[j]);
                }

                datasets.Add(numbers);
            }

            return datasets;
        }

        private sealed class SetResult
        {
            public int DatasetNumber { get; }
            public int Sum { get; }
            public int ThreadId { get; }

            public SetResult(int datasetNumber, int sum, int threadId)
            {
                DatasetNumber = datasetNumber;
                Sum = sum;
                ThreadId = threadId;
            }
        }
    }
}