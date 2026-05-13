namespace Fgs.User.Domain.Entities;

public class FgsSetupTax : FgsTenantCompanySetupEntityBase
{
    public string TaxCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsExternalSystemRecord { get; set; }

    public string? Description { get; set; }
}
