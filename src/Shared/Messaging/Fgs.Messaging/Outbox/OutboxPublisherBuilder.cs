using Fgs.Messaging.Abstractions;

namespace Fgs.Messaging.Outbox;

public sealed class OutboxPublisherBuilder
{
    private readonly List<OutboxSourceRegistration> _sources = [];
    private Type? _customPublisherType;

    public string? ClientProvidedName { get; set; }

    public bool AutomaticRecoveryEnabled { get; set; } = true;

    /// <summary>
    /// When set, registers <typeparamref name="TPublisher"/> as <see cref="IIntegrationEventPublisher"/>
    /// and skips the default RabbitMQ adapter.
    /// </summary>
    public OutboxPublisherBuilder UsePublisher<TPublisher>()
        where TPublisher : class, IIntegrationEventPublisher
    {
        _customPublisherType = typeof(TPublisher);
        return this;
    }

    public OutboxPublisherBuilder AddSource(
        string sourceKey,
        string schema,
        string table,
        Func<IServiceProvider, string> connectionStringFactory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceKey);
        ArgumentException.ThrowIfNullOrWhiteSpace(schema);
        ArgumentException.ThrowIfNullOrWhiteSpace(table);
        ArgumentNullException.ThrowIfNull(connectionStringFactory);

        _sources.Add(new OutboxSourceRegistration(sourceKey, schema, table, connectionStringFactory));
        return this;
    }

    internal IReadOnlyList<OutboxSourceRegistration> Sources => _sources;

    internal Type? CustomPublisherType => _customPublisherType;
}

internal sealed record OutboxSourceRegistration(
    string SourceKey,
    string Schema,
    string Table,
    Func<IServiceProvider, string> ConnectionStringFactory);
