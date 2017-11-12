using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace PrimeFinder
{
    public class PrimeTest 
    {
        private readonly string url = "http://www.primos.mat.br/primeiros_10000_primos.txt"; 
        private readonly string fileName = "primes.txt";

        FileInfo primeFile;

        public PrimeTest()
        {
            DirectoryInfo appDir = new DirectoryInfo(".");
            primeFile = appDir.EnumerateFiles(fileName).FirstOrDefault();

            if (primeFile == null) {
                using (var client = new WebClient()) {
                    
                    client.DownloadFile(url, fileName);

                }
                primeFile = appDir.EnumerateFiles(fileName).FirstOrDefault();
            }


        }

       

        public IReadOnlyList<int> Primes() {
            List<int> list = new List<int>();
            using (var handle = primeFile.OpenText())
            {
                while (!handle.EndOfStream)
                {
                    string line = handle.ReadLine();
                    string[] numbers = line.Split("\t");
                    foreach (var num in numbers)
                    {
                        int i = 0;
                        int.TryParse(num, out i);
                        if (i > 1) list.Add(i);
                    }
                }
            }
            return list;
        }
    }
}
