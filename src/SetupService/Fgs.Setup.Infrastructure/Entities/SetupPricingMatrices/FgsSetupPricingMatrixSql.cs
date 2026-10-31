using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;
using Fgs.Setup.Infrastructure.Entities.SetupPricingMatrixLabors;
using Fgs.Setup.Infrastructure.Entities.SetupPricingMatrixOthers;

namespace Fgs.Setup.Infrastructure.Entities.SetupPricingMatrices;

internal static class FgsSetupPricingMatrixSql
{
    public const string Table = "setup.\"FgsSetupPricingMatrix\"";

    /// <summary>
    /// Derived category: Labor (2) → Other (3) → Material (1). Same expression used for SELECT and filter.
    /// </summary>
    public static readonly string CategoryExpression = $"""
        CASE
          WHEN pm."IsLaborTierStructure" = TRUE
            OR pm."IsLaborRateBySkillLevel" = TRUE
            OR EXISTS (
              SELECT 1
              FROM {FgsSetupPricingMatrixLaborSql.Table} l
              WHERE l."PricingMatrixId" = pm."Id"
                AND l."IsActive" = TRUE
            )
          THEN 2
          WHEN EXISTS (
              SELECT 1
              FROM {FgsSetupPricingMatrixOtherSql.Table} o
              WHERE o."PricingMatrixId" = pm."Id"
                AND o."IsActive" = TRUE
            )
          THEN 3
          ELSE 1
        END
        """;

    public static readonly string SelectHeaderColumns = $"""
        pm."Id", pm."Code", pm."Name", pm."IsDefault", pm."IsLaborTierStructure", pm."IsLaborRateBySkillLevel",
        pm."PriceAdjustmentTypeId", ({CategoryExpression}) AS "Category",
        pm."EffectiveFrom", pm."EffectiveTo", pm."IsMobileVisible", pm."IsActive"
        """;

    public static readonly string SelectSummaryColumns = SelectHeaderColumns;

    public static readonly string SelectLookupColumns = $"""
        pm."Id", pm."Code", pm."Name", pm."IsDefault", ({CategoryExpression}) AS "Category"
        """;

    public const string SelectFlagsColumns = """
        pm."Id", pm."IsLaborTierStructure", pm."IsLaborRateBySkillLevel", pm."PriceAdjustmentTypeId", pm."IsActive"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "Code", "Name", "IsDefault", "IsLaborTierStructure", "IsLaborRateBySkillLevel",
        "PriceAdjustmentTypeId", "EffectiveFrom", "EffectiveTo", "IsMobileVisible", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            defaultColumn: "Code", tableAlias: "pm");

}
