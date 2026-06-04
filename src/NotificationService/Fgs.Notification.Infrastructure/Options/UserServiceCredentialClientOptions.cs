namespace Fgs.Notification.Infrastructure.Options;

public sealed class UserServiceCredentialClientOptions
{
    public const string SectionName = "UserService";

    /// <summary>
    /// User Service base URL. Use <c>http://localhost:5001</c> when debugging locally;
    /// use <c>http://setup-service:5001</c> inside the Docker compose network.
    /// </summary>
    public string BaseUrl { get; set; } = "http://localhost:5001";

    public string InternalServiceKey { get; set; } = string.Empty;

    /// <summary>
    /// When false, Platform starts using appsettings if User Service is unreachable (typical for local dev).
    /// </summary>
    public bool RequiredOnStartup { get; set; } = true;

    public int StartupRetryCount { get; set; } = 5;

    public int StartupRetryDelaySeconds { get; set; } = 3;

    /// <summary>
    /// When startup load fails or returns no entries, retry on this interval until User Service is available.
    /// </summary>
    public int BackgroundRetryIntervalSeconds { get; set; } = 15;
}
