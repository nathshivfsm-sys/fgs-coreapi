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
}
