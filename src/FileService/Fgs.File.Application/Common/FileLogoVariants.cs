namespace Fgs.File.Application.Common;

public static class FileLogoVariants
{
    public const string LogoTag = "logo";

    public static readonly IReadOnlySet<string> SupportedVariants = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "full",
        "compact",
        "icon",
        "favicon"
    };

    public static readonly IReadOnlyDictionary<string, int> MaxDimensions = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
    {
        ["full"] = 800,
        ["compact"] = 200,
        ["icon"] = 64,
        ["favicon"] = 32
    };

    public static bool IsSupported(string variant) => SupportedVariants.Contains(variant);

    public static string[] BuildVariantTags(string variant) => [LogoTag, variant.ToLowerInvariant()];

    public static string BuildVariantFileName(string storedFileName, string variant, string extension)
    {
        var nameWithoutExtension = Path.GetFileNameWithoutExtension(storedFileName);
        return $"{nameWithoutExtension}-{variant}{extension}";
    }

    public static bool TryGetVariantTag(string[]? tags, out string variant)
    {
        variant = string.Empty;
        if (tags is null)
        {
            return false;
        }

        foreach (var tag in tags)
        {
            if (IsSupported(tag))
            {
                variant = tag.ToLowerInvariant();
                return true;
            }
        }

        return false;
    }
}
