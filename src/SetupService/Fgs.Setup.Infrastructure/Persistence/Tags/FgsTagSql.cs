using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.Tags;

internal static class FgsTagSql
{
    public const string Table = "setup.\"FgsTag\"";

    public const string SelectDetailColumns = """
        "Id", "TagCode", "Name", "Description", "BackgroundColor", "TextColor", "IconFileId", "UsageCount", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TagCode", "Name", "Description", "BackgroundColor", "TextColor", "IconFileId", "UsageCount", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "TagCode", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "TagCode", "Name", "Description", "BackgroundColor", "TextColor", "IconFileId", "UsageCount"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}