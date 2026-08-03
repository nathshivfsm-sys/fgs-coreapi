using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixOthers;

internal static class FgsSetupPricingMatrixOtherSql
{
    public const string Table = "setup.\"FgsSetupPricingMatrixOther\"";
    public const string DetailColumns = """"Id", "PricingMatrixId", "CategoryCode", "Name", "AdjustmentValue", "DiscountPercent", "IsActive"""";
    public const string LookupColumns = """"Id", "PricingMatrixId", "CategoryCode", "Name"""";
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "Id", "PricingMatrixId", "CategoryCode", "Name", "AdjustmentValue", "DiscountPercent", "IsActive" };
    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        var column = !string.IsNullOrWhiteSpace(sortBy) && Allowed.Contains(sortBy) ? Allowed.First(x => x.Equals(sortBy, StringComparison.OrdinalIgnoreCase)) : "Id";
        return $"ORDER BY \"{column}\" {dir}";
    }
}
