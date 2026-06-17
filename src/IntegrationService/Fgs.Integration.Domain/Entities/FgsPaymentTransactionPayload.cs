using Fgs.Kernel.Entities;

namespace Fgs.Integration.Domain.Entities;

public class FgsPaymentTransactionPayload : ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long PaymentTransactionId { get; set; }

    public string? RequestJson { get; set; }

    public string? ResponseJson { get; set; }

    public DateTime CreatedOn { get; set; }

    public long CreatedBy { get; set; }
}
