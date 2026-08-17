using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupTechSkillLevels;

internal static class FgsSetupTechSkillLevelSql
{
    public const string Table = "setup.\"FgsSetupTechSkillLevel\"";

    public const string SelectDetailColumns = """
        "Id", "Code", "Name", "Description", "SortOrder", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Code", "Name", "Description", "SortOrder", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name", "SortOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "SortOrder", "Code", "Name", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            nullsLastTiebreakerColumn: "Name");

}