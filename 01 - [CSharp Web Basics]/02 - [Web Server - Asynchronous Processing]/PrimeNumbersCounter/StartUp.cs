using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PrimeNumbersCounter
{
    public class StartUp
    {
        static int Count = 0;

        private static object lockObj = new object();

        public static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();

            var tasks = new List<Task>();

            for (int i = 1; i <= 100; i++)
            {
                var task = Task.Run(async () =>
                {
                    var client = new HttpClient();

                    var url = $"https://vicove.com/vic-{i}";

                    var response = await client.GetAsync(url);

                    var vic = await response.Content.ReadAsStringAsync();

                    Console.WriteLine(vic.Length);
                });

                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine(sw.Elapsed);

            Console.ReadLine();

            // TaskTest();

            // ThreadsWorkTest();

            // PrintPrimeCount(1,10_000_000);

            // ThreadException();

            // FourThreadsWork();
        }

        private static void TaskTest()
        {
            var sw = Stopwatch.StartNew();

            for (int i = 0; i < 1000000; i++)
            {
                Task.Run(() =>
                {
                    while (true)
                    {
                    }
                });
            }

            Console.WriteLine(sw.Elapsed);
            Console.ReadLine();
        }

        private static void ThreadsWorkTest()
        {
            var sw = Stopwatch.StartNew();

            var threads = new List<Thread>();

            for (int i = 0; i < 10000; i++)
            {
                var thread = new Thread(() =>
                {
                    while (true)
                    {
                    }
                });

                thread.Start();

                threads.Add(thread);
            }

            Console.WriteLine(sw.Elapsed);
        }

        private static void ThreadException()
        {
            new Thread(() =>
            {
                try
                {
                    throw new Exception();
                }
                catch (Exception)
                {
                }
            }).Start();

            while (true)
            {
                var input = Console.ReadLine();

                Console.WriteLine(input.ToUpper());
            }
        }

        private static void FourThreadsWork()
        {
            var sw = Stopwatch.StartNew();

            var thread = new Thread(() => PrintPrimeCount(1, 2_500_000));
            thread.Start();

            var thread2 = new Thread(() => PrintPrimeCount(2_500_001, 5_000_000));
            thread2.Start();

            var thread3 = new Thread(() => PrintPrimeCount(5_000_001, 7_500_000));
            thread3.Start();

            var thread4 = new Thread(() => PrintPrimeCount(7_500_001, 10_000_000));
            thread4.Start();

            thread.Join();
            thread2.Join();
            thread3.Join();
            thread4.Join();

            Console.WriteLine(Count);

            Console.WriteLine(sw.Elapsed);

            while (true)
            {
                var input = Console.ReadLine();

                Console.WriteLine(input.ToUpper());
            }
        }

        private static void PrintPrimeCount(int min, int max)
        {
            for (int i = min; i <= max; i++)
            {
                var isPrime = true;

                for (int j = 2; j < Math.Sqrt(i); j++)
                {
                    if (i % j == 0)
                    {
                        isPrime = false;

                        break;
                    }
                }

                if (isPrime)
                {
                    lock (lockObj)
                    {
                        Count++;
                    }
                }
            }
        }
    }
}
