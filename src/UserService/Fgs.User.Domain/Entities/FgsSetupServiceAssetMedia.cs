namespace Fgs.User.Domain.Entities;

public class FgsSetupServiceAssetMedia : FgsTenantCompanySetupEntityBase
{
    public long? FgsSetupServiceAssetTypeId { get; set; }

    public string Title { get; set; } = null!;

    public string MediaUrl { get; set; } = null!;

    public string? ContentType { get; set; }
}
