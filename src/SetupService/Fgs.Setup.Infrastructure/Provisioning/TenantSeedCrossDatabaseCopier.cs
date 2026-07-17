using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Fgs.Setup.Application.Features.TenantProvisioning;
using Fgs.Setup.Domain.Entities;
using Microsoft.Extensions.Logging;
using Npgsql;
using NpgsqlTypes;

namespace Fgs.Setup.Infrastructure.Provisioning;

internal static class TenantSeedCrossDatabaseCopier
{
    public static async Task<int> CopyAsync(
        DbConnection sourceConnection,
        DbConnection targetConnection,
        DbTransaction? targetTransaction,
        GloSeedTableMapping mapping,
        IReadOnlyList<GloSeedTableColumnMapping> columns,
        TenantSeedMetadataValidator.TableMetadata sourceMetadata,
        long tenantId,
        long companyId,
        int[] businessTypeIds,
        bool hasBusinessTypeFilter,
        int batchSize,
        ILogger logger,
        CancellationToken cancellationToken)
    {
        var orderedColumns = columns
            .OrderBy(c => c.ColumnOrder)
            .ThenBy(c => c.Id)
            .ToList();

        var sourceColumns = orderedColumns
            .Where(c => string.IsNullOrWhiteSpace(c.TransformationType)
                && !string.IsNullOrWhiteSpace(c.SourceColumnName))
            .Select(c => c.SourceColumnName!)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var qualifiedSource = TenantSeedSqlBuilder.QualifyTable(
            mapping.SourceSchemaName,
            mapping.SourceTableName);

        var selectColumns = string.Join(
            ", ",
            sourceColumns.Select(TenantSeedSqlBuilder.QuoteIdentifier));

        var whereClause = TenantSeedSqlBuilder.CombineWhereClauses(
            TenantSeedSqlBuilder.BuildTenantScopeFilterClause(
                mapping,
                orderedColumns,
                sourceMetadata.Columns.ContainsKey(SeedTransformationTypes.TargetColumns.TenantId)),
            TenantSeedSqlBuilder.BuildBusinessTypeFilterClause(
                sourceMetadata.HasBusinessTypeId,
                sourceMetadata.BusinessTypeIdIsNullable,
                hasBusinessTypeFilter),
            TenantSeedSqlBuilder.BuildSourceFilterClause(mapping));

        var selectSql = string.IsNullOrWhiteSpace(whereClause)
            ? $"SELECT {selectColumns} FROM {qualifiedSource}"
            : $"SELECT {selectColumns} FROM {qualifiedSource} WHERE {whereClause}";

        var sourceRows = new List<IReadOnlyDictionary<string, object?>>();
        await using (var selectCommand = sourceConnection.CreateCommand())
        {
            selectCommand.CommandText = selectSql;
            TenantSeedCommandExtensions.AddSeedParameters(selectCommand, tenantId, companyId, businessTypeIds);

            await using var reader = await selectCommand.ExecuteReaderAsync(cancellationToken);
            var ordinals = sourceColumns.ToDictionary(
                c => c,
                c => reader.GetOrdinal(c),
                StringComparer.OrdinalIgnoreCase);

            while (await reader.ReadAsync(cancellationToken))
            {
                var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
                foreach (var sourceColumn in sourceColumns)
                {
                    var ordinal = ordinals[sourceColumn];
                    row[sourceColumn] = await reader.IsDBNullAsync(ordinal, cancellationToken)
                        ? null
                        : reader.GetValue(ordinal);
                }

                sourceRows.Add(row);
            }
        }

        if (sourceRows.Count == 0)
        {
            logger.LogInformation(
                "Cross-database seed {SeedCode} found no source rows in {SourceDatabase}.{SourceSchema}.{SourceTable}",
                mapping.SeedCode,
                mapping.SourceDatabaseName,
                mapping.SourceSchemaName,
                mapping.SourceTableName);
            return 0;
        }

        var qualifiedTarget = TenantSeedSqlBuilder.QualifyTable(
            mapping.TargetSchemaName,
            mapping.TargetTableName);

        var inserted = 0;
        foreach (var batch in sourceRows.Chunk(Math.Max(1, batchSize)))
        {
            inserted += await InsertBatchAsync(
                targetConnection,
                targetTransaction,
                qualifiedTarget,
                orderedColumns,
                batch,
                tenantId,
                companyId,
                cancellationToken);
        }

        return inserted;
    }

