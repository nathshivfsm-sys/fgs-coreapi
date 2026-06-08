namespace Fgs.Credentials.Options;

public sealed class CredentialDistributionOptions
{
    public const string SectionName = "CredentialDistribution";

    public string InternalServiceKey { get; set; } = string.Empty;
}
