using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace PrimeFinder
{
    public class PrimeTest : IDisposable
    {
        private readonly string url = "http://www.primos.mat.br/primeiros_10000_primos.txt"; 
        private readonly string fileName = "primes.txt";

        private StreamReader handle;

        public PrimeTest()
        {
            DirectoryInfo appDir = new DirectoryInfo(".");
            FileInfo primeFile = appDir.EnumerateFiles(fileName).FirstOrDefault();

            if (primeFile == null) {
                using (var client = new WebClient()) {
                    
                    client.DownloadFile(url, fileName);

                }
                primeFile = appDir.EnumerateFiles(fileName).FirstOrDefault();
            }

            handle = primeFile.OpenText();
        }

        public void Dispose()
        {
            ((IDisposable)handle).Dispose();
        }

        public IReadOnlyList<int> Primes() {
            List<int> list = new List<int>();
            while (!handle.EndOfStream) {
                string line = handle.ReadLine();
                string[] numbers = line.Split("\t");
                foreach(var num in numbers) {
                    int i = 0;
                    int.TryParse(num, out i);
                    if (i > 1) list.Add(i);
                }
            }
            return list;
        }
    }
}
