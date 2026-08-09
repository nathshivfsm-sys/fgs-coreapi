using Fgs.User.Application.Features.ServiceSetups.Dtos;
using Fgs.User.Domain.Enums;

namespace Fgs.User.Infrastructure.Entities.ServiceSetups;

internal sealed class FgsTenantServiceSetupDetailRow
{
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public short TimeCardOptionId { get; set; }
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
    public long BillToStartNumber { get; set; }
    public long POStartNumber { get; set; }
    public long QuoteStartNumber { get; set; }
    public long WorkOrderStartNumber { get; set; }
    public string? InvoiceNumberPrefix { get; set; }
    public string? QuoteNumberPrefix { get; set; }
    public string? PONumberPrefix { get; set; }
    public string? WorkOrderNumberPrefix { get; set; }
    public string? InvoiceBatchNumberFormat { get; set; }
    public bool IsActive { get; set; }

    public FgsTenantServiceSetupDetailDto ToDto() =>
        new(
            TenantId,
            CompanyId,
            (TimeCardOption)TimeCardOptionId,
            AccountingIntegrationTypeId,
            UseExternalTaxCalculationProvider,
            EnableCallBookingWidget,
            EnablePaymentWidget,
            EnableCustomerPortal,
            EnableRulesManagement,
            EnableAutoArrive,
            WorkLocationRadiusForAutoArrive,
            OTStartTime,
            OTEndTime,
            DTStartTime,
            DTEndTime,
            BillHoursFromDispatchOrArrive,
            SourceCodeRequiredOnWorkOrder,
            SourceCodeRequiredOnServiceLocation,
            BillToStartNumber,
            POStartNumber,
            QuoteStartNumber,
            WorkOrderStartNumber,
            InvoiceNumberPrefix,
            QuoteNumberPrefix,
            PONumberPrefix,
            WorkOrderNumberPrefix,
            InvoiceBatchNumberFormat,
            IsActive);
}
