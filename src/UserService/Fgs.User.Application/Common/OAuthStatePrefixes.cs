namespace Fgs.User.Application.Common;

public static class OAuthStatePrefixes
{
    public const string UserLogin = "user:";

    /// <summary>
    /// Unique per login attempt so Redis PKCE is not overwritten when start-login
    /// is called again before token exchange (same user id).
    /// Format: <c>user:{userId:D}:{nonce:N}</c>. Legacy <c>user:{userId:D}</c> still parses.
    /// </summary>
    public static string CreateUserLoginState(Guid userId) =>
        $"{UserLogin}{userId:D}:{Guid.NewGuid():N}";

    public static bool TryParseUserLoginState(string state, out Guid userId)
    {
        userId = default;
        if (string.IsNullOrWhiteSpace(state)
            || !state.StartsWith(UserLogin, StringComparison.Ordinal))
        {
            return false;
        }

        var rest = state[UserLogin.Length..];
        var separator = rest.IndexOf(':');
        if (separator < 0)
        {
            return Guid.TryParse(rest, out userId);
        }

        return Guid.TryParse(rest.AsSpan(0, separator), out userId);
    }
}
