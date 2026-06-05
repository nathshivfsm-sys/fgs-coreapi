namespace Fgs.Publisher.Infrastructure.Options;

public sealed class OutboxSourcesOptions
{
    public const string SectionName = "OutboxSources";

    public List<OutboxSourceOptions> Sources { get; set; } = [];
}

public sealed class OutboxSourceOptions
{
    public string SourceKey { get; set; } = null!;

    public string ConnectionStringName { get; set; } = null!;

    public string Schema { get; set; } = null!;

    public string Table { get; set; } = null!;
}
