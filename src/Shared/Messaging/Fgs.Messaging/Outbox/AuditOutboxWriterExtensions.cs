using System.Text.Json;
using Fgs.Contracts.CredentialAudit;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Abstractions;

namespace Fgs.Messaging.Outbox;

public static class AuditOutboxWriterExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static Task EnqueueCredentialAuditAsync(
        this IOutboxWriter writer,
        RecordCredentialAuditRequest request,
        Guid correlationId,
        CancellationToken cancellationToken = default)
    {
        var evt = new CredentialAuditRequestedEvent(
            request.TenantId,
            request.CompanyId,
            request.CredentialId,
            request.ActionType,
            request.Remarks,
            request.OldVersionNo,
            request.NewVersionNo,
            request.CreatedBy);

        var payload = JsonSerializer.Serialize(evt, JsonOptions);

        return writer.EnqueueAsync(
            IntegrationEventTypes.CredentialAuditRequested,
            payload,
            correlationId,
            tenantId: request.TenantId,
            companyId: request.CompanyId,
            aggregateType: IntegrationEventTypes.AggregateTypes.Credential,
            aggregateId: request.CredentialId.ToString(),
            exchangeName: IntegrationEventExchanges.AuditEvents,
            routingKey: IntegrationEventRoutingKeys.CredentialAuditRequested,
            cancellationToken: cancellationToken);
    }
}
