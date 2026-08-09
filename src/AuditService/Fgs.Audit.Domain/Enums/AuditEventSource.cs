namespace Fgs.Audit.Domain.Enums;

/// <summary>
/// Defines the application, service, integration, or process that originated an event.
/// </summary>
public enum AuditEventSource
{
    WEB,
    MOBILE,
    PORTAL,
    API,
    IMPORT,
    EXPORT,
    WORKER,
    SCHEDULER,
    RABBITMQ,
    QBO,
    EMAIL,
    SMS,
    SYSTEM
}
