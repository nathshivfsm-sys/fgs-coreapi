using Fgs.Contracts.Audit;

namespace Fgs.Contracts.IntegrationEvents;

/// <summary>
/// Published when a product audit event should be written to the Audit service.
/// </summary>
public sealed record AuditEventRequestedEvent(RecordAuditEventRequest Request);
