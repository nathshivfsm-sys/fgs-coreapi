using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class CrmNote : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public int EntityTypeId { get; set; }

    public long EntityId { get; set; }

    public short NoteTypeId { get; set; }

    public string? Title { get; set; }

    public string NoteText { get; set; } = null!;

    public short DisplayOrder { get; set; } = 1;

    public bool IsPinned { get; set; }

    public bool IsActive { get; set; } = true;
}
