using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.GLBreaks;

internal static class GLBreakSql
{
    public const string Table = "setup.\"FgsSetupGLBreak\"";

    public const string LocationTable = "setup.\"FgsLocation\"";

    public const string TradeTable = "setup.\"FgsSetupGLBreakTrade\"";

    public const string SelectDetailColumns = """
        glb."Id", glb."TenantId", glb."CompanyId", glb."Code", glb."Name", glb."BreakLabel",
        glb."BreakLevel", glb."LogoFileId", glb."AddressId", glb."IsActive",
        glb."CreatedOn", glb."CreatedBy", glb."UpdatedOn", glb."UpdatedBy",
        loc."Id" AS "LocationId", loc."AddressLine1", loc."AddressLine2", loc."AddressLine3",
        loc."AddressLine4", loc."City", loc."State", loc."County", loc."Country",
        loc."PostalCode", loc."FormattedAddress", loc."Latitude", loc."Longitude",
        loc."PlaceId", loc."IsActive" AS "LocationIsActive",
        loc."CreatedOn" AS "LocationCreatedOn", loc."CreatedBy" AS "LocationCreatedBy",
        loc."UpdatedOn" AS "LocationUpdatedOn", loc."UpdatedBy" AS "LocationUpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "Code", "Name", "BreakLabel", "BreakLevel",
        "LogoFileId", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "Code", "Name", "BreakLevel"
        """;

    public const string SelectTradeColumns = """
        "Id", "GLBreakId", "TradeCode", "CreatedOn", "CreatedBy"
        """;

  public const string LocationJoin = """
        LEFT JOIN setup."FgsLocation" loc
          ON loc."Id" = glb."AddressId" AND loc."IsActive" = TRUE
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "Code", "Name", "BreakLevel", "CreatedOn", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"BreakLevel\" {dir}, \"Name\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
