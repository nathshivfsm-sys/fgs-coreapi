namespace Fgs.User.Domain.Entities;

public class FgsSetupPostalCode : FgsTenantCompanySetupEntityBase
{
    public string PostalCode { get; set; } = null!;

    public long? FgsSetupZoneId { get; set; }

    public long? FgsSetupTaxId { get; set; }
}