    private static async Task<int> InsertBatchAsync(
        DbConnection targetConnection,
        DbTransaction? targetTransaction,
        string qualifiedTarget,
        IReadOnlyList<GloSeedTableColumnMapping> orderedColumns,
        IReadOnlyList<IReadOnlyDictionary<string, object?>> batch,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        if (batch.Count == 0)
        {
            return 0;
        }

        var targetColumnNames = orderedColumns
            .Select(c => TenantSeedSqlBuilder.QuoteIdentifier(c.TargetColumnName))
            .ToList();

        var sql = new StringBuilder();
        sql.Append("INSERT INTO ").Append(qualifiedTarget).Append(" (");
        sql.AppendJoin(", ", targetColumnNames);
        sql.Append(") VALUES ");

        await using var command = targetConnection.CreateCommand();
        command.Transaction = targetTransaction;
        var valueClauses = new List<string>(batch.Count);
        var parameterIndex = 0;

        for (var rowIndex = 0; rowIndex < batch.Count; rowIndex++)
        {
            var row = batch[rowIndex];
            var parameterNames = new List<string>(orderedColumns.Count);

            foreach (var column in orderedColumns)
            {
                var parameterName = $"p{parameterIndex++}";
                parameterNames.Add("@" + parameterName);
                AddParameter(command, parameterName, ResolveInsertValue(column, row, tenantId, companyId));
            }

            valueClauses.Add($"({string.Join(", ", parameterNames)})");
        }

        sql.AppendJoin(", ", valueClauses);
        command.CommandText = sql.ToString();

        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private static object? ResolveInsertValue(
        GloSeedTableColumnMapping column,
        IReadOnlyDictionary<string, object?> sourceRow,
        long tenantId,
        long companyId)
    {
        if (string.IsNullOrWhiteSpace(column.TransformationType))
        {
            if (string.IsNullOrWhiteSpace(column.SourceColumnName))
            {
                throw new InvalidOperationException(
                    string.Format(
                        SeedTransformationTypes.ErrorMessages.SourceColumnRequiredFormat,
                        column.Id));
            }

            return sourceRow.TryGetValue(column.SourceColumnName, out var value) ? value : null;
        }

        return column.TransformationType switch
        {
            SeedTransformationTypes.TenantId => tenantId,
            SeedTransformationTypes.CompanyId => companyId,
            SeedTransformationTypes.Static => string.Equals(
                column.TargetColumnName,
                SeedTransformationTypes.TargetColumns.CreatedBy,
                StringComparison.OrdinalIgnoreCase)
                ? SeedTransformationTypes.SeedCreatedByValue
                : ParseStaticValue(column.StaticValue),
            SeedTransformationTypes.CurrentTimestamp => DateTimeOffset.UtcNow,
            SeedTransformationTypes.SeedCreatedBy => SeedTransformationTypes.SeedCreatedByValue,
            _ => throw new InvalidOperationException(
                string.Format(
                    SeedTransformationTypes.ErrorMessages.UnsupportedTransformationFormat,
                    column.TransformationType,
                    column.Id))
        };
    }

    private static object? ParseStaticValue(string? staticValue)
    {
        if (staticValue is null)
        {
            return null;
        }

        if (bool.TryParse(staticValue, out var boolValue))
        {
            return boolValue;
        }

        if (short.TryParse(staticValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var shortValue))
        {
            return shortValue;
        }

        if (int.TryParse(staticValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
        {
            return intValue;
        }

        if (long.TryParse(staticValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
        {
            return longValue;
        }

        if (decimal.TryParse(staticValue, NumberStyles.Number, CultureInfo.InvariantCulture, out var decimalValue))
        {
            return decimalValue;
        }

        return staticValue;
    }

    private static void AddParameter(DbCommand command, string name, object? value)
    {
        if (command is NpgsqlCommand npgsqlCommand)
        {
            npgsqlCommand.Parameters.Add(new NpgsqlParameter(name, value ?? DBNull.Value));
            return;
        }

        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }
}

internal static class TenantSeedCommandExtensions
{
    public static void AddBusinessTypeIdsParameter(DbCommand command, int[] businessTypeIds)
    {
        if (command is NpgsqlCommand npgsqlCommand)
        {
            npgsqlCommand.Parameters.Add(new NpgsqlParameter(
                SeedTransformationTypes.SqlParameters.BusinessTypeIds,
                NpgsqlDbType.Array | NpgsqlDbType.Integer)
            {
                Value = businessTypeIds
            });
            return;
        }

        var parameter = command.CreateParameter();
        parameter.ParameterName = SeedTransformationTypes.SqlParameters.BusinessTypeIds;
        parameter.Value = businessTypeIds;
        command.Parameters.Add(parameter);
    }

    public static void AddSeedParameters(
        DbCommand command,
        long tenantId,
        long companyId,
        int[]? businessTypeIds = null)
    {
        if (command is NpgsqlCommand npgsqlCommand)
        {
            npgsqlCommand.Parameters.Add(new NpgsqlParameter(
                SeedTransformationTypes.SqlParameters.TenantId,
                tenantId));
            npgsqlCommand.Parameters.Add(new NpgsqlParameter(
                SeedTransformationTypes.SqlParameters.CompanyId,
                companyId));

            if (businessTypeIds is { Length: > 0 })
            {
                AddBusinessTypeIdsParameter(command, businessTypeIds);
            }

            return;
        }

        var tenantParameter = command.CreateParameter();
        tenantParameter.ParameterName = SeedTransformationTypes.SqlParameters.TenantId;
        tenantParameter.Value = tenantId;
        command.Parameters.Add(tenantParameter);

        var companyParameter = command.CreateParameter();
        companyParameter.ParameterName = SeedTransformationTypes.SqlParameters.CompanyId;
        companyParameter.Value = companyId;
        command.Parameters.Add(companyParameter);

        if (businessTypeIds is { Length: > 0 })
        {
            AddBusinessTypeIdsParameter(command, businessTypeIds);
        }
    }
}
