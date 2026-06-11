using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class CrmContactCommunication : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long ContactId { get; set; }

    public short CommunicationTypeId { get; set; }

    public string? Label { get; set; }

    public string Value { get; set; } = null!;

    public string? Extension { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsPrimary { get; set; }

    public bool IsActive { get; set; } = true;
}
