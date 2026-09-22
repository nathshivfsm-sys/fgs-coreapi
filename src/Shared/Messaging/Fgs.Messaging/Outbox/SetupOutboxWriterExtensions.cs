using System.Text.Json;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Abstractions;

namespace Fgs.Messaging.Outbox;

public static class SetupOutboxWriterExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static Task EnqueueEmployeeAccessChangedAsync(
        this IOutboxWriter writer,
        EmployeeAccessChangedEvent evt,
        Guid correlationId,
        CancellationToken cancellationToken = default)
    {
        var payload = JsonSerializer.Serialize(evt, JsonOptions);

        return writer.EnqueueAsync(
            IntegrationEventTypes.EmployeeAccessChanged,
            payload,
            correlationId,
            tenantId: evt.TenantId,
            companyId: evt.CompanyId,
            aggregateType: IntegrationEventTypes.AggregateTypes.Employee,
            aggregateId: evt.EmployeeId.ToString(),
            exchangeName: IntegrationEventExchanges.SetupEvents,
            routingKey: IntegrationEventRoutingKeys.EmployeeAccessChanged,
            cancellationToken: cancellationToken);
    }
}
