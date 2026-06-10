namespace Fgs.Setup.Application.Features.Generated.Dtos;

/// <summary>FgsInventoryItemAlternate</summary>
public sealed record FgsInventoryItemAlternateSummaryDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>InventoryItemId</summary>
    long InventoryItemId,
    /// <summary>AlternateInventoryItemId</summary>
    long AlternateInventoryItemId,
    /// <summary>AlternateType</summary>
    string? AlternateType,
    /// <summary>PriorityOrder</summary>
    short PriorityOrder,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsInventoryItemAlternateDetailDto(
    /// <summary>Id</summary>
    long Id,
    /// <summary>TenantId</summary>
    long TenantId,
    /// <summary>CompanyId</summary>
    long CompanyId,
    /// <summary>InventoryItemId</summary>
    long InventoryItemId,
    /// <summary>AlternateInventoryItemId</summary>
    long AlternateInventoryItemId,
    /// <summary>AlternateType</summary>
    string? AlternateType,
    /// <summary>PriorityOrder</summary>
    short PriorityOrder,
    /// <summary>Notes</summary>
    string? Notes,
    /// <summary>CreatedOn</summary>
    DateTimeOffset CreatedOn,
    /// <summary>CreatedBy</summary>
    string? CreatedBy,
    /// <summary>UpdatedOn</summary>
    DateTimeOffset? UpdatedOn,
    /// <summary>UpdatedBy</summary>
    string? UpdatedBy,
    /// <summary>IsActive</summary>
    bool IsActive)
;

public sealed record FgsInventoryItemAlternateCreateDto(
    /// <summary>InventoryItemId</summary>
    long InventoryItemId,
    /// <summary>AlternateInventoryItemId</summary>
    long AlternateInventoryItemId,
    /// <summary>AlternateType</summary>
    string? AlternateType,
    /// <summary>PriorityOrder</summary>
    short PriorityOrder,
    /// <summary>Notes</summary>
    string? Notes)
;

public sealed record FgsInventoryItemAlternateUpdateDto(
    /// <summary>InventoryItemId</summary>
    long InventoryItemId,
    /// <summary>AlternateInventoryItemId</summary>
    long AlternateInventoryItemId,
    /// <summary>AlternateType</summary>
    string? AlternateType,
    /// <summary>PriorityOrder</summary>
    short PriorityOrder,
    /// <summary>Notes</summary>
    string? Notes)
;

public sealed record FgsInventoryItemAlternatePatchDto(
    /// <summary>InventoryItemId</summary>
    long? InventoryItemId,
    /// <summary>AlternateInventoryItemId</summary>
    long? AlternateInventoryItemId,
    /// <summary>AlternateType</summary>
    string? AlternateType,
    /// <summary>PriorityOrder</summary>
    short? PriorityOrder,
    /// <summary>Notes</summary>
    string? Notes)
;

