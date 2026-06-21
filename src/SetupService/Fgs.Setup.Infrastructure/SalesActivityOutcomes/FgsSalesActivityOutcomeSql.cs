using Fgs.Foundation.CatalogCrud;

namespace Fgs.Setup.Infrastructure.SalesActivityOutcomes;

internal static class FgsSalesActivityOutcomeSql
{
    public const string Table = "setup.\"FgsSalesActivityOutcome\"";

    public const string SelectDetailColumns = """
        "Id", "TenantId", "CompanyId", "OutcomeCode", "OutcomeName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "NextSalesPipelineStatusId", "IsTerminal", "RequireComment", "AllowManualSelection", "IsActive", "CreatedOn", "CreatedBy", "UpdatedOn", "UpdatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "TenantId", "CompanyId", "OutcomeCode", "OutcomeName", "Description", "DisplayOrder", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "NextSalesPipelineStatusId", "IsTerminal", "RequireComment", "AllowManualSelection", "IsActive", "CreatedOn", "UpdatedOn"
        """;

    public const string SelectLookupColumns = """
        "Id", "OutcomeCode", "OutcomeName", "DisplayOrder"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "CreatedOn", "IsActive", "DisplayOrder", "OutcomeCode", "OutcomeName", "Description", "IsSystem", "AppliesToLead", "AppliesToOpportunity", "NextSalesPipelineStatusId", "IsTerminal", "RequireComment", "AllowManualSelection"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"OutcomeName\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
            ? $"ORDER BY \"DisplayOrder\" {dir} NULLS LAST, \"OutcomeName\" {dir}"
            : $"ORDER BY \"{column}\" {dir}";
    }
}
