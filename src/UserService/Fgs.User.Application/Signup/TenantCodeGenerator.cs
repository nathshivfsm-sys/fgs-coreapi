using System.Text.RegularExpressions;

namespace Fgs.User.Application.Signup;

internal static partial class TenantCodeGenerator
{
    public static string FromCompanyName(string companyName)
    {
        var slug = SlugRegex().Replace(companyName.Trim().ToLowerInvariant(), "-").Trim('-');
        if (string.IsNullOrEmpty(slug))
        {
            slug = "company";
        }

        return slug.Length <= 40 ? slug : slug[..40];
    }

    public static string WithSuffix(string baseCode, string suffix)
    {
        var maxBase = Math.Max(1, 50 - suffix.Length - 1);
        var trimmed = baseCode.Length <= maxBase ? baseCode : baseCode[..maxBase].TrimEnd('-');
        return $"{trimmed}-{suffix}";
    }

    [GeneratedRegex(@"[^a-z0-9]+", RegexOptions.CultureInvariant)]
    private static partial Regex SlugRegex();
}
