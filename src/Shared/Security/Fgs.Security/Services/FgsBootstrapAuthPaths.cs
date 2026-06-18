namespace Fgs.Security.Services;

public static class FgsBootstrapAuthPaths
{
    public static bool IsBootstrapPath(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        return path.EndsWith("/auth/me", StringComparison.OrdinalIgnoreCase)
               || path.EndsWith("/auth/validate", StringComparison.OrdinalIgnoreCase);
    }
}
