using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.SetupPaymentTerms;

internal static class FgsSetupPaymentTermSql
{
    public const string Table = "setup.\"FgsSetupPaymentTerm\"";

    public const string SelectDetailColumns = """
        "Id", "Name", "DueDateMethod", "NumberOfDays", "IsAccountsReceivable", "IsAccountsPayable", "IsMobileVisible", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Name", "DueDateMethod", "NumberOfDays", "IsAccountsReceivable", "IsAccountsPayable", "IsMobileVisible", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Name"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "Name", "DueDateMethod", "NumberOfDays", "IsAccountsReceivable", "IsAccountsPayable", "IsMobileVisible"
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
            ? $"ORDER BY \"Name\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}