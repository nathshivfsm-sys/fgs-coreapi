using Fgs.Kernel.Entities;

namespace Fgs.Scheduling.Domain.Entities;

public class FgsPayroll : ITenantCompanyScoped
{
    public long Id { get; set; }
    public long TenantId { get; set; }
    public long CompanyId { get; set; }
    public long PayPeriodId { get; set; }
    public long EmployeeId { get; set; }
    public string? EmployeeNumber { get; set; }
    public string EmployeeName { get; set; } = null!;
    public decimal RegularHours { get; set; }
    public decimal OvertimeHours { get; set; }
    public decimal DoubleTimeHours { get; set; }
    public decimal RegularRate { get; set; }
    public decimal OvertimeRate { get; set; }
    public decimal DoubleTimeRate { get; set; }
    public decimal RegularAmount { get; set; }
    public decimal OvertimeAmount { get; set; }
    public decimal DoubleTimeAmount { get; set; }
    public decimal CommissionAmount { get; set; }
    public decimal BonusAmount { get; set; }
    public decimal AdjustmentAmount { get; set; }
    public string BurdenTypeId { get; set; } = "P";
    public decimal BurdenValue { get; set; }
    public decimal BurdenAmount { get; set; }
    public decimal GrossPayAmount { get; set; }
    public long? SignatureFileId { get; set; }
    public DateTimeOffset? SignedOn { get; set; }
    public string? SignedBy { get; set; }
    public string? Notes { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public long CreatedBy { get; set; }
    public DateTimeOffset? UpdatedOn { get; set; }
    public long? UpdatedBy { get; set; }
}
