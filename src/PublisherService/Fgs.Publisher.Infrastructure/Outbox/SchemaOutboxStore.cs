using System.Text.RegularExpressions;
using Fgs.Messaging.Models;
using Npgsql;

namespace Fgs.Publisher.Infrastructure.Outbox;

public sealed partial class SchemaOutboxStore : ISchemaOutboxSource
{
    private const string StatusPending = "Pending";
    private const string StatusProcessing = "Processing";
    private const string StatusPublished = "Published";
    private const string StatusRetry = "Retry";
    private const string StatusFailed = "Failed";
    private static readonly TimeSpan StaleProcessingThreshold = TimeSpan.FromMinutes(2);

    private readonly Func<string> _connectionStringFactory;
    private readonly string _qualifiedTable;

    public SchemaOutboxStore(
        string sourceKey,
        Func<string> connectionStringFactory,
        string schema,
        string table)
    {
        SourceKey = sourceKey;
        _connectionStringFactory = connectionStringFactory
            ?? throw new ArgumentNullException(nameof(connectionStringFactory));
        ValidateIdentifier(schema, nameof(schema));
        ValidateIdentifier(table, nameof(table));
        _qualifiedTable = $"{schema}.\"{table}\"";
    }

    /// <summary>Backward-compatible constructor for tests and callers with a fixed connection string.</summary>
    public SchemaOutboxStore(
        string sourceKey,
        string connectionString,
        string schema,
        string table)
        : this(sourceKey, () => connectionString, schema, table)
    {
    }

    public string SourceKey { get; }

    public async Task<IReadOnlyList<ClaimedOutboxRow>> ClaimPendingBatchAsync(
        int batchSize,
        CancellationToken cancellationToken)
    {
        if (batchSize <= 0)
        {
            return [];
        }

        var now = DateTimeOffset.UtcNow;
        var staleBefore = now.Subtract(StaleProcessingThreshold);

        await using var connection = new NpgsqlConnection(_connectionStringFactory());
        await connection.OpenAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        const string reclaimSql = """
            UPDATE {0}
            SET "Status" = @pendingStatus
            WHERE "Status" = @processingStatus
                AND COALESCE("UpdatedOn", "CreatedOn") < @staleBefore
            """;

        await using (var reclaimCommand = new NpgsqlCommand(
            string.Format(reclaimSql, _qualifiedTable),
            connection,
            transaction))
        {
            reclaimCommand.Parameters.AddWithValue("pendingStatus", StatusPending);
            reclaimCommand.Parameters.AddWithValue("processingStatus", StatusProcessing);
            reclaimCommand.Parameters.AddWithValue("staleBefore", staleBefore);
            await reclaimCommand.ExecuteNonQueryAsync(cancellationToken);
        }

        var claimSql = $"""
            UPDATE {_qualifiedTable} AS outbox
            SET "Status" = @processingStatus,
                "UpdatedOn" = @now
            FROM (
                SELECT "Id"
                FROM {_qualifiedTable}
                WHERE "Status" IN (@pendingStatus, @retryStatus)
                    AND ("NextRetryOn" IS NULL OR "NextRetryOn" <= @now)
                    AND "RetryCount" < "MaxRetryCount"
                ORDER BY "CreatedOn"
                LIMIT @batchSize
                FOR UPDATE SKIP LOCKED
            ) AS pending
            WHERE outbox."Id" = pending."Id"
            RETURNING outbox."Id",
                      outbox."EventType",
                      outbox."Payload",
                      outbox."CorrelationId",
                      outbox."ExchangeName",
                      outbox."RoutingKey",
                      outbox."RetryCount",
                      outbox."MaxRetryCount",
                      outbox."CreatedOn"
            """;

        await using var command = new NpgsqlCommand(claimSql, connection, transaction);
        command.Parameters.AddWithValue("processingStatus", StatusProcessing);
        command.Parameters.AddWithValue("pendingStatus", StatusPending);
        command.Parameters.AddWithValue("retryStatus", StatusRetry);
        command.Parameters.AddWithValue("now", now);
        command.Parameters.AddWithValue("batchSize", batchSize);

        var rows = new List<ClaimedOutboxRow>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var message = new PendingOutboxMessage(
                SourceKey,
                reader.GetInt64(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetGuid(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.IsDBNull(5) ? null : reader.GetString(5),
                reader.GetInt32(6),
                reader.GetInt32(7));

            rows.Add(new ClaimedOutboxRow(message, reader.GetFieldValue<DateTimeOffset>(8)));
        }

        await reader.CloseAsync();
        await transaction.CommitAsync(cancellationToken);
        return rows;
    }

    public async Task MarkPublishedAsync(
        long messageId,
        DateTimeOffset processedOn,
        CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE {0}
            SET "Status" = @status,
                "ProcessedOn" = @processedOn,
                "LastError" = NULL,
                "NextRetryOn" = NULL
            WHERE "Id" = @id
            """;

        await ExecuteNonQueryAsync(
            string.Format(sql, _qualifiedTable),
            cancellationToken,
            new NpgsqlParameter("status", StatusPublished),
            new NpgsqlParameter("processedOn", processedOn),
            new NpgsqlParameter("id", messageId));
    }

    public async Task MarkRetryOrFailedAsync(
        long messageId,
        int retryCount,
        string lastError,
        bool isFailed,
        DateTimeOffset? nextRetryOn,
        CancellationToken cancellationToken)
    {
        const string sql = """
            UPDATE {0}
            SET "Status" = @status,
                "RetryCount" = @retryCount,
                "LastError" = @lastError,
                "NextRetryOn" = @nextRetryOn
            WHERE "Id" = @id
            """;

        await ExecuteNonQueryAsync(
            string.Format(sql, _qualifiedTable),
            cancellationToken,
            new NpgsqlParameter("status", isFailed ? StatusFailed : StatusRetry),
            new NpgsqlParameter("retryCount", retryCount),
            new NpgsqlParameter("lastError", lastError),
            new NpgsqlParameter("nextRetryOn", (object?)nextRetryOn ?? DBNull.Value),
            new NpgsqlParameter("id", messageId));
    }

    private async Task ExecuteNonQueryAsync(
        string sql,
        CancellationToken cancellationToken,
        params NpgsqlParameter[] parameters)
    {
        await using var connection = new NpgsqlConnection(_connectionStringFactory());
        await connection.OpenAsync(cancellationToken);
        await using var command = new NpgsqlCommand(sql, connection);
        command.Parameters.AddRange(parameters);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static void ValidateIdentifier(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value) || !IdentifierPattern().IsMatch(value))
        {
            throw new ArgumentException($"Invalid SQL identifier: {value}", paramName);
        }
    }

    [GeneratedRegex("^[A-Za-z_][A-Za-z0-9_]*$")]
    private static partial Regex IdentifierPattern();
}
