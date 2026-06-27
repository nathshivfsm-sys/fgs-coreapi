namespace Fgs.Setup.Domain.Entities;

public class FgsSetupTaxAuthority : FgsTenantCompanySetupEntityBase<long>
{
    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? RegionCode { get; set; }

    public bool IsExternalSystemRecord { get; set; }

    public decimal TaxPercent { get; set; }

    public string? Description { get; set; }
}
