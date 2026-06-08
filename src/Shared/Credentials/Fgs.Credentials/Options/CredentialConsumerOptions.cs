namespace Fgs.Credentials.Options;

public sealed class CredentialConsumerOptions
{
    public const string SectionName = "CredentialConsumer";

    public string ServiceName { get; set; } = string.Empty;

    public string[] RequiredProviders { get; set; } = [];
}
