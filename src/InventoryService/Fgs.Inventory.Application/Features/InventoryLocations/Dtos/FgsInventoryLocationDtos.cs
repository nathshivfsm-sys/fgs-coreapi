namespace Fgs.Inventory.Application.Features.InventoryLocations.Dtos;

public sealed record FgsInventoryLocationSummaryDto(
    long Id,
    string InventoryLocationCode,
    string Name,
    string InventoryLocationType,
    long? ParentInventoryLocationId,
    string? Description,
    string? Address1,
    string? Address2,
    string? City,
    string? StateProvince,
    string? PostalCode,
    string? Country,
    string? ContactName,
    string? PhoneNumber,
    string? Email,
    bool IsDefault,
    bool IsActive);

public sealed record FgsInventoryLocationDetailDto(
    long Id,
    string InventoryLocationCode,
    string Name,
    string InventoryLocationType,
    long? ParentInventoryLocationId,
    string? Description,
    string? Address1,
    string? Address2,
    string? City,
    string? StateProvince,
    string? PostalCode,
    string? Country,
    string? ContactName,
    string? PhoneNumber,
    string? Email,
    bool IsDefault,
    bool IsActive);

public sealed record FgsInventoryLocationLookupDto(
    long Id,
    string InventoryLocationCode,
    string Name);

public sealed record FgsInventoryLocationCreateDto(
    string InventoryLocationCode,
    string Name,
    string InventoryLocationType,
    long? ParentInventoryLocationId,
    string? Description,
    string? Address1,
    string? Address2,
    string? City,
    string? StateProvince,
    string? PostalCode,
    string? Country,
    string? ContactName,
    string? PhoneNumber,
    string? Email,
    bool IsDefault);

public sealed record FgsInventoryLocationUpdateDto(
    string InventoryLocationCode,
    string Name,
    string InventoryLocationType,
    long? ParentInventoryLocationId,
    string? Description,
    string? Address1,
    string? Address2,
    string? City,
    string? StateProvince,
    string? PostalCode,
    string? Country,
    string? ContactName,
    string? PhoneNumber,
    string? Email,
    bool IsDefault);

public sealed record FgsInventoryLocationPatchDto(
    string? InventoryLocationCode,
    string? Name,
    string? InventoryLocationType,
    long? ParentInventoryLocationId,
    string? Description,
    string? Address1,
    string? Address2,
    string? City,
    string? StateProvince,
    string? PostalCode,
    string? Country,
    string? ContactName,
    string? PhoneNumber,
    string? Email,
    bool? IsDefault,
    bool? IsActive);

public sealed record FgsInventoryLocationListFilters(
    string? InventoryLocationCode = null,
    string? Name = null);
