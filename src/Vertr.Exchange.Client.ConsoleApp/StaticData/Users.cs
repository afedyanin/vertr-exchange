namespace Vertr.Exchange.Client.ConsoleApp.StaticData;

public record User(int Id, string Name);

public static class Users
{
    public static readonly User Alice = new User(10, "Alice");
    public static readonly User Bob = new User(20, "Bob");

    public static readonly User[] All = [Alice, Bob];

    public static User? GetByName(string name)
    {
        return All.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
