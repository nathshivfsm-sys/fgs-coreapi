namespace Fgs.Contracts.IntegrationEvents;

/// <summary>
/// Published when a purchase order status is set on create or changes on update/patch.
/// </summary>
public sealed record PurchaseOrderStatusChangedEvent(
    long TenantId,
    long CompanyId,
    long PurchaseOrderId,
    string PurchaseOrderNumber,
    string? PreviousStatus,
    string NewStatus,
    DateTimeOffset OccurredAtUtc,
    string Source = "Fgs.Inventory");
