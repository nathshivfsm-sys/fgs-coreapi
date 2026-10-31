using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Entities.PriceBooks;

internal static class FgsPriceBookSql
{
    public const string Table = "setup.\"FgsPriceBook\"";
    public const string Alias = "pb";
    public const string JobTypeTable = "setup.\"FgsJobType\"";
    public const string VersionTable = "setup.\"FgsSetupPriceBookVersion\"";

    public const string SelectDetailColumns = """
        pb."Id", pb."PriceBookCode", pb."PriceBookName", pb."PriceBookVersionId",
        v."Version", v."EffectiveFrom", v."EffectiveTo",
        pb."Description", pb."JobTypeId", pb."PricingModel",
        pb."EstimatedDurationMinutes", pb."BasePrice", pb."IsTaxable", pb."IsActive"
        """;

    public const string SelectSummaryColumns = """
        pb."Id", pb."PriceBookCode", pb."PriceBookName", pb."PriceBookVersionId",
        v."Version", v."EffectiveFrom", v."EffectiveTo",
        pb."JobTypeId", pb."PricingModel",
        pb."EstimatedDurationMinutes", pb."BasePrice", pb."IsTaxable", pb."IsActive"
        """;

    public const string SelectLookupColumns = """
        pb."Id", pb."PriceBookCode", pb."PriceBookName", pb."PricingModel", pb."PriceBookVersionId"
        """;

    public const string FromWithVersionJoin = $"""
        {Table} {Alias}
        LEFT JOIN {VersionTable} v ON v."Id" = {Alias}."PriceBookVersionId"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "PriceBookCode", "PriceBookName", "PriceBookVersionId", "JobTypeId", "PricingModel",
        "EstimatedDurationMinutes", "BasePrice", "IsTaxable"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "PriceBookName",
            tableAlias: Alias);
}
