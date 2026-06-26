namespace Fgs.Setup.Application.Features.Warehouses.Dtos;

public sealed record FgsWarehouseSummaryDto(
    long Id,
    string WarehouseCode,
    string Name,
    string WarehouseType,
    Guid? AddressId,
    string? Description,
    bool IsDefault,
    bool IsActive);

public sealed record FgsWarehouseDetailDto(
    long Id,
    string WarehouseCode,
    string Name,
    string WarehouseType,
    Guid? AddressId,
    string? Description,
    bool IsDefault,
    bool IsActive);

public sealed record FgsWarehouseLookupDto(
    long Id,
    string WarehouseCode,
    string Name);

public sealed record FgsWarehouseCreateDto(
    string WarehouseCode,
    string Name,
    string WarehouseType,
    Guid? AddressId,
    string? Description,
    bool IsDefault);

public sealed record FgsWarehouseUpdateDto(
    string WarehouseCode,
    string Name,
    string WarehouseType,
    Guid? AddressId,
    string? Description,
    bool IsDefault);

public sealed record FgsWarehousePatchDto(
    string? WarehouseCode,
    string? Name,
    string? WarehouseType,
    Guid? AddressId,
    string? Description,
    bool? IsDefault,
    bool? IsActive);

public sealed record FgsWarehouseListFilters(
    string? WarehouseCode = null,
    string? Name = null);
