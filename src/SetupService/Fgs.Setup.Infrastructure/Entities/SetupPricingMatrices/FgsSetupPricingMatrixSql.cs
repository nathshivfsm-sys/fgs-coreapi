using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Entities.SetupPricingMatrices;

internal static class FgsSetupPricingMatrixSql
{
    public const string Table = "setup.\"FgsSetupPricingMatrix\"";

    public const string LaborTable = "setup.\"FgsSetupPricingMatrixLabor\"";

    public const string LaborTierTable = "setup.\"FgsSetupPricingMatrixLaborTier\"";

    public const string MaterialTierTable = "setup.\"FgsSetupPricingMatrixMaterialTier\"";

    public const string OtherTable = "setup.\"FgsSetupPricingMatrixOther\"";

    public const string SelectHeaderColumns = """
        pm."Id", pm."Code", pm."Name", pm."IsDefault", pm."IsLaborTierStructure", pm."IsLaborRateBySkillLevel",
        pm."PriceAdjustmentTypeId", pm."EffectiveFrom", pm."EffectiveTo", pm."IsMobileVisible", pm."IsActive"
        """;

    public const string SelectSummaryColumns = SelectHeaderColumns;

    public const string SelectLookupColumns = """
        pm."Id", pm."Code", pm."Name", pm."IsDefault"
        """;

    public const string SelectLaborColumns = """
        l."Id", l."PricingMatrixId", l."LaborRateTypeId", l."TechSkillLevelId", l."BaseRate",
        l."OvertimeMultiplier", l."DoubleTimeMultiplier", l."DiscountPercent", l."IsActive"
        """;

    public const string SelectLaborTierColumns = """
        lt."Id", lt."PricingMatrixLaborId", lt."SequenceOrder", lt."DurationMinutes", lt."Rate",
        lt."TechSkillLevelId", lt."IsActive"
        """;

    public const string SelectMaterialTierColumns = """
        mt."Id", mt."PricingMatrixId", mt."FromCost", mt."ToCost", mt."AdjustmentValue", mt."IsActive"
        """;

    public const string SelectOtherColumns = """
        o."Id", o."PricingMatrixId", o."CategoryCode", o."Name", o."AdjustmentValue", o."DiscountPercent", o."IsActive"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "Code", "Name", "IsDefault", "IsLaborTierStructure", "IsLaborRateBySkillLevel",
        "PriceAdjustmentTypeId", "EffectiveFrom", "EffectiveTo", "IsMobileVisible", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY pm.\"Code\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY pm.\"{column}\" {dir}";
    }
}
