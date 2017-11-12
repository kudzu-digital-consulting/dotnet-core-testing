using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PrimeFinder
{
    public class PrimeFinder
    {
        private static List<int> _primes = new List<int>() { 1, 2, 3, 5 };
        private static ParallelOptions opts = new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount };
        private static List<int> _buffer = new List<int>();

        public static List<int> Primes(int Skip, int Take = 100)
        {
            lock (_primes)
            {
                return _primes.Skip(Skip).Take(Take).ToList();
            }
        }

        public static int MaxFound()
        {
            lock (_primes)
            {
                // assume last is the max
                return _primes.Last();
            }
        }

        public static int Count()
        {
            lock (_primes)
            {
                return _primes.Count();
            }
        }

        private static void AddPrime(int Prime)
        {
            lock (_primes)
            {
                _primes.Add(Prime);

            }
        }

        private static void factorNumber(int Number)
        {
            try
            {
                // even number done
                if (Number % 2 == 0) return;

                int next = 2;
                int test;
                do
                {
                    while (PrimeFinder.MaxFound() * 2 < Number)
                    {
                        Thread.Sleep(1000);
                    }
                    var tests = PrimeFinder.Primes(next);
                    while (!tests.Any())
                    {
                        Thread.Sleep(500);
                        tests = PrimeFinder.Primes(next);
                    }

                    test = tests.First();
                    next = next + tests.Count();
                    for (int i = 0; i < tests.Count(); i++)
                    {
                        test = tests[i];
                        // a divisor was found
                        if (Number % test == 0) return;
                    }

                }
                // only need to test the first half of the primes
                while (test * 2 <= Number);

                // no divisor found for num
                // add it to the list
                PrimeFinder.AddPrime(Number);

            }
            catch (Exception e)
            {
                Console.WriteLine((e.Message));
            }
        }

        public static void Find(int Max = int.MaxValue)
        {
            int start = PrimeFinder.MaxFound() + 1;
            int count = Max - start;
            var range = Enumerable.Range(start, count);
            // single thread the first 100 to prevent errors
            foreach (var num in range)
            {
                if (num % 2 != 0 && !_primes.AsReadOnly().Skip(2).Any(p => (num % p) == 0))
                {
                    PrimeFinder.AddPrime(num);
                }
            }


        }

    }
}
