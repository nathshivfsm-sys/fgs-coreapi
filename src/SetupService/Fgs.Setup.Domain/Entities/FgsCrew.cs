namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Technician crew definition used for dispatching and scheduling.
/// </summary>
public class FgsCrew : FgsTenantCompanySetupEntityBase<long>
{
    public string CrewCode { get; set; } = null!;

    public string CrewName { get; set; } = null!;

    public string? Description { get; set; }
}
