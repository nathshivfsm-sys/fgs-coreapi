using System.Text;
using Fgs.User.Application.Abstractions.Provisioning;
using Fgs.User.Application.TenantProvisioning;
using Fgs.User.Infrastructure.Common.Options;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Fgs.User.Infrastructure.Provisioning;

public sealed class TenantDataSeedingEngine(
    FgsUserDbContext dbContext,
    IOptions<TenantProvisioningOptions> options,
    ILogger<TenantDataSeedingEngine> logger) : ITenantDataSeedingEngine
{
    private readonly TenantProvisioningOptions _options = options.Value;

    public async Task SeedTenantDataAsync(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken = default)
    {
        var mappings = await dbContext.GloSeedTableMappings
            .AsNoTracking()
            .Where(m => m.IsActive)
            .OrderBy(m => m.SeedOrder)
            .ThenBy(m => m.Id)
            .ToListAsync(cancellationToken);

        if (mappings.Count == 0)
        {
            logger.LogWarning("No active GloSeedTableMapping rows found; skipping tenant data seed for tenant {TenantId}", tenantId);
            return;
        }

        var columnMappings = await dbContext.GloSeedTableColumnMappings
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.SeedTableMappingId)
            .ThenBy(c => c.ColumnOrder)
            .ThenBy(c => c.Id)
            .ToListAsync(cancellationToken);

        // Do not dispose this connection — it is owned by the DbContext.
        var connection = (NpgsqlConnection)dbContext.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);

        foreach (var mapping in mappings)
        {
            var columns = columnMappings
                .Where(c => c.SeedTableMappingId == mapping.Id)
                .ToList();

            if (columns.Count == 0)
            {
                logger.LogWarning(
                    "Seed mapping {SeedCode} has no active column mappings; skipping",
                    mapping.SeedCode);
                continue;
            }

            var seedsTenantId = columns.Any(c =>
                string.Equals(c.TargetColumnName, "TenantId", StringComparison.OrdinalIgnoreCase)
                && string.Equals(c.TransformationType, SeedTransformationTypes.TenantId, StringComparison.Ordinal));

            if (seedsTenantId
                && await TargetAlreadySeededAsync(
                    connection,
                    transaction,
                    mapping.TargetSchemaName,
                    mapping.TargetTableName,
                    tenantId,
                    cancellationToken))
            {
                logger.LogInformation(
                    "Target {Schema}.{Table} already has rows for tenant {TenantId}; skipping {SeedCode}",
                    mapping.TargetSchemaName,
                    mapping.TargetTableName,
                    tenantId,
                    mapping.SeedCode);
                continue;
            }

            var sql = BuildInsertSelectSql(mapping, columns, tenantId, companyId);
            logger.LogInformation(
                "Seeding {SeedCode}: {SourceSchema}.{SourceTable} -> {TargetSchema}.{TargetTable} for tenant {TenantId}",
                mapping.SeedCode,
                mapping.SourceSchemaName,
                mapping.SourceTableName,
                mapping.TargetSchemaName,
                mapping.TargetTableName,
                tenantId);

            await using var command = new NpgsqlCommand(sql, connection, transaction);
            command.Parameters.AddWithValue("tenantId", tenantId);
            command.Parameters.AddWithValue("companyId", companyId);
            var inserted = await command.ExecuteNonQueryAsync(cancellationToken);
            logger.LogInformation(
                "Seed {SeedCode} inserted {RowCount} row(s) for tenant {TenantId}",
                mapping.SeedCode,
                inserted,
                tenantId);
        }

        await transaction.CommitAsync(cancellationToken);
    }

    private static async Task<bool> TargetAlreadySeededAsync(
        NpgsqlConnection connection,
        NpgsqlTransaction transaction,
        string targetSchema,
        string targetTable,
        long tenantId,
        CancellationToken cancellationToken)
    {
        var qualifiedTarget = QualifyTable(targetSchema, targetTable);
        var checkSql = $"""
            SELECT EXISTS(
                SELECT 1 FROM {qualifiedTarget}
                WHERE "TenantId" = @tenantId
                LIMIT 1
            )
            """;

        await using var command = new NpgsqlCommand(checkSql, connection, transaction);
        command.Parameters.AddWithValue("tenantId", tenantId);
        var exists = (bool)(await command.ExecuteScalarAsync(cancellationToken) ?? false);
        return exists;
    }

    private static string BuildInsertSelectSql(
        Domain.Entities.GloSeedTableMapping mapping,
        IReadOnlyList<Domain.Entities.GloSeedTableColumnMapping> columns,
        long tenantId,
        long companyId)
    {
        var target = QualifyTable(mapping.TargetSchemaName, mapping.TargetTableName);
        var source = QualifyTable(mapping.SourceSchemaName, mapping.SourceTableName);

        var targetColumns = new StringBuilder();
        var selectExpressions = new StringBuilder();

        for (var i = 0; i < columns.Count; i++)
        {
            var column = columns[i];
            if (i > 0)
            {
                targetColumns.Append(", ");
                selectExpressions.Append(", ");
            }

            targetColumns.Append(QuoteIdentifier(column.TargetColumnName));
            selectExpressions.Append(BuildSelectExpression(column));
        }

        return $"""
            INSERT INTO {target} ({targetColumns})
            SELECT {selectExpressions}
            FROM {source}
            """;
    }

    private static string BuildSelectExpression(Domain.Entities.GloSeedTableColumnMapping column)
    {
        if (string.IsNullOrWhiteSpace(column.TransformationType))
        {
            if (string.IsNullOrWhiteSpace(column.SourceColumnName))
            {
                throw new InvalidOperationException(
                    $"Column mapping {column.Id} requires SourceColumnName when TransformationType is null.");
            }

            return QuoteIdentifier(column.SourceColumnName);
        }

        return column.TransformationType switch
        {
            SeedTransformationTypes.TenantId => "@tenantId",
            SeedTransformationTypes.CompanyId => "@companyId",
            SeedTransformationTypes.Static => string.Equals(
                column.TargetColumnName,
                "CreatedBy",
                StringComparison.OrdinalIgnoreCase)
                ? ToSqlLiteral(SeedTransformationTypes.SeedCreatedByValue)
                : ToSqlLiteral(column.StaticValue),
            SeedTransformationTypes.CurrentTimestamp => "NOW()",
            SeedTransformationTypes.SeedCreatedBy => ToSqlLiteral(SeedTransformationTypes.SeedCreatedByValue),
            _ => throw new InvalidOperationException(
                $"Unsupported transformation type '{column.TransformationType}' on column mapping {column.Id}.")
        };
    }

    private static string QualifyTable(string schema, string table) =>
        $"{QuoteIdentifier(schema)}.{QuoteIdentifier(table)}";

    private static string QuoteIdentifier(string identifier) =>
        $"\"{identifier.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";

    private static string ToSqlLiteral(string? value) =>
        value is null ? "NULL" : $"'{value.Replace("'", "''", StringComparison.Ordinal)}'";
}
