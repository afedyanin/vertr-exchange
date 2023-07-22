using Vertr.OrderMatching.ConsoleApp.AeronSample;

namespace Vertr.OrderMatching.ConsoleApp2;

internal class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Console app #2 starting...");
        Console.WriteLine(GetKey("aaa", "bbb", "ccc", 18, "ddd"));

        // SimpleSubscriber.Start();
    }

    private static string GetKey(params object[] items)
    {
        return $"Prefix_{string.Join("_", items)}";
    }
}
