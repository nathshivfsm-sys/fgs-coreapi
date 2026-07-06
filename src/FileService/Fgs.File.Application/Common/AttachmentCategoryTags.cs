namespace Fgs.File.Application.Common;

public static class AttachmentCategoryTags
{
    public const string Prefix = "category:";

    public static string ToTag(string category) =>
        $"{Prefix}{category.Trim().ToLowerInvariant()}";

    public static bool TryGetCategory(string[]? tags, out string category)
    {
        category = string.Empty;
        if (tags is null)
        {
            return false;
        }

        foreach (var tag in tags)
        {
            if (tag.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase)
                && tag.Length > Prefix.Length)
            {
                category = tag[Prefix.Length..];
                return true;
            }
        }

        return false;
    }

    public static string[] MergeTags(string category, IEnumerable<string>? userTags)
    {
        var set = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ToTag(category) };
        if (userTags is not null)
        {
            foreach (var tag in userTags.Where(t => !string.IsNullOrWhiteSpace(t)))
            {
                var trimmed = tag.Trim();
                if (!trimmed.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
                {
                    set.Add(trimmed);
                }
            }
        }

        return set.ToArray();
    }
}
