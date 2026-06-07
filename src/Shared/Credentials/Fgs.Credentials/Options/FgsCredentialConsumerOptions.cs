namespace Fgs.Credentials.Options;

public sealed class FgsCredentialConsumerOptions
{
    public string ServiceName { get; set; } = string.Empty;

    public string[] RequiredProviders { get; set; } = [];

    public bool RegisterSetupClient { get; set; } = true;

    public string SetupBaseUrlKey { get; set; } = "SetupService:BaseUrl";

    public string SetupDefaultBaseUrl { get; set; } = "http://setup-service:5004";
}
