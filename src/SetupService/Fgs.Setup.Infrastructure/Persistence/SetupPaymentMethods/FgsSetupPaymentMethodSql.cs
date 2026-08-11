using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPaymentMethods;

internal static class FgsSetupPaymentMethodSql
{
    public const string Table = "setup.\"FgsSetupPaymentMethod\"";

    public const string SelectDetailColumns = """
        "Id", "DisplayName", "SortOrder", "IsMobileVisible", "IsCustomerPortalVisible", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "DisplayName", "SortOrder", "IsMobileVisible", "IsCustomerPortalVisible", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "DisplayName", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "SortOrder", "DisplayName", "IsMobileVisible", "IsCustomerPortalVisible"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            nullsLastTiebreakerColumn: "DisplayName");

}