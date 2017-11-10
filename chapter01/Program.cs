using System;
using System.Threading.Tasks;

class MyApp
{
    private static void Main(string[] args)
    {
        string name = string.Empty;
        if (args.Length > 0) name = args[0];
        var task = HelloAsync(name);
        task.Wait(2000);
    }

    private static async Task HelloAsync(string Name)
    {

        await Task.Run(() =>
        {
            string message = string.Empty;
            if (Name is string && !string.IsNullOrEmpty(Name))
            {
                message = string.Format("Hello, {0}!", Name as string);
            }
            else
            {
                message = "Hello!";
            }
            Console.WriteLine(message);

        });

    }
}
