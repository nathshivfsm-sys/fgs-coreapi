namespace Fgs.Setup.Domain.Entities;

public class FgsSetupPostalCode : FgsTenantCompanySetupEntityBase<long>
{
    public string PostalCode { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string StateProvinceCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public decimal TripChargeAmount { get; set; }

    public long? FgsSetupZoneId { get; set; }

    public long? FgsSetupTaxId { get; set; }
}
