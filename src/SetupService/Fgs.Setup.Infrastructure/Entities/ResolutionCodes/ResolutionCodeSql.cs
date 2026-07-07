using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Entities.ResolutionCodes;

internal static class ResolutionCodeSql
{
    public const string Table = "setup.\"FgsResolutionCode\"";

    public const string SelectDetailColumns = """
        "Id", "GloResolutionTypeId", "ResolutionCode", "ResolutionName", "IsMobileVisible", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "GloResolutionTypeId", "ResolutionCode", "ResolutionName", "IsMobileVisible", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "ResolutionCode", "ResolutionName"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "GloResolutionTypeId", "ResolutionCode", "ResolutionName", "IsMobileVisible"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"ResolutionName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}