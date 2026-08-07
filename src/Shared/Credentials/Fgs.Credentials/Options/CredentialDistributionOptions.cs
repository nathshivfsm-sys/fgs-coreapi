namespace Fgs.Credentials.Options;

public sealed class CredentialDistributionOptions
{
    public const string SectionName = "CredentialDistribution";

    /// <summary>
    /// Primary shared internal service key (emitted by outbound S2S clients).
    /// </summary>
    public string InternalServiceKey { get; set; } = string.Empty;

    /// <summary>
    /// Optional additional keys accepted during rotation. Callers continue to send
    /// <see cref="InternalServiceKey"/>; validators accept the primary key or any entry here.
    /// </summary>
    public string[] AdditionalInternalServiceKeys { get; set; } = [];
}
