using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixOthers;

internal static class FgsSetupPricingMatrixOtherSql
{
    public const string Table = "setup.\"FgsSetupPricingMatrixOther\"";
    public const string DetailColumns = """"Id", "PricingMatrixId", "CategoryCode", "Name", "AdjustmentValue", "DiscountPercent", "IsActive"""";
    public const string LookupColumns = """"Id", "PricingMatrixId", "CategoryCode", "Name"""";
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "Id", "PricingMatrixId", "CategoryCode", "Name", "AdjustmentValue", "DiscountPercent", "IsActive" };
    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, Allowed);

}
