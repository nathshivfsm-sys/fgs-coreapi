namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Stores the master list of Job Categories available for configuring Job Types.
/// </summary>
public class FgsJobCategory : FgsTenantCompanySetupEntityBase<long>
{
    public string CategoryCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public short DisplayOrder { get; set; } = 1;
}
