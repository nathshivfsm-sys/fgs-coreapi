using Fgs.Kernel.Entities;

namespace Fgs.Scheduling.Domain.Entities;

public class FgsDispatchBoardTechnician : FgsEntityBase, ITenantCompanyScoped
{
    public long Id { get; set; }

    public long TenantId { get; set; }

    public long CompanyId { get; set; }

    public DateOnly ServiceDate { get; set; }

    public long TechnicianProfileId { get; set; }

    public string TechCode { get; set; } = null!;

    public string TechName { get; set; } = null!;

    public long? CrewId { get; set; }

    public string? CrewCode { get; set; }

    public string? CrewName { get; set; }

    public short DispatchBoardStatusId { get; set; }

    public bool IsWorking { get; set; } = true;
}
