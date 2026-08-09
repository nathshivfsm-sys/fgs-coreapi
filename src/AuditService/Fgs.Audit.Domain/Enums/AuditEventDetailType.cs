namespace Fgs.Audit.Domain.Enums;

/// <summary>
/// Defines the classification of a detail record associated with an event,
/// such as a field change, calculation, validation, workflow action, integration, message, or exception.
/// </summary>
public enum AuditEventDetailType
{
    FIELD_CHANGE,
    CALCULATION,
    VALIDATION,
    WORKFLOW,
    INTEGRATION,
    MESSAGE,
    EXCEPTION,
    NOTE
}
