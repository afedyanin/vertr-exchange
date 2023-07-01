using Disruptor;
using Disruptor.Dsl;
using Vertr.OrderMatching.ConsoleApp.AeronSample;

namespace Vertr.OrderMatching.ConsoleApp;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Console app starting...");
        SimplePublisher.Start();
    }
}


