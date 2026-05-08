namespace UserService.Domain.Entities;

public sealed class CompanyConfiguration : AuditableEntity
{
    public long Id { get; private set; }
    public long CompanyId { get; private set; }
    public int TimeCardOptionId { get; private set; }
    public int? AccountingIntegrationTypeId { get; private set; }
    public bool EnableCallBookingWidget { get; private set; }
    public bool EnablePaymentWidget { get; private set; }
    public bool EnableCustomerPortal { get; private set; }
    public bool EnableRulesManagement { get; private set; }
    public bool EnableAutoArrive { get; private set; }
    public int? WorkLocationRadiusForAutoArrive { get; private set; }
    public TimeOnly? OTStartTime { get; private set; }
    public TimeOnly? OTEndTime { get; private set; }
    public TimeOnly? DTStartTime { get; private set; }
    public TimeOnly? DTEndTime { get; private set; }
    public string BillHoursFromDispatchOrArrive { get; private set; } = null!;
    public bool SourceCodeRequiredOnWorkOrder { get; private set; }
    public bool SourceCodeRequiredOnServiceLocation { get; private set; }
    public long BillToStartNumber { get; private set; }
    public long POStartNumber { get; private set; }
    public long QuoteStartNumber { get; private set; }
    public long WorkOrderStartNumber { get; private set; }
    public string? InvoiceNumberPrefix { get; private set; }
    public string? QuoteNumberPrefix { get; private set; }
    public string? PONumberPrefix { get; private set; }
    public string? WorkOrderNumberPrefix { get; private set; }
    public string? InvoiceBatchNumberFormat { get; private set; }
    public bool IsActive { get; private set; }

    public Company Company { get; private set; } = null!;
    public FSGSetupTimeCardOption TimeCardOption { get; private set; } = null!;
    public FSGSetupAccountingIntegrationType? AccountingIntegrationType { get; private set; }

    private CompanyConfiguration()
    {
    }
}
