using Fgs.Platform.Application.Audit;
using Microsoft.Extensions.Logging;

namespace Fgs.Platform.Infrastructure.Audit;

public sealed class NoOpAuditLogger(ILogger<NoOpAuditLogger> logger) : IAuditLogger
{
    public Task LogAsync(AuditEntry entry, CancellationToken cancellationToken = default)
    {
        logger.LogDebug(
            "Audit placeholder: {Action} on {EntityType}/{EntityId} (TenantId={TenantId}, CorrelationId={CorrelationId})",
            entry.Action,
            entry.EntityType,
            entry.EntityId,
            entry.TenantId,
            entry.CorrelationId);
        return Task.CompletedTask;
    }
}
