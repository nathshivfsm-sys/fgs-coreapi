namespace Fgs.User.Domain.Entities;

public class FgsSetupServiceAssetModelSerialDescription : FgsTenantCompanySetupEntityBase
{
    public long? FgsSetupServiceAssetManufacturerId { get; set; }

    public string ModelDescription { get; set; } = null!;

    public string? SerialNumberPattern { get; set; }

    public string? Notes { get; set; }
}
