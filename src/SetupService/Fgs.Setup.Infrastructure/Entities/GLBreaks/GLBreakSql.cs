using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Entities.GLBreaks;

internal static class GLBreakSql
{
    public const string Table = "setup.\"FgsSetupGLBreak\"";

    public const string LocationTable = "setup.\"FgsLocation\"";

    public const string TradeTable = "setup.\"FgsSetupGLBreakTrade\"";

    public const string SelectDetailColumns = """
        glb."Id", glb."Code", glb."Name", glb."BreakLabel",
        glb."BreakLevel", glb."LogoFileId", glb."AddressId", glb."IsActive",
        loc."Id" AS "LocationId", loc."AddressLine1", loc."AddressLine2", loc."AddressLine3",
        loc."AddressLine4", loc."City", loc."State", loc."County", loc."Country",
        loc."PostalCode", loc."FormattedAddress", loc."Latitude", loc."Longitude",
        loc."PlaceId", loc."IsActive" AS "LocationIsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Code", "Name", "BreakLabel", "BreakLevel",
        "LogoFileId", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name", "BreakLevel"
        """;

    public const string SelectTradeColumns = """
        "Id", "GLBreakId", "TradeCode"
        """;

    public const string LocationJoin = """
        LEFT JOIN setup."FgsLocation" loc
          ON loc."Id" = glb."AddressId" AND loc."IsActive" = TRUE
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "Code", "Name", "BreakLevel", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}