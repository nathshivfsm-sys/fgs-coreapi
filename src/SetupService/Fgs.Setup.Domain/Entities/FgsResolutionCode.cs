namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Tenant-scoped resolution codes linked to <see cref="GloResolutionType"/>.
/// </summary>
public class FgsResolutionCode : FgsTenantCompanySetupEntityBase<long>
{
    public int GloResolutionTypeId { get; set; }

    public string ResolutionCode { get; set; } = null!;

    public string ResolutionName { get; set; } = null!;

    public bool IsMobileVisible { get; set; } = true;

    public GloResolutionTypeCache? ResolutionType { get; set; }
}
