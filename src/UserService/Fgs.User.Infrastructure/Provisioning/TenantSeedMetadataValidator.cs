using System.Data.Common;
using Fgs.User.Application.TenantProvisioning;
using Fgs.User.Domain.Entities;

namespace Fgs.User.Infrastructure.Provisioning;

internal sealed class TenantSeedMetadataValidator
{
    private readonly Dictionary<(string Database, string Schema, string Table), TableMetadata> _cache = new();

    public async Task<TableMetadata> GetTableMetadataAsync(
        DbConnection connection,
        string databaseName,
        string schemaName,
        string tableName,
        CancellationToken cancellationToken) =>
        await LoadTableMetadataAsync(connection, databaseName, schemaName, tableName, cancellationToken);

    public async Task<ValidationResult> ValidateMappingAsync(
        TenantSeedConnectionScope connectionScope,
        GloSeedTableMapping mapping,
        IReadOnlyList<GloSeedTableColumnMapping> columns,
        CancellationToken cancellationToken)
    {
        if (columns.Count == 0)
        {
            return ValidationResult.Fail(
                string.Format(SeedTransformationTypes.ErrorMessages.NoColumnMappingsFormat, mapping.SeedCode));
        }

        var duplicateTarget = columns
            .GroupBy(c => c.TargetColumnName, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault(g => g.Count() > 1);

        if (duplicateTarget is not null)
        {
            return ValidationResult.Fail(
                string.Format(
                    SeedTransformationTypes.ErrorMessages.DuplicateTargetColumnFormat,
                    mapping.SeedCode,
                    duplicateTarget.Key));
        }

        var sourceDatabase = connectionScope.ResolveSourceDatabaseName(mapping.SourceDatabaseName);
        var targetDatabase = connectionScope.ResolveTargetDatabaseName(mapping.TargetDatabaseName);

        DbConnection sourceConnection;
        DbConnection targetConnection;
        try
        {
            (sourceConnection, targetConnection) = await connectionScope.GetSourceAndTargetConnectionsAsync(
                mapping.SourceDatabaseName,
                mapping.TargetDatabaseName,
                cancellationToken);
        }
        catch (Exception ex)
        {
            return ValidationResult.Fail(
                string.Format(
                    SeedTransformationTypes.ErrorMessages.DatabaseConnectionFailedFormat,
                    sourceDatabase,
                    mapping.SeedCode,
                    ex.Message));
        }

        var sourceQualified = $"{sourceDatabase}.{mapping.SourceSchemaName}.{mapping.SourceTableName}";
        var targetQualified = $"{targetDatabase}.{mapping.TargetSchemaName}.{mapping.TargetTableName}";
        var sourceTableQualified = TenantSeedSqlBuilder.QualifyTable(mapping.SourceSchemaName, mapping.SourceTableName);
        var targetTableQualified = TenantSeedSqlBuilder.QualifyTable(mapping.TargetSchemaName, mapping.TargetTableName);

        var sourceMetadata = await LoadTableMetadataAsync(
            sourceConnection,
            sourceDatabase,
            mapping.SourceSchemaName,
            mapping.SourceTableName,
            cancellationToken);

        if (!sourceMetadata.Exists)
        {
            return ValidationResult.Fail(
                string.Format(
                    SeedTransformationTypes.ErrorMessages.SourceTableNotFoundFormat,
                    sourceQualified,
                    mapping.SeedCode,
                    mapping.Id));
        }

        var targetMetadata = await LoadTableMetadataAsync(
            targetConnection,
            targetDatabase,
            mapping.TargetSchemaName,
            mapping.TargetTableName,
            cancellationToken);

        if (!targetMetadata.Exists)
        {
            return ValidationResult.Fail(
                string.Format(
                    SeedTransformationTypes.ErrorMessages.TargetTableNotFoundFormat,
                    targetQualified,
                    mapping.SeedCode,
                    mapping.Id));
        }

        foreach (var column in columns)
        {
            try
            {
                TenantSeedSqlBuilder.BuildSelectExpression(column);
            }
            catch (InvalidOperationException ex)
            {
                return ValidationResult.Fail(
                    $"Invalid column mapping configuration for {mapping.SeedCode}, mapping id {column.Id}: {ex.Message}");
            }

            if (!targetMetadata.Columns.ContainsKey(column.TargetColumnName))
            {
                return ValidationResult.Fail(
                    string.Format(
                        SeedTransformationTypes.ErrorMessages.TargetColumnNotFoundFormat,
                        column.TargetColumnName,
                        targetTableQualified,
                        column.Id,
                        mapping.SeedCode));
            }

            if (string.IsNullOrWhiteSpace(column.TransformationType)
                && !string.IsNullOrWhiteSpace(column.SourceColumnName)
                && !sourceMetadata.Columns.ContainsKey(column.SourceColumnName))
            {
                return ValidationResult.Fail(
                    string.Format(
                        SeedTransformationTypes.ErrorMessages.SourceColumnNotFoundFormat,
                        column.SourceColumnName,
                        sourceTableQualified,
                        column.Id,
                        mapping.SeedCode));
            }
        }

        return ValidationResult.Ok(sourceMetadata, targetMetadata, sourceDatabase, targetDatabase);
    }

    private async Task<TableMetadata> LoadTableMetadataAsync(
        DbConnection connection,
        string databaseName,
        string schemaName,
        string tableName,
        CancellationToken cancellationToken)
    {
        var key = (databaseName, schemaName, tableName);
        if (_cache.TryGetValue(key, out var cached))
        {
            return cached;
        }

        const string sql = """
            SELECT
                a.attname AS column_name,
                NOT a.attnotnull AS is_nullable
            FROM pg_catalog.pg_attribute a
            INNER JOIN pg_catalog.pg_class c ON c.oid = a.attrelid
            INNER JOIN pg_catalog.pg_namespace n ON n.oid = c.relnamespace
            WHERE n.nspname = @schema
              AND c.relname = @table
              AND a.attnum > 0
              AND NOT a.attisdropped
            """;

        var columns = new Dictionary<string, ColumnMetadata>(StringComparer.OrdinalIgnoreCase);

        await using var command = connection.CreateCommand();
        command.CommandText = sql;
        AddParameter(command, "schema", schemaName);
        AddParameter(command, "table", tableName);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            var columnName = reader.GetString(0);
            var isNullable = reader.GetBoolean(1);
            columns[columnName] = new ColumnMetadata(columnName, isNullable);
        }

        var metadata = new TableMetadata(columns.Count > 0, columns);
        _cache[key] = metadata;
        return metadata;
    }

    private static void AddParameter(DbCommand command, string name, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }

    internal sealed record ColumnMetadata(string Name, bool IsNullable);

    internal sealed record TableMetadata(bool Exists, IReadOnlyDictionary<string, ColumnMetadata> Columns)
    {
        public bool HasBusinessTypeId =>
            Columns.ContainsKey(SeedTransformationTypes.SourceColumns.BusinessTypeId);

        public bool BusinessTypeIdIsNullable =>
            Columns.TryGetValue(SeedTransformationTypes.SourceColumns.BusinessTypeId, out var column)
            && column.IsNullable;
    }

    internal sealed record ValidationResult(
        bool IsValid,
        string? ErrorMessage,
        TableMetadata? SourceMetadata,
        TableMetadata? TargetMetadata,
        string? SourceDatabaseName,
        string? TargetDatabaseName)
    {
        public static ValidationResult Ok(
            TableMetadata source,
            TableMetadata target,
            string sourceDatabase,
            string targetDatabase) =>
            new(true, null, source, target, sourceDatabase, targetDatabase);

        public static ValidationResult Fail(string message) =>
            new(false, message, null, null, null, null);
    }
}
