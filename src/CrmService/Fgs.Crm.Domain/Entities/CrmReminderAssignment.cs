using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class CrmReminderAssignment : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long ReminderId { get; set; }

    public long? UserId { get; set; }

    public long? RoleId { get; set; }

    public string? ResponseText { get; set; }

    public DateTimeOffset? CompletedOn { get; set; }

    public long? CompletedByUserId { get; set; }
}
