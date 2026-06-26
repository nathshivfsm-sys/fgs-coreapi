namespace Fgs.Setup.Domain.Entities;

public class FgsSetupTitleOfCourtesy : FgsTenantCompanySetupEntityBase<long>
{
    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int? SortOrder { get; set; }
}
