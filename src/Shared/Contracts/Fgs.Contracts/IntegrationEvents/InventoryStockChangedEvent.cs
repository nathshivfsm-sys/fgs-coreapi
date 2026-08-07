namespace Fgs.Contracts.IntegrationEvents;

/// <summary>
/// Published when inventory stock is created or quantities/costs are updated.
/// </summary>
public sealed record InventoryStockChangedEvent(
    long TenantId,
    long CompanyId,
    long InventoryStockId,
    long InventoryItemId,
    decimal QuantityOnHand,
    decimal QuantityCommitted,
    decimal QuantityAvailable,
    decimal AverageCost,
    decimal LastCost,
    DateTimeOffset OccurredAtUtc,
    string ChangeKind,
    string Source = "Fgs.Inventory");
