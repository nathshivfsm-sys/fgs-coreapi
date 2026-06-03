using Fgs.Notification.Infrastructure.Options;
namespace Fgs.Notification.Infrastructure.Options;

public sealed class SendGridOptions
{
    public const string SectionName = "SendGrid";

    public string ApiKey { get; set; } = string.Empty;

    public string FromAddress { get; set; } = "noreply@example.com";

    public string FromName { get; set; } = "FGS Platform";
}
