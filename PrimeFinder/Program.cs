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
            Timer timer = new Timer(1000);
            timer.Elapsed += async (sender, e) => await PrintPrimes();
            timer.Start();

            var stopWatch = new System.Diagnostics.Stopwatch();
            stopWatch.Start();
            PrimeFinder.Find();
            stopWatch.Stop();

            timer.Stop();

            var t = PrintPrimes(true);
            while (!t.IsCompleted)
            {
                System.Threading.Thread.Sleep((500));
            }

            Console.WriteLine();
            Console.WriteLine($"Time: {stopWatch.Elapsed}");

            var pt = new PrimeTest().Primes();
            var primes = PrimeFinder.Primes(1, 500);
            for (int i = 0; i < Math.Max(primes.Count(),pt.Count()) ; i++)
            {

                if (pt[i] != primes[i])
                {
                    Console.WriteLine("** error **");
                    Console.WriteLine($"{pt[i]} <> {primes[i]}");
                }

            }

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
                   foreach (var p in batch)
                   {
                       Console.WriteLine(p);
                   }
               }
               while (all && (PrimeFinder.MaxFound() >= batch.Last()));
           });
            t.Start();
            return t;
        }
    }
}
