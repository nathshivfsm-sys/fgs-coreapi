namespace Fgs.Setup.Domain.Entities;

public class FgsSetupTax : FgsTenantCompanySetupEntityBase<long>
{
    public string TaxCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsExternalSystemRecord { get; set; }

    public string? ExternalSystemId { get; set; }

    public string? SyncToken { get; set; }

    public bool ShowTaxDetail { get; set; }

    public string? Description { get; set; }

    public ICollection<FgsSetupTaxDetail> TaxDetails { get; set; } = [];
}
