namespace Fgs.Messaging.Abstractions;

public interface IOutboxWriter
{
    Task EnqueueAsync(
        string eventType,
        string payload,
        Guid correlationId,
        long? tenantId = null,
        long? companyId = null,
        string? aggregateType = null,
        string? aggregateId = null,
        Guid? causationId = null,
        string? exchangeName = null,
        string? routingKey = null,
        string? headers = null,
        long? createdBy = null,
        CancellationToken cancellationToken = default);
}
