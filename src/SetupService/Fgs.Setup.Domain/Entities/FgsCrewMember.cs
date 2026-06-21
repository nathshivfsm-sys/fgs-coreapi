namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Permanent crew membership assignment for a technician profile.
/// </summary>
public class FgsCrewMember : FgsTenantCompanySetupEntityBase<long>
{
    public long CrewId { get; set; }

    public long TechnicianProfileId { get; set; }

    public bool IsLead { get; set; }
}
