using Vertr.Terminal.ApiClient.Contracts;

namespace Vertr.Terminal.ApiClient.Extensions;

public static class UserExtensions
{
    public static User? GetById(this IEnumerable<User> users, long id) => users.FirstOrDefault(x => x.Id == id);

    public static User? GetByName(this IEnumerable<User> users, string name)
    {
        return users.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
