using Fgs.User.Domain.Enums;

namespace Fgs.User.Domain.Entities;

/// <summary>
/// Per-company service / operations configuration (replaces legacy FgsTenantCompanyConfiguration).
/// </summary>
public class FgsTenantServiceSetup : FgsEntityBase, ITenantCompanyScoped
{
    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public TimeCardOption TimeCardOptionId { get; set; } = TimeCardOption.None;

    public int? AccountingIntegrationTypeId { get; set; }

    public bool UseExternalTaxCalculationProvider { get; set; }

    public bool EnableCallBookingWidget { get; set; }

    public bool EnablePaymentWidget { get; set; }

    public bool EnableCustomerPortal { get; set; }

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

    /// <summary>
    /// Controls when estimate revisions are created.
    /// See <see cref="EstimateRevisionCreationModes"/>.
    /// </summary>
    public string EstimateRevisionCreationMode { get; set; } = EstimateRevisionCreationModes.OnDemand;

    public bool IsActive { get; set; } = true;
}
