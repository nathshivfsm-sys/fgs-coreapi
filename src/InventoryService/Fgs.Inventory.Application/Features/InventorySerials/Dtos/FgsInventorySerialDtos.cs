using Fgs.Inventory.Domain.Enums;

namespace Fgs.Inventory.Application.Features.InventorySerials.Dtos;

public sealed record FgsInventorySerialSummaryDto(
    long Id,
    long InventoryItemId,
    string SerialNumber,
    FgsInventorySerialStatus InventorySerialStatus,
    string? Notes,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsInventorySerialDetailDto(
    long Id,
    long InventoryItemId,
    string SerialNumber,
    FgsInventorySerialStatus InventorySerialStatus,
    string? Notes,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsInventorySerialLookupDto(
    long Id,
    string SerialNumber,
    long InventoryItemId,
    FgsInventorySerialStatus InventorySerialStatus);

public sealed record FgsInventorySerialCreateDto(
    long InventoryItemId,
    string SerialNumber,
    FgsInventorySerialStatus InventorySerialStatus = FgsInventorySerialStatus.Available,
    string? Notes = null);

public sealed record FgsInventorySerialUpdateDto(
    long InventoryItemId,
    string SerialNumber,
    FgsInventorySerialStatus InventorySerialStatus,
    string? Notes);

public sealed record FgsInventorySerialPatchDto(
    long? InventoryItemId,
    string? SerialNumber,
    FgsInventorySerialStatus? InventorySerialStatus,
    string? Notes);

public sealed record FgsInventorySerialListFilters(
    long? InventoryItemId = null,
    FgsInventorySerialStatus? InventorySerialStatus = null,
    string? SerialNumber = null);
