namespace Fgs.File.Application.Common;

public static class AttachmentDeletionTags
{
    public const string DeletedTag = "deleted";

    public static bool IsDeleted(string[]? tags) =>
        tags is not null && tags.Contains(DeletedTag, StringComparer.OrdinalIgnoreCase);

    public static bool IsActive(string[]? tags) => !IsDeleted(tags);

    public static string[] MarkDeleted(string[]? tags)
    {
        var set = new HashSet<string>(tags ?? [], StringComparer.OrdinalIgnoreCase) { DeletedTag };
        return set.ToArray();
    }
}
