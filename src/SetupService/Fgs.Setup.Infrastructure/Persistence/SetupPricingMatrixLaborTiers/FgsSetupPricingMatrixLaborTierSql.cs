using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SetupPricingMatrixLaborTiers;

internal static class FgsSetupPricingMatrixLaborTierSql
{
    public const string Table = "setup.\"FgsSetupPricingMatrixLaborTier\"";
    public const string DetailColumns = """"Id", "PricingMatrixLaborId", "SequenceOrder", "DurationMinutes", "Rate", "TechSkillLevelId", "IsActive"""";
    public const string LookupColumns = """"Id", "PricingMatrixLaborId", "SequenceOrder"""";
    private static readonly HashSet<string> Allowed = new(StringComparer.OrdinalIgnoreCase) { "Id", "PricingMatrixLaborId", "SequenceOrder", "DurationMinutes", "Rate", "TechSkillLevelId", "IsActive" };
    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(sortBy, direction, Allowed);

}
