namespace Fgs.Platform.Application.Audit;

public interface IAuditLogger
{
    Task LogAsync(AuditEntry entry, CancellationToken cancellationToken = default);
}

public sealed record AuditEntry(
    long? TenantId,
    string EntityType,
    string EntityId,
    string Action,
    string? ActorId,
    string? CorrelationId,
    string? Payload);
