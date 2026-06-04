namespace Fgs.Setup.Domain.Entities;

/// <summary>
/// Model reference catalog for service assets (type, manufacturer, model metadata).
/// </summary>
public class FgsSetupServiceAssetModelReference : FgsTenantCompanySetupEntityBase<long>
{
    public long FgsSetupServiceAssetTypeId { get; set; }

    public long FgsSetupServiceAssetManufacturerId { get; set; }

    public string? ModelNumber { get; set; }

    public string ModelDescription { get; set; } = null!;

    public string? SerialNumberPattern { get; set; }

    public string? Notes { get; set; }

    /// <summary>JSON array stored as PostgreSQL <c>jsonb</c>.</summary>
    public string? UrlsJson { get; set; }

    public FgsSetupServiceAssetType? ServiceAssetType { get; set; }

    public FgsSetupServiceAssetManufacturer? ServiceAssetManufacturer { get; set; }
}
