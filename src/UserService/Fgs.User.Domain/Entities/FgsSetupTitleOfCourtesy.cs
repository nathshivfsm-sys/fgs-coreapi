namespace Fgs.User.Domain.Entities;

public class FgsSetupTitleOfCourtesy : FgsTenantCompanySetupEntityBase
{
    public string Code { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public int? SortOrder { get; set; }
}
