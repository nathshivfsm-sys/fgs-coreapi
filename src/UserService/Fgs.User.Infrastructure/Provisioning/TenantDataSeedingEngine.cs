using System.Data;
using System.Data.Common;
using System.Text;
using Fgs.User.Application.Abstractions.Provisioning;
using Fgs.User.Application.TenantProvisioning;
using Fgs.User.Infrastructure.Persistence.Database.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Fgs.User.Infrastructure.Provisioning;

public sealed class TenantDataSeedingEngine(
    FgsUserDbContext dbContext,
    ILogger<TenantDataSeedingEngine> logger) : ITenantDataSeedingEngine
{
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
        var connection = dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
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
                string.Equals(c.TargetColumnName, SeedTransformationTypes.TargetColumns.TenantId, StringComparison.OrdinalIgnoreCase)
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

            var sql = BuildInsertSelectSql(mapping, columns);
            logger.LogInformation(
                "Seeding {SeedCode}: {SourceSchema}.{SourceTable} -> {TargetSchema}.{TargetTable} for tenant {TenantId}",
                mapping.SeedCode,
                mapping.SourceSchemaName,
                mapping.SourceTableName,
                mapping.TargetSchemaName,
                mapping.TargetTableName,
                tenantId);

            await using var command = connection.CreateCommand();
            command.Transaction = transaction;
            command.CommandText = sql;
            AddParameter(command, SeedTransformationTypes.SqlParameters.TenantId, tenantId);
            AddParameter(command, SeedTransformationTypes.SqlParameters.CompanyId, companyId);
            var inserted = await command.ExecuteNonQueryAsync(cancellationToken);
            logger.LogInformation(
                "Seed {SeedCode} inserted {RowCount} row(s) for tenant {TenantId}",
                mapping.SeedCode,
                inserted,
                tenantId);
        }

        await transaction.CommitAsync(cancellationToken);

        await SeedInventorySubCategoriesAsync(tenantId, companyId, cancellationToken);
    }

    private async Task SeedInventorySubCategoriesAsync(
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var alreadySeeded = await dbContext.FgsInventorySubCategories
            .AsNoTracking()
            .AnyAsync(sc => sc.TenantId == tenantId && sc.CompanyId == companyId, cancellationToken);

        if (alreadySeeded)
        {
            return;
        }

        var sql = """
            INSERT INTO dbo."FgsInventorySubCategory"
            (
                "TenantId",
                "CompanyId",
                "InventoryCategoryId",
                "SubCategoryCode",
                "Name",
                "Description",
                "DisplayOrder",
                "IsSystem",
                "IsActive",
                "CreatedOn",
                "CreatedBy"
            )
            SELECT
                @tenantId,
                @companyId,
                fc."Id",
                glo."SubCategoryCode",
                glo."Name",
                glo."Description",
                glo."DisplayOrder",
                true,
                glo."IsActive",
                NOW(),
                @seedCreatedBy
            FROM dbo."GloInventorySubCategory" glo
            INNER JOIN dbo."GloInventoryCategory" gc ON gc."Id" = glo."InventoryCategoryId"
            INNER JOIN dbo."FgsInventoryCategory" fc
                ON fc."TenantId" = @tenantId
               AND fc."CompanyId" = @companyId
               AND fc."CategoryCode" = gc."CategoryCode"
            WHERE NOT EXISTS (
                SELECT 1
                FROM dbo."FgsInventorySubCategory" existing
                WHERE existing."TenantId" = @tenantId
                  AND existing."CompanyId" = @companyId
                  AND existing."InventoryCategoryId" = fc."Id"
                  AND existing."SubCategoryCode" = glo."SubCategoryCode"
            )
            """;

        await dbContext.Database.ExecuteSqlRawAsync(
            sql,
            [
                new NpgsqlParameter("tenantId", tenantId),
                new NpgsqlParameter("companyId", companyId),
                new NpgsqlParameter("seedCreatedBy", SeedTransformationTypes.SeedCreatedByValue)
            ],
            cancellationToken);

        logger.LogInformation(
            "Seeded FgsInventorySubCategory rows for tenant {TenantId}, company {CompanyId}",
            tenantId,
            companyId);
    }

    private static async Task<bool> TargetAlreadySeededAsync(
        DbConnection connection,
        DbTransaction transaction,
        string targetSchema,
        string targetTable,
        long tenantId,
        CancellationToken cancellationToken)
    {
        var qualifiedTarget = QualifyTable(targetSchema, targetTable);
        var tenantColumn = QuoteIdentifier(SeedTransformationTypes.TargetColumns.TenantId);
        var checkSql = $"""
            SELECT EXISTS(
                SELECT 1 FROM {qualifiedTarget}
                WHERE {tenantColumn} = @{SeedTransformationTypes.SqlParameters.TenantId}
                LIMIT 1
            )
            """;

        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = checkSql;
        AddParameter(command, SeedTransformationTypes.SqlParameters.TenantId, tenantId);
        var exists = Convert.ToBoolean(await command.ExecuteScalarAsync(cancellationToken) ?? false);
        return exists;
    }

    private static string BuildInsertSelectSql(
        Domain.Entities.GloSeedTableMapping mapping,
        IReadOnlyList<Domain.Entities.GloSeedTableColumnMapping> columns)
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

    internal static string BuildSelectExpression(Domain.Entities.GloSeedTableColumnMapping column)
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

            return QuoteIdentifier(column.SourceColumnName);
        }

        return column.TransformationType switch
        {
            SeedTransformationTypes.TenantId => $"@{SeedTransformationTypes.SqlParameters.TenantId}",
            SeedTransformationTypes.CompanyId => $"@{SeedTransformationTypes.SqlParameters.CompanyId}",
            SeedTransformationTypes.Static => string.Equals(
                column.TargetColumnName,
                SeedTransformationTypes.TargetColumns.CreatedBy,
                StringComparison.OrdinalIgnoreCase)
                ? ToSqlLiteral(SeedTransformationTypes.SeedCreatedByValue)
                : ToSqlLiteral(column.StaticValue),
            SeedTransformationTypes.CurrentTimestamp => SeedTransformationTypes.SqlFunctions.CurrentTimestamp,
            SeedTransformationTypes.SeedCreatedBy => ToSqlLiteral(SeedTransformationTypes.SeedCreatedByValue),
            _ => throw new InvalidOperationException(
                string.Format(
                    SeedTransformationTypes.ErrorMessages.UnsupportedTransformationFormat,
                    column.TransformationType,
                    column.Id))
        };
    }

    private static string QualifyTable(string schema, string table) =>
        $"{QuoteIdentifier(schema)}.{QuoteIdentifier(table)}";

    internal static string QuoteIdentifier(string identifier) =>
        $"\"{identifier.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";

    internal static string ToSqlLiteral(string? value) =>
        value is null ? "NULL" : $"'{value.Replace("'", "''", StringComparison.Ordinal)}'";

    private static void AddParameter(DbCommand command, string name, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }
}
