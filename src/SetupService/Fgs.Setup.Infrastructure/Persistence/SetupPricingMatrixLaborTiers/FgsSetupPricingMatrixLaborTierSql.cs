using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixLaborTiers;

internal static class FgsSetupPricingMatrixLaborTierSql
{
    public const string Table = "setup.\"FgsSetupPricingMatrixLaborTier\"";
    public const string DetailColumns = """"Id", "PricingMatrixLaborId", "SequenceOrder", "DurationMinutes", "Rate", "TechSkillLevelId", "IsActive"""";
    public const string LookupColumns = """"Id", "PricingMatrixLaborId", "SequenceOrder"""";
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "Id", "PricingMatrixLaborId", "SequenceOrder", "DurationMinutes", "Rate", "TechSkillLevelId", "IsActive" };
    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        var column = !string.IsNullOrWhiteSpace(sortBy) && Allowed.Contains(sortBy) ? Allowed.First(x => x.Equals(sortBy, StringComparison.OrdinalIgnoreCase)) : "Id";
        return $"ORDER BY \"{column}\" {dir}";
    }
}
