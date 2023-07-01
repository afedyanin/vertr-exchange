using Vertr.OrderMatching.ConsoleApp.AeronSample;

namespace Vertr.OrderMatching.ConsoleApp2;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Console app #2 starting...");

        SimpleSubscriber.Start();
    }
}
