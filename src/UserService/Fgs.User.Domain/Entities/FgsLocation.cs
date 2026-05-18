namespace Fgs.User.Domain.Entities;

/// <summary>
/// Reusable address / geo row scoped to a tenant company. <see cref="MasterEntityTypeId"/> references <see cref="GloMasterEntityType"/>.
/// </summary>
public class FgsLocation : FgsEntityBase
{
    public Guid Id { get; set; }

    public Guid TenantId { get; set; }

    public long CompanyId { get; set; }

    public int MasterEntityTypeId { get; set; }

    public long? EntityNumber { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? AddressLine3 { get; set; }

    public string? AddressLine4 { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? County { get; set; }

    public string? Country { get; set; }

    public string? PostalCode { get; set; }

    public string? FormattedAddress { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public string? PlaceId { get; set; }

    public bool IsActive { get; set; } = true;
}
