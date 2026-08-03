namespace Fgs.Inventory.Application.Features.InventoryTransactions.Dtos;

public sealed record FgsInventoryTransactionSummaryDto(
    long Id,
    string TransactionNumber,
    long InventoryItemId,
    string? SerialNumber,
    string TransactionType,
    decimal Quantity,
    long? FromInventoryLocationId,
    long? ToInventoryLocationId,
    decimal UnitCost,
    DateTimeOffset TransactionDate,
    string? ReferenceType,
    long? ReferenceId,
    string? Notes,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsInventoryTransactionDetailDto(
    long Id,
    string TransactionNumber,
    long InventoryItemId,
    string? SerialNumber,
    string TransactionType,
    decimal Quantity,
    long? FromInventoryLocationId,
    long? ToInventoryLocationId,
    decimal UnitCost,
    DateTimeOffset TransactionDate,
    string? ReferenceType,
    long? ReferenceId,
    string? Notes,
    DateTimeOffset CreatedOn,
    string? CreatedBy,
    DateTimeOffset? UpdatedOn,
    string? UpdatedBy);

public sealed record FgsInventoryTransactionCreateDto(
    string TransactionNumber,
    long InventoryItemId,
    string? SerialNumber,
    string TransactionType,
    decimal Quantity,
    long? FromInventoryLocationId,
    long? ToInventoryLocationId,
    decimal UnitCost,
    DateTimeOffset? TransactionDate,
    string? ReferenceType,
    long? ReferenceId,
    string? Notes);

public sealed record FgsInventoryTransactionListFilters(
    long? InventoryItemId = null,
    string? TransactionType = null);
