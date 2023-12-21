namespace Vertr.Terminal.ConsoleApp.StaticData;

public record User(long Id, string Name);

public static class Users
{
    public static readonly User Alice = new User(10, "Alice");
    public static readonly User Bob = new User(20, "Bob");

    public static readonly User[] All = [Alice, Bob];

    public static User? GetById(long id) => All.FirstOrDefault(x => x.Id == id);

    public static User? GetByName(string name)
    {
        return All.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
