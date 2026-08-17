using Fgs.Foundation.Paging;
using Fgs.Setup.Infrastructure.Common;

namespace Fgs.Setup.Infrastructure.Persistence.SalesActivityOutcomes;

internal static class FgsSalesActivityOutcomeSql
{
    public const string Table = "setup.\"FgsSalesActivityOutcome\"";

    public const string SelectDetailColumns = """
        "Id", "OutcomeCode", "OutcomeName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "NextSalesPipelineStatusId", "IsTerminal", "RequireComment", "AllowManualSelection", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "OutcomeCode", "OutcomeName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "NextSalesPipelineStatusId", "IsTerminal", "RequireComment", "AllowManualSelection", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "OutcomeCode", "OutcomeName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "IsActive", "DisplayOrder", "OutcomeCode", "OutcomeName", "Description", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "NextSalesPipelineStatusId", "IsTerminal", "RequireComment", "AllowManualSelection"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
        => SetupSqlOrderBy.Resolve(
            sortBy,
            direction,
            AllowedSortColumns,
            nullsLastTiebreakerColumn: "OutcomeName");

}