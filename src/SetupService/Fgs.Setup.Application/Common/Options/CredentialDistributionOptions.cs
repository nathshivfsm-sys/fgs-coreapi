namespace Fgs.Setup.Application.Common.Options;

/// <summary>
/// Service-to-service credential distribution settings (e.g. Platform Service fetching resolved config).
/// </summary>
public sealed class CredentialDistributionOptions
{
    public const string SectionName = "CredentialDistribution";

    public string InternalServiceKey { get; set; } = string.Empty;
}
