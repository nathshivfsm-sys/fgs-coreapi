namespace Fgs.User.Domain.Entities;

public class FgsTenantCompanyConfiguration : FgsEntityBase
{
    public Guid TenantId { get; set; }

    public long CompanyId { get; set; }

    public int TimeCardOptionId { get; set; }

    public int? AccountingIntegrationTypeId { get; set; }

    public bool EnableCallBookingWidget { get; set; } = true;

    public bool EnablePaymentWidget { get; set; } = true;

    public bool EnableCustomerPortal { get; set; } = true;

    public bool EnableRulesManagement { get; set; }

    public bool EnableAutoArrive { get; set; }

    public int? WorkLocationRadiusForAutoArrive { get; set; }

    public TimeSpan? OTStartTime { get; set; }

    public TimeSpan? OTEndTime { get; set; }

    public TimeSpan? DTStartTime { get; set; }

    public TimeSpan? DTEndTime { get; set; }

    public string BillHoursFromDispatchOrArrive { get; set; } = null!;

    public bool SourceCodeRequiredOnWorkOrder { get; set; }

    public bool SourceCodeRequiredOnServiceLocation { get; set; }

    public long BillToStartNumber { get; set; } = 100;

    public long POStartNumber { get; set; } = 100;

    public long QuoteStartNumber { get; set; } = 100;

    public long WorkOrderStartNumber { get; set; } = 100;

    public string? InvoiceNumberPrefix { get; set; }

    public string? QuoteNumberPrefix { get; set; }

    public string? PONumberPrefix { get; set; }

    public string? WorkOrderNumberPrefix { get; set; }

    public string? InvoiceBatchNumberFormat { get; set; }

    public bool IsActive { get; set; } = true;
}
