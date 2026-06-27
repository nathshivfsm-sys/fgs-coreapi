using Fgs.Inventory.Application.Features.InventoryLocations.Dtos;

namespace Fgs.Inventory.Infrastructure.InventoryLocations;

internal sealed class FgsInventoryLocationSummaryRow
{
    public long Id { get; set; }
    public string InventoryLocationCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string InventoryLocationType { get; set; } = null!;
    public long? ParentInventoryLocationId { get; set; }
    public string? Description { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? StateProvince { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? ContactName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }

    public FgsInventoryLocationSummaryDto ToDto() =>
        new(
            Id,
            InventoryLocationCode,
            Name,
            InventoryLocationType,
            ParentInventoryLocationId,
            Description,
            Address1,
            Address2,
            City,
            StateProvince,
            PostalCode,
            Country,
            ContactName,
            PhoneNumber,
            Email,
            IsDefault,
            IsActive);
}

internal sealed class FgsInventoryLocationDetailRow
{
    public long Id { get; set; }
    public string InventoryLocationCode { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string InventoryLocationType { get; set; } = null!;
    public long? ParentInventoryLocationId { get; set; }
    public string? Description { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? City { get; set; }
    public string? StateProvince { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? ContactName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public bool IsDefault { get; set; }
    public bool IsActive { get; set; }

    public FgsInventoryLocationDetailDto ToDto() =>
        new(
            Id,
            InventoryLocationCode,
            Name,
            InventoryLocationType,
            ParentInventoryLocationId,
            Description,
            Address1,
            Address2,
            City,
            StateProvince,
            PostalCode,
            Country,
            ContactName,
            PhoneNumber,
            Email,
            IsDefault,
            IsActive);
}

internal sealed class FgsInventoryLocationLookupRow
{
    public long Id { get; set; }
    public string InventoryLocationCode { get; set; } = null!;
    public string Name { get; set; } = null!;

    public FgsInventoryLocationLookupDto ToDto() => new(Id, InventoryLocationCode, Name);
}
