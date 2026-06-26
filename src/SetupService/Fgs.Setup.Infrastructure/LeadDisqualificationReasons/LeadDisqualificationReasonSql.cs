using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.LeadDisqualificationReasons;

internal static class LeadDisqualificationReasonSql
{
    public const string Table = "setup.\"FgsLeadDisqualificationReason\"";

    public const string SelectDetailColumns = """
        "Id", "ReasonCode", "ReasonName", "Description", "DisplayOrder", "IsSystem", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "ReasonCode", "ReasonName", "Description", "DisplayOrder", "IsSystem", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "ReasonCode", "ReasonName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "ReasonCode", "ReasonName", "Description", "IsSystem"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"ReasonName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"ReasonName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
