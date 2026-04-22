using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Lab2_Task11
{
    internal static class Task11
    {
        private const int StartNumber = 1;
        private const int EndNumber = 10000;
        private const int ThreadCount = 4;

        public static void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;

            while (true)
            {
                Console.WriteLine("Задача 1.1");
                Console.WriteLine("1 - Monitor");
                Console.WriteLine("2 - Mutex");
                Console.WriteLine("3 - Semaphore");
                Console.WriteLine("0 - Выход");
                Console.Write("Выберите версию: ");

                string choice = Console.ReadLine() ?? string.Empty;

                Console.WriteLine();

                if (choice == "0")
                    return;

                switch (choice)
                {
                    case "1":
                        RunMonitorVersion();
                        break;

                    case "2":
                        RunMutexVersion();
                        break;

                    case "3":
                        RunSemaphoreVersion();
                        break;

                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }

                Console.WriteLine();
            }
        }

        private static void RunMonitorVersion()
        {
            int primeCount = 0;
            object countLock = new object();
            object consoleLock = new object();

            Action incrementPrimeCount = () =>
            {
                lock (countLock)
                {
                    primeCount++;
                }
            };

            RunPrimeCounting(
                "Monitor",
                incrementPrimeCount,
                () => primeCount,
                consoleLock);
        }

        private static void RunMutexVersion()
        {
            int primeCount = 0;
            object consoleLock = new object();
            Mutex mutex = new Mutex();

            Action incrementPrimeCount = () =>
            {
                mutex.WaitOne();
                try
                {
                    primeCount++;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            };

            RunPrimeCounting(
                "Mutex",
                incrementPrimeCount,
                () => primeCount,
                consoleLock);
        }

        private static void RunSemaphoreVersion()
        {
            int primeCount = 0;
            object consoleLock = new object();
            Semaphore semaphore = new Semaphore(1, 1);

            Action incrementPrimeCount = () =>
            {
                semaphore.WaitOne();
                try
                {
                    primeCount++;
                }
                finally
                {
                    semaphore.Release();
                }
            };

            RunPrimeCounting(
                "Semaphore",
                incrementPrimeCount,
                () => primeCount,
                consoleLock);
        }

        private static void RunPrimeCounting(
            string versionName,
            Action incrementPrimeCount,
            Func<int> getPrimeCount,
            object consoleLock)
        {
            Thread[] threads = new Thread[ThreadCount];
            Stopwatch stopwatch = Stopwatch.StartNew();

            for (int i = 0; i < ThreadCount; i++)
            {
                int threadNumber = i + 1;
                (int start, int end) = GetChunk(i, ThreadCount, StartNumber, EndNumber);

                threads[i] = new Thread(() =>
                {
                    WriteLineSafe(consoleLock, $"[{versionName}] Поток {threadNumber} обрабатывает диапазон {start}-{end}");

                    for (int number = start; number <= end; number++)
                    {
                        WriteLineSafe(consoleLock, $"[{versionName}] Поток {threadNumber}: проверяю {number}");

                        if (IsPrime(number))
                        {
                            incrementPrimeCount();
                            WriteLineSafe(consoleLock, $"[{versionName}] Поток {threadNumber}: {number} - простое");
                        }
                    }

                    WriteLineSafe(consoleLock, $"[{versionName}] Поток {threadNumber} завершил работу");
                });

                threads[i].Start();
            }

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i].Join();
            }

            stopwatch.Stop();

            Console.WriteLine();
            Console.WriteLine($"[{versionName}] Общее количество простых чисел: {getPrimeCount()}");
            Console.WriteLine($"[{versionName}] Время выполнения: {stopwatch.Elapsed}");
        }

        private static (int start, int end) GetChunk(int index, int totalThreads, int rangeStart, int rangeEnd)
        {
            int totalNumbers = rangeEnd - rangeStart + 1;
            int baseSize = totalNumbers / totalThreads;
            int remainder = totalNumbers % totalThreads;

            int start = rangeStart + index * baseSize + Math.Min(index, remainder);
            int size = baseSize + (index < remainder ? 1 : 0);
            int end = start + size - 1;

            return (start, end);
        }

        private static bool IsPrime(int number)
        {
            if (number < 2)
                return false;

            if (number == 2)
                return true;

            if (number % 2 == 0)
                return false;

            int limit = (int)Math.Sqrt(number);
            for (int divisor = 3; divisor <= limit; divisor += 2)
            {
                if (number % divisor == 0)
                    return false;
            }

            return true;
        }

        private static void WriteLineSafe(object consoleLock, string message)
        {
            lock (consoleLock)
            {
                Console.WriteLine(message);
            }
        }
    }
}