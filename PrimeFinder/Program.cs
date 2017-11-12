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

            PrimeFinder.Find();

            timer.Stop();

            var t = PrintPrimes(true);
            while (!t.IsCompleted)
            {
                System.Threading.Thread.Sleep((500));
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
                       batch = PrimeFinder.Primes(lastPrinted, 15);
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
