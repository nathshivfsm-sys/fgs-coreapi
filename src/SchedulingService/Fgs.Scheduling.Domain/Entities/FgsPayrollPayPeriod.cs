using Fgs.Kernel.Entities;

namespace Fgs.Scheduling.Domain.Entities;

public class FgsPayrollPayPeriod : ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public string PayPeriodCode { get; set; } = null!;
    public DateOnly PeriodStartDate { get; set; }
    public DateOnly PeriodEndDate { get; set; }
    public short PayrollStatusId { get; set; } = 1;
    public DateTimeOffset? CalculatedOn { get; set; }
    public long? CalculatedBy { get; set; }
    public DateTimeOffset? ApprovedOn { get; set; }
    public long? ApprovedBy { get; set; }
    public DateTimeOffset? ExportedOn { get; set; }
    public long? ExportedBy { get; set; }
    public string? ExportReference { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public long CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public long? UpdatedBy { get; set; }
}
