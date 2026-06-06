namespace Fgs.Notification.Infrastructure.Options;

public sealed class SetupServiceClientOptions
{
    public const string SectionName = "SetupService";

    public string BaseUrl { get; set; } = "http://setup-service:5004";

    public string InternalServiceKey { get; set; } = string.Empty;
}
