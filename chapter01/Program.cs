using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class MyApp
{
    private static ParallelOptions opts = new ParallelOptions() {MaxDegreeOfParallelism = 4};

    private static void Main(string[] args)
    {
        List<string> names = new List<string>(args);
        if (!names.Any()) {
            names.Add(string.Empty);
        }

        var stopWatch = new Stopwatch();

        stopWatch.Start();
        Hello(Reverse(names));

        stopWatch.Stop();

        Console.WriteLine(string.Format("Elapsed: {0}", stopWatch.ElapsedMilliseconds));

    }

    private static List<string> Reverse(List<string> Names) {
        var sync = new object();
        var list = new List<string>();

        Parallel.ForEach(Names, opts, name =>
        {
            if (name is string)
            {
                string temp = name as string;
                string reversed = new string(temp.ToLower().Reverse().ToArray());
                lock(sync) {
                    list.Add(reversed);
                }
            }
        });


        return list;
    }

    private static void Hello(List<string> Names)
    {
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
