namespace Fgs.User.Domain.Entities;

/// <summary>
/// Reusable address / geo row. <see cref="EntityTypeId"/> is the owning entity kind (FK to global entity-type catalog, e.g. GloMasterEntityType).
/// </summary>
public class FgsLocation : FgsEntityBase
{
    public Guid Id { get; set; }

    // Ownership metadata
    public int EntityTypeId { get; set; }

    public long? EntityNumber { get; set; }

    // Address
    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? County { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public string? FormattedAddress { get; set; }

    // Geo location
    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? PlaceId { get; set; }

    public bool IsActive { get; set; } = true;
}
