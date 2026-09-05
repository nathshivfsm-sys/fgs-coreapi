using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixLabors;

internal static class FgsSetupPricingMatrixLaborSql
{
    public const string Table = "setup.\"FgsSetupPricingMatrixLabor\"";
    public const string DetailColumns = """"Id", "PricingMatrixId", "LaborRateTypeId", "TechSkillLevelId", "BaseRate", "OvertimeMultiplier", "DoubleTimeMultiplier", "DiscountPercent", "IsActive"""";
    public const string LookupColumns = """"Id", "PricingMatrixId", "LaborRateTypeId"""";
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "Id", "PricingMatrixId", "LaborRateTypeId", "TechSkillLevelId", "BaseRate", "OvertimeMultiplier", "DoubleTimeMultiplier", "DiscountPercent", "IsActive" };
    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, Allowed);

}
