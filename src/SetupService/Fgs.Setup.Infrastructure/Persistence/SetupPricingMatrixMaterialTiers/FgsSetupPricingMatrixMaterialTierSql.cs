using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixMaterialTiers;

internal static class FgsSetupPricingMatrixMaterialTierSql
{
    public const string Table = "setup.\"FgsSetupPricingMatrixMaterialTier\"";
    public const string DetailColumns = """"Id", "PricingMatrixId", "FromCost", "ToCost", "AdjustmentValue", "IsActive"""";
    public const string LookupColumns = """"Id", "PricingMatrixId", "FromCost", "AdjustmentValue"""";
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "Id", "PricingMatrixId", "FromCost", "ToCost", "AdjustmentValue", "IsActive" };
    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, Allowed);

}
