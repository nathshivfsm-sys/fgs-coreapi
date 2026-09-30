using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.ServiceSetups;

internal static class FgsTenantServiceSetupSql
{
    public const string Table = "tenant.\"FgsTenantServiceSetup\"";

    public const string SelectDetailColumns = """
        "TenantId", "CompanyId", "TimeCardOptionId", "AccountingIntegrationTypeId",
        "UseExternalTaxCalculationProvider", "EnableCallBookingWidget", "EnablePaymentWidget",
        "EnableCustomerPortal", "EnableRulesManagement", "EnableAutoArrive",
        "WorkLocationRadiusForAutoArrive", "OTStartTime", "OTEndTime", "DTStartTime", "DTEndTime",
        "BillHoursFromDispatchOrArrive", "SourceCodeRequiredOnWorkOrder", "SourceCodeRequiredOnServiceLocation",
        "BillToStartNumber", "POStartNumber", "QuoteStartNumber", "WorkOrderStartNumber",
        "InvoiceNumberPrefix", "QuoteNumberPrefix", "PONumberPrefix", "WorkOrderNumberPrefix",
        "InvoiceBatchNumberFormat", "EstimateRevisionCreationMode", "IsActive"
        """;

    public const string SelectSummaryColumns = SelectDetailColumns;

    public const string SelectLookupColumns = """
        "CompanyId", "IsActive"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "CompanyId",
        "TimeCardOptionId",
        "EstimateRevisionCreationMode",
        "BillHoursFromDispatchOrArrive",
        "BillToStartNumber",
        "POStartNumber",
        "QuoteStartNumber",
        "WorkOrderStartNumber",
        "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"CompanyId\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
