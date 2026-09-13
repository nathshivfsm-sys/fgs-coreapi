namespace Fgs.Credentials.Options;

public sealed class CredentialConsumerOptions
{
    public const string SectionName = "CredentialConsumer";

    public string ServiceName { get; set; } = string.Empty;

    public string[] RequiredProviders { get; set; } = [];

    /// <summary>
    /// Safety-net poll of <c>fgs:credentials:snapshot</c> when Redis pub/sub is missed.
    /// Default 300s; set 0 to disable.
    /// </summary>
    public int SnapshotRefreshIntervalSeconds { get; set; } = 300;
}
