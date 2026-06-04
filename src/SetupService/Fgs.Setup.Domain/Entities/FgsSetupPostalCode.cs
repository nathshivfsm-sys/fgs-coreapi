namespace Fgs.Setup.Domain.Entities;

public class FgsSetupPostalCode : FgsTenantCompanySetupEntityBase<long>
{
    public string PostalCode { get; set; } = null!;

    public long? FgsSetupZoneId { get; set; }

    public long? FgsSetupTaxId { get; set; }
}
