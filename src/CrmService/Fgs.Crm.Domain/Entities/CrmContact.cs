using Fgs.Kernel.Entities;

namespace Fgs.Crm.Domain.Entities;

public class CrmContact : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public long? CustomerId { get; set; }

    public long? ServiceLocationId { get; set; }

    public string DisplayName { get; set; } = null!;

    public string? Title { get; set; }

    public string? DepartmentName { get; set; }

    public short DisplayOrder { get; set; } = 1;

    public bool IsDefaultContact { get; set; }

    public bool CanReceiveEstimates { get; set; }

    public bool CanReceiveInvoices { get; set; }

    public bool CanReceiveAppointments { get; set; } = true;

    public bool IsActive { get; set; } = true;
}
