using System.Text.Json;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Abstractions;

namespace Fgs.Messaging.Outbox;

public static class InventoryOutboxWriterExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static Task EnqueueInventoryStockChangedAsync(
        this IOutboxWriter writer,
        InventoryStockChangedEvent evt,
        Guid correlationId,
        CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(evt, JsonOptions);

        return writer.EnqueueAsync(
            IntegrationEventTypes.InventoryStockChanged,
            payload,
            correlationId,
            tenantId: evt.TenantId,
            companyId: evt.CompanyId,
            aggregateType: IntegrationEventTypes.AggregateTypes.InventoryStock,
            aggregateId: evt.InventoryStockId.ToString(),
            exchangeName: IntegrationEventExchanges.InventoryEvents,
            routingKey: IntegrationEventRoutingKeys.InventoryStockChanged,
            cancellationToken: cancellationToken);
    }

    public static Task EnqueuePurchaseOrderStatusChangedAsync(
        this IOutboxWriter writer,
        PurchaseOrderStatusChangedEvent evt,
        Guid correlationId,
        CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(evt, JsonOptions);

        return writer.EnqueueAsync(
            IntegrationEventTypes.PurchaseOrderStatusChanged,
            payload,
            correlationId,
            tenantId: evt.TenantId,
            companyId: evt.CompanyId,
            aggregateType: IntegrationEventTypes.AggregateTypes.PurchaseOrder,
            aggregateId: evt.PurchaseOrderId.ToString(),
            exchangeName: IntegrationEventExchanges.InventoryEvents,
            routingKey: IntegrationEventRoutingKeys.PurchaseOrderStatusChanged,
            cancellationToken: cancellationToken);
    }
}
