using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixLabors;

internal static class FgsSetupPricingMatrixLaborSql
{
    public const string Table = "setup.\"FgsSetupPricingMatrixLabor\"";
    public const string DetailColumns = """"Id", "PricingMatrixId", "LaborRateTypeId", "TechSkillLevelId", "BaseRate", "OvertimeMultiplier", "DoubleTimeMultiplier", "DiscountPercent", "IsActive"""";
    public const string LookupColumns = """"Id", "PricingMatrixId", "LaborRateTypeId"""";
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "Id", "PricingMatrixId", "LaborRateTypeId", "TechSkillLevelId", "BaseRate", "OvertimeMultiplier", "DoubleTimeMultiplier", "DiscountPercent", "IsActive" };
    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        var column = !string.IsNullOrWhiteSpace(sortBy) && Allowed.Contains(sortBy) ? Allowed.First(x => x.Equals(sortBy, StringComparison.OrdinalIgnoreCase)) : "Id";
        return $"ORDER BY \"{column}\" {dir}";
    }
}
