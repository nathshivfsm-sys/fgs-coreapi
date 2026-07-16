using Fgs.User.Domain.Entities;

namespace Fgs.User.Application.Abstractions.Identity;

public interface ILoginAuthorizationProfileBuilder
{
    Task<FgsUserProfile> BuildAsync(FgsUser user, CancellationToken cancellationToken = default);
}

public static class LoginDisplayNameParser
{
    public static (string FirstName, string LastName) Split(string? displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
        {
            return (string.Empty, string.Empty);
        }

        var parts = displayName.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length switch
        {
            0 => (string.Empty, string.Empty),
            1 => (parts[0], string.Empty),
            _ => (parts[0], parts[1])
        };
    }
}
