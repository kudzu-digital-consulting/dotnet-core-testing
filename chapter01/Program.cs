using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class MyApp
{
    private static void Main(string[] args)
    {
        List<string> names = new List<string>(args);
        if (!names.Any()) {
            names.Add(string.Empty);
        }

        
        Hello(names);

    }

    private static void Hello(List<string> Names)
    {
        ParallelOptions opts = new ParallelOptions();
        opts.MaxDegreeOfParallelism = 2;
        Parallel.ForEach(Names, opts, name =>
        {
            string message = string.Empty;
            if (name is string && !string.IsNullOrEmpty(name))
            {
                message = string.Format("Hello, {0}!", name as string);
            }
            else
            {
                message = "Hello!";
            }
            Console.WriteLine(message);

        });

    }
}
