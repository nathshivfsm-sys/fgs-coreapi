using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.LeadSources;

internal static class LeadSourceSql
{
    public const string Table = "setup.\"FgsLeadSource\"";

    public const string SelectDetailColumns = """
        "Id", "SourceCode", "SourceName", "Description", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "SourceCode", "SourceName", "Description", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "SourceCode", "SourceName"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "SourceCode", "SourceName", "Description"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}