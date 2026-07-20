namespace Fgs.Notification.Domain.Enums;

/// <summary>
/// Current notification delivery status.
/// </summary>
public enum NotificationStatus
{
    Queued,
    Sending,
    Sent,
    Delivered,
    Opened,
    Clicked,
    Failed,
    Undelivered,
    Bounced,
    Cancelled
}
