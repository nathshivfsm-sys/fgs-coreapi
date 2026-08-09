namespace Fgs.Notification.Domain.Enums;

/// <summary>
/// Identifies the FGS application or component that originated the outbound notification.
/// </summary>
public enum NotificationSourceApplication
{
    FgsWeb,
    FgsMobile,
    CustomerPortal,
    TechnicianPortal,
    WorkflowEngine,
    Scheduler,
    CreditCard,
    CreditCardWidget,
    Api,
    FgsAddon
}
