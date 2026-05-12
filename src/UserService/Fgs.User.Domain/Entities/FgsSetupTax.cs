namespace Fgs.User.Domain.Entities;

public class FgsSetupTax : FgsTenantCompanySetupEntityBase
{
    public string TaxCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}
