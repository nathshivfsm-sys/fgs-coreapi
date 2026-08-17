using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrices;

internal static class FgsSetupPricingMatrixSql
{
    public const string Table = "setup.\"FgsSetupPricingMatrix\"";

    public const string SelectHeaderColumns = """
        pm."Id", pm."Code", pm."Name", pm."IsDefault", pm."IsLaborTierStructure", pm."IsLaborRateBySkillLevel",
        pm."PriceAdjustmentTypeId", pm."EffectiveFrom", pm."EffectiveTo", pm."IsMobileVisible", pm."IsActive"
        """;

    public const string SelectSummaryColumns = SelectHeaderColumns;

    public const string SelectLookupColumns = """
        pm."Id", pm."Code", pm."Name", pm."IsDefault"
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
