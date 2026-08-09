using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixMaterialTiers;

internal static class FgsSetupPricingMatrixMaterialTierSql
{
    public const string Table = "setup.\"FgsSetupPricingMatrixMaterialTier\"";
    public const string DetailColumns = """"Id", "PricingMatrixId", "FromCost", "ToCost", "AdjustmentValue", "IsActive"""";
    public const string LookupColumns = """"Id", "PricingMatrixId", "FromCost", "AdjustmentValue"""";
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "Id", "PricingMatrixId", "FromCost", "ToCost", "AdjustmentValue", "IsActive" };
    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        var column = !string.IsNullOrWhiteSpace(sortBy) && Allowed.Contains(sortBy) ? Allowed.First(x => x.Equals(sortBy, StringComparison.OrdinalIgnoreCase)) : "Id";
        return $"ORDER BY \"{column}\" {dir}";
    }
}
