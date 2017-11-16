using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace PrimeFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            int findCount = 500;
            if (args.Length > 0) {
                if (args[0].ToLower() == "all")
                {
                    findCount = int.MaxValue;
                }
                else
                {
                    int.TryParse(args[0], out findCount);
                }
            }

            Console.CancelKeyPress += delegate
            {
                Console.WriteLine("## stopped ##");
                Console.WriteLine($"Found: {PrimeFinder.Count()}");
            };

            Timer timer = new Timer(500);
            timer.Elapsed += async (sender, e) => await PrintPrimes();
            timer.Start();

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            PrimeFinder.Find(findCount);
            stopWatch.Stop();

            timer.Stop();

            var t = PrintPrimes(true);
            while (!t.IsCompleted)
            {
                System.Threading.Thread.Sleep((500));
            }

            Console.WriteLine();
            Console.WriteLine($"Time: {stopWatch.Elapsed}");
            Console.WriteLine($"Found: {PrimeFinder.Count()}");

            Console.WriteLine();
            Console.WriteLine("checking ");

            var pt = new PrimeTest().Primes();
            var primes = PrimeFinder.Primes(1, pt.Count());
            for (int i = 0; i < Math.Min(primes.Count(), pt.Count()); i++)
            {
                if (pt[i] != primes[i])
                {
                    Console.WriteLine($"{pt[i]} <> {primes[i]} ** ");
                }


            }

            Console.WriteLine();
            Console.WriteLine();

        }

        private static object printLock = new object();
        private static int lastPrinted = 0;
        private static Task PrintPrimes(bool all = false)
        {
            var t = new Task(() =>
           {
               List<int> batch;
               do
               {
                   lock (printLock)
                   {
                       batch = PrimeFinder.Primes(lastPrinted, 100);
                       lastPrinted = lastPrinted + batch.Count;


                   }
                   int lineCounter = lastPrinted - batch.Count();
                   foreach (var p in batch)
                   {
                       lineCounter++;
                       Console.Write(p);
                       if (lineCounter % 10 == 0)
                       {
                           Console.WriteLine();
                       }
                       else
                       {
                           Console.Write("\t");
                       }
                   }
               }
               while (all && (PrimeFinder.MaxFound() > batch.Last()));
           });
            t.Start();
            return t;
        }
    }
}
