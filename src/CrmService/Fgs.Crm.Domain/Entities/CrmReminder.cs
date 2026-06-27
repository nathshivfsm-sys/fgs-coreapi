using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class CrmReminder : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public int? EntityId { get; set; }

    public long? EntityValue { get; set; }

    public short PriorityId { get; set; } = 2;

    public short StatusId { get; set; } = 1;

    public string Subject { get; set; } = null!;

    public string ReminderText { get; set; } = null!;

    public DateTimeOffset DueOn { get; set; }
}
