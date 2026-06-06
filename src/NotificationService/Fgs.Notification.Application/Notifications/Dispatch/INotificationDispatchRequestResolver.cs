using Fgs.Contracts.Requests;

namespace Fgs.Notification.Application.Notifications.Dispatch;

public interface INotificationDispatchRequestResolver
{
    NotificationDispatchResolveResult Resolve(DispatchNotificationRequest request);
}
