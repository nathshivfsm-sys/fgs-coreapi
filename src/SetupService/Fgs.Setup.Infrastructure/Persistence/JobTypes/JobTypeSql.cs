using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.JobTypes;

internal static class JobTypeSql
{
    public const string Table = "setup.\"FgsJobType\"";

    public const string SelectDetailColumns = """
        "Id", "JobTypeCode", "Name", "UsedFor", "BusinessUnit", "BackgroundColor", "TextColor", "ShowToFieldTech", "ShowOnCustomerPortal", "DisplayOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "JobTypeCode", "Name", "UsedFor", "BusinessUnit", "BackgroundColor", "TextColor", "ShowToFieldTech", "ShowOnCustomerPortal", "DisplayOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "JobTypeCode", "Name", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "JobTypeCode", "Name", "UsedFor", "BusinessUnit", "BackgroundColor", "TextColor", "ShowToFieldTech", "ShowOnCustomerPortal"
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
            ? $"ORDER BY \"Id\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
