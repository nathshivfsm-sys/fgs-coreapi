using Fgs.Notification.Infrastructure.Options;
namespace Fgs.Notification.Infrastructure.Options;

public sealed class NotificationOptions
{
    public const string SectionName = "Notification";

    public string PlatformName { get; set; } = "FGS";

    public string SupportEmail { get; set; } = "support@fgs.example";

    public string CompanyName { get; set; } = "FGS";

    public int InvitationExpirationHours { get; set; } = 72;
}
