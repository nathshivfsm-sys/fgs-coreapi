using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPaymentTerms;

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
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "Name");

}