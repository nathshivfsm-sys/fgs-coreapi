using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupDescriptions;

internal static class FgsSetupDescriptionSql
{
    public const string Table = "setup.\"FgsSetupDescription\"";

    public const string SelectDetailColumns = """
        "Id", "DescriptionTypeCode", "ShortNote", "Body", "FgsSetupTechTradeId", "SortOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "DescriptionTypeCode", "ShortNote", "Body", "FgsSetupTechTradeId", "SortOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "DescriptionTypeCode", "Body", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "SortOrder", "DescriptionTypeCode", "ShortNote", "Body", "FgsSetupTechTradeId"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            nullsLastTiebreakerColumn: "Body");

}