namespace Fgs.Notification.Infrastructure.Options;

public sealed class SetupServiceClientOptions
{
    public const string SectionName = "SetupService";

    public string BaseUrl { get; set; } = "http://localhost:5003";

    public string InternalServiceKey { get; set; } = string.Empty;
}
