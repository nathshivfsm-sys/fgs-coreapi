namespace Fgs.Platform.Domain.Notifications;

public static class NotificationChannelExtensions
{
    public static string ToCommunicationTemplateType(this NotificationChannel channel) =>
        channel switch
        {
            NotificationChannel.Email => CommunicationTemplateTypes.Email,
            NotificationChannel.Sms => CommunicationTemplateTypes.Sms,
            NotificationChannel.Push => CommunicationTemplateTypes.Push,
            _ => throw new ArgumentOutOfRangeException(nameof(channel), channel, "Unsupported notification channel.")
        };
}
