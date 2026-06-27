using Fgs.Kernel.Entities;

namespace Fgs.Scheduling.Domain.Entities;

public class FgsWorkOrderIntegration : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long? WorkOrderId { get; set; }
    public string IntegrationName { get; set; } = null!;
    public string ExternalId { get; set; } = null!;
    public string? ExternalWorkOrderNumber { get; set; }
    public DateTimeOffset ReceivedOn { get; set; }
    public string Status { get; set; } = "Received";
    public string Payload { get; set; } = null!;
    public DateTimeOffset? ProcessedOn { get; set; }
    public string? ProcessedBy { get; set; }
}
