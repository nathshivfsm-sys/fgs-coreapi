using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.ResolutionCodes;

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
        => SetupSqlOrderBy.Resolve(sortBy, direction, AllowedSortColumns);

}