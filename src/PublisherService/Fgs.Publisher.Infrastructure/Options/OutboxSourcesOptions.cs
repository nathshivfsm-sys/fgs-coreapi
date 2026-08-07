namespace Fgs.Publisher.Infrastructure.Options;

public sealed class OutboxSourcesOptions
{
    public const string SectionName = "OutboxSources";

    public List<OutboxSourceOptions> Sources { get; set; } = [];

    /// <summary>Returns sources with <see cref="OutboxSourceOptions.Enabled"/> true (default).</summary>
    public IEnumerable<OutboxSourceOptions> GetEnabledSources() =>
        Sources.Where(source => source.Enabled);
}

public sealed class OutboxSourceOptions
{
    public string SourceKey { get; set; } = null!;

    /// <summary>Primary connection string name (service DB). Used when <see cref="OutboxConnectionStringName"/> is unset.</summary>
    public string ConnectionStringName { get; set; } = null!;

    /// <summary>
    /// Optional outbox-reader connection string name for least-privilege credentials.
    /// When null or empty, falls back to <see cref="ConnectionStringName"/>.
    /// </summary>
    public string? OutboxConnectionStringName { get; set; }

    public string Schema { get; set; } = null!;

    public string Table { get; set; } = null!;

    /// <summary>When false, the source is excluded from <c>CompositeOutboxStore</c>. Defaults to true.</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Resolves the connection string name used for outbox read/claim (outbox-only or primary).</summary>
    public string ResolveConnectionStringName() =>
        string.IsNullOrWhiteSpace(OutboxConnectionStringName)
            ? ConnectionStringName
            : OutboxConnectionStringName;
}
