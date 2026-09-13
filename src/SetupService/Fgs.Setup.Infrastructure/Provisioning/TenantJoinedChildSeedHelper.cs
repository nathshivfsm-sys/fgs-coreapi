using System.Data.Common;
using Fgs.Setup.Application.Features.TenantProvisioning;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Database.Schemas;

namespace Fgs.Setup.Infrastructure.Provisioning;

internal static class TenantJoinedChildSeedHelper
{
    internal sealed record JoinedSeedConnections(
        DbConnection SourceConnection,
        DbConnection TargetConnection,
        string SourceSchema,
        string TargetSchema,
        string? SourceDatabaseName,
        string? TargetDatabaseName,
        bool IsCrossDatabase);

    internal static async Task<JoinedSeedConnections> ResolveConnectionsAsync(
        TenantSeedConnectionScope connectionScope,
        IReadOnlyList<GloSeedTableMapping> mappings,
        string targetSchema,
        CancellationToken cancellationToken)
    {
        var referenceMapping = SelectReferenceMapping(mappings, targetSchema);
        var sourceSchema = referenceMapping?.SourceSchemaName ?? FgsDatabaseSchemas.Glo;
        var sourceDatabaseName = referenceMapping?.SourceDatabaseName;
        var targetDatabaseName = referenceMapping?.TargetDatabaseName;

        var (sourceConnection, targetConnection) = await connectionScope.GetSourceAndTargetConnectionsAsync(
            sourceDatabaseName,
            targetDatabaseName,
            cancellationToken);

        return new JoinedSeedConnections(
            sourceConnection,
            targetConnection,
            sourceSchema,
            targetSchema,
            sourceDatabaseName,
            targetDatabaseName,
            connectionScope.IsCrossDatabase(sourceDatabaseName, targetDatabaseName));
    }

    /// <summary>
    /// Picks a glo→target mapping for soft-path seeds. Cache mappings (SeedOrder 1+) use
    /// <c>tenant</c> as source and must not drive catalog joins.
    /// </summary>
    internal static GloSeedTableMapping? SelectReferenceMapping(
        IReadOnlyList<GloSeedTableMapping> mappings,
        string targetSchema)
    {
        if (mappings.Count == 0)
        {
            return null;
        }

        var gloToTarget = mappings.FirstOrDefault(m =>
            string.Equals(m.SourceSchemaName, FgsDatabaseSchemas.Glo, StringComparison.OrdinalIgnoreCase)
            && string.Equals(m.TargetSchemaName, targetSchema, StringComparison.OrdinalIgnoreCase));
        if (gloToTarget is not null)
        {
            return gloToTarget;
        }

        return mappings.FirstOrDefault(m =>
            string.Equals(m.SourceSchemaName, FgsDatabaseSchemas.Glo, StringComparison.OrdinalIgnoreCase));
    }

    internal static async Task<bool> IsTenantCompanySeededAsync(
        DbConnection connection,
        string targetSchema,
        string targetTable,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var table = TenantSeedSqlBuilder.QualifyTable(targetSchema, targetTable);
        await using var command = connection.CreateCommand();
        command.CommandText = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {table}
                WHERE "TenantId" = @TenantId AND "CompanyId" = @CompanyId
            )
            """;
        AddParameter(command, "TenantId", tenantId);
        AddParameter(command, "CompanyId", companyId);
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result is true or 1 or 1L;
    }

    internal static async Task<T> ExecuteInTargetTransactionAsync<T>(
        DbConnection targetConnection,
        Func<DbTransaction, CancellationToken, Task<T>> seedAsync,
        CancellationToken cancellationToken)
    {
        await using var transaction = await targetConnection.BeginTransactionAsync(cancellationToken);
        try
        {
            var result = await seedAsync(transaction, cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return result;
        }
        catch
        {
            await TryRollbackAsync(transaction, cancellationToken);
            throw;
        }
    }

    internal static async Task<int> ExecuteInTargetTransactionAsync(
        DbConnection targetConnection,
        Func<DbTransaction, CancellationToken, Task<int>> seedAsync,
        CancellationToken cancellationToken) =>
        await ExecuteInTargetTransactionAsync<int>(targetConnection, seedAsync, cancellationToken);

    internal static async Task<int> SeedUniversalMatrixChildTableSameDatabaseAsync(
        DbConnection connection,
        DbTransaction transaction,
        string sourceSchema,
        string targetSchema,
        string gloChildTableName,
        string fgsChildTableName,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var gloChildTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, gloChildTableName);
        var gloServiceTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, "GloUniversalPricingService");
        var fgsServiceTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsUniversalPricingService");
        var fgsChildTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, fgsChildTableName);
        var gloChildAlias = gloChildTableName switch
        {
            "GloUniversalMatrixTier" => "gt",
            "GloUniversalMatrixSizeTier" => "gst",
            _ => throw new ArgumentException($"Unsupported glo child table '{gloChildTableName}'.", nameof(gloChildTableName)),
        };

        var sql = $"""
            INSERT INTO {fgsChildTable}
            (
                "TenantId",
                "CompanyId",
                "UniversalPricingServiceId",
                "Name",
                "Multiplier",
                "DisplayOrder",
                "IsActive",
                "CreatedOn",
                "CreatedBy"
            )
            SELECT
                @tenantId,
                @companyId,
                ups."Id",
                {gloChildAlias}."Name",
                {gloChildAlias}."Multiplier",
                {gloChildAlias}."DisplayOrder",
                true,
                NOW(),
                @seedCreatedBy
            FROM {gloChildTable} {gloChildAlias}
            INNER JOIN {gloServiceTable} gps ON gps."Id" = {gloChildAlias}."UniversalPricingServiceId"
            INNER JOIN {fgsServiceTable} ups
                ON ups."TenantId" = @tenantId
               AND ups."CompanyId" = @companyId
               AND ups."UniversalPricingServiceCode" = gps."ServiceCode"
            WHERE NOT EXISTS (
                SELECT 1
                FROM {fgsChildTable} existing
                WHERE existing."TenantId" = @tenantId
                  AND existing."CompanyId" = @companyId
                  AND existing."UniversalPricingServiceId" = ups."Id"
                  AND existing."Name" = {gloChildAlias}."Name"
            )
            """;

        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = sql;
        TenantSeedCommandExtensions.AddSeedParameters(command, tenantId, companyId);
        AddSeedCreatedByParameter(command);
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    /// <summary>
    /// Seeds <c>identity.FgsRolePermission</c> from <c>glo.GloRolePermission</c>, remapping
    /// RoleId → tenant <c>FgsRole.Id</c> by RoleCode and PermissionId → platform
    /// <c>FgsPermission.Id</c> by PermissionCode (permissions are not tenant-scoped).
    /// </summary>
    internal static async Task<int> SeedRolePermissionsSameDatabaseAsync(
        DbConnection connection,
        DbTransaction transaction,
        string sourceSchema,
        string targetSchema,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = BuildRolePermissionSameDatabaseSql(sourceSchema, targetSchema);
        TenantSeedCommandExtensions.AddSeedParameters(command, tenantId, companyId);
        AddSeedCreatedByParameter(command);
        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    internal static string BuildRolePermissionSameDatabaseSql(string sourceSchema, string targetSchema)
    {
        var gloRolePermissionTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, "GloRolePermission");
        var gloRoleTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, "GloRole");
        var gloPermissionTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, "GloPermission");
        var fgsRoleTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsRole");
        var fgsPermissionTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsPermission");
        var fgsRolePermissionTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsRolePermission");

        return $"""
            INSERT INTO {fgsRolePermissionTable}
            (
                "TenantId",
                "CompanyId",
                "FgsRoleId",
                "FgsPermissionId",
                "CreatedOn",
                "CreatedBy"
            )
            SELECT
                @tenantId,
                @companyId,
                fr."Id",
                fp."Id",
                NOW(),
                @seedCreatedBy
            FROM {gloRolePermissionTable} grp
            INNER JOIN {gloRoleTable} gr ON gr."Id" = grp."RoleId"
            INNER JOIN {gloPermissionTable} gp ON gp."Id" = grp."PermissionId"
            INNER JOIN {fgsRoleTable} fr
                ON fr."TenantId" = @tenantId
               AND fr."CompanyId" = @companyId
               AND fr."RoleCode" = gr."RoleCode"
            INNER JOIN {fgsPermissionTable} fp
                ON fp."PermissionCode" = gp."PermissionCode"
            WHERE grp."IsActive" = true
              AND NOT EXISTS (
                SELECT 1
                FROM {fgsRolePermissionTable} existing
                WHERE existing."TenantId" = @tenantId
                  AND existing."CompanyId" = @companyId
                  AND existing."FgsRoleId" = fr."Id"
                  AND existing."FgsPermissionId" = fp."Id"
            )
            """;
    }

    internal static async Task<int> SeedRolePermissionsCrossDatabaseAsync(
        DbConnection sourceConnection,
        DbConnection targetConnection,
        DbTransaction transaction,
        string sourceSchema,
        string targetSchema,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var sourceRows = await LoadRolePermissionSourceRowsAsync(
            sourceConnection,
            sourceSchema,
            cancellationToken);

        var roleIds = await LoadTenantRoleIdsByCodeAsync(
            targetConnection,
            transaction,
            targetSchema,
            tenantId,
            companyId,
            cancellationToken);

        var permissionIds = await LoadPlatformPermissionIdsByCodeAsync(
            targetConnection,
            transaction,
            targetSchema,
            cancellationToken);

        var existingKeys = await LoadRolePermissionExistingKeysAsync(
            targetConnection,
            transaction,
            targetSchema,
            tenantId,
            companyId,
            cancellationToken);

        var fgsRolePermissionTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsRolePermission");
        var inserted = 0;

        foreach (var row in sourceRows)
        {
            if (!roleIds.TryGetValue(row.RoleCode, out var fgsRoleId)
                || !permissionIds.TryGetValue(row.PermissionCode, out var fgsPermissionId))
            {
                continue;
            }

            var key = $"{fgsRoleId}:{fgsPermissionId}";
            if (existingKeys.Contains(key))
            {
                continue;
            }

            await using var insertCommand = targetConnection.CreateCommand();
            insertCommand.Transaction = transaction;
            insertCommand.CommandText = $"""
                INSERT INTO {fgsRolePermissionTable}
                (
                    "TenantId", "CompanyId", "FgsRoleId", "FgsPermissionId", "CreatedOn", "CreatedBy"
                )
                VALUES
                (
                    @tenantId, @companyId, @fgsRoleId, @fgsPermissionId, NOW(), @seedCreatedBy
                )
                """;
            TenantSeedCommandExtensions.AddSeedParameters(insertCommand, tenantId, companyId);
            AddSeedCreatedByParameter(insertCommand);
            AddParameter(insertCommand, "fgsRoleId", fgsRoleId);
            AddParameter(insertCommand, "fgsPermissionId", fgsPermissionId);

            inserted += await insertCommand.ExecuteNonQueryAsync(cancellationToken);
            existingKeys.Add(key);
        }

        return inserted;
    }

    private static async Task<List<RolePermissionSourceRow>> LoadRolePermissionSourceRowsAsync(
        DbConnection sourceConnection,
        string sourceSchema,
        CancellationToken cancellationToken)
    {
        var gloRolePermissionTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, "GloRolePermission");
        var gloRoleTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, "GloRole");
        var gloPermissionTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, "GloPermission");

        var sourceSql = $"""
            SELECT gr."RoleCode", gp."PermissionCode"
            FROM {gloRolePermissionTable} grp
            INNER JOIN {gloRoleTable} gr ON gr."Id" = grp."RoleId"
            INNER JOIN {gloPermissionTable} gp ON gp."Id" = grp."PermissionId"
            WHERE grp."IsActive" = true
            """;

        var rows = new List<RolePermissionSourceRow>();
        await using var sourceCommand = sourceConnection.CreateCommand();
        sourceCommand.CommandText = sourceSql;
        await using var reader = await sourceCommand.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            rows.Add(new RolePermissionSourceRow(reader.GetString(0), reader.GetString(1)));
        }

        return rows;
    }

    private static async Task<Dictionary<string, long>> LoadTenantRoleIdsByCodeAsync(
        DbConnection targetConnection,
        DbTransaction transaction,
        string targetSchema,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var fgsRoleTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsRole");
        var roleIds = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

        await using var command = targetConnection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = $"""
            SELECT "Id", "RoleCode"
            FROM {fgsRoleTable}
            WHERE "TenantId" = @tenantId AND "CompanyId" = @companyId
            """;
        TenantSeedCommandExtensions.AddSeedParameters(command, tenantId, companyId);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            roleIds[reader.GetString(1)] = reader.GetInt64(0);
        }

        return roleIds;
    }

    private static async Task<Dictionary<string, long>> LoadPlatformPermissionIdsByCodeAsync(
        DbConnection targetConnection,
        DbTransaction transaction,
        string targetSchema,
        CancellationToken cancellationToken)
    {
        var fgsPermissionTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsPermission");
        var permissionIds = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

        await using var command = targetConnection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = $"""
            SELECT "Id", "PermissionCode"
            FROM {fgsPermissionTable}
            """;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            permissionIds[reader.GetString(1)] = reader.GetInt64(0);
        }

        return permissionIds;
    }

    private static async Task<HashSet<string>> LoadRolePermissionExistingKeysAsync(
        DbConnection targetConnection,
        DbTransaction transaction,
        string targetSchema,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var fgsRolePermissionTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsRolePermission");
        var existingKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await using var command = targetConnection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = $"""
            SELECT "FgsRoleId", "FgsPermissionId"
            FROM {fgsRolePermissionTable}
            WHERE "TenantId" = @tenantId AND "CompanyId" = @companyId
            """;
        TenantSeedCommandExtensions.AddSeedParameters(command, tenantId, companyId);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            existingKeys.Add($"{reader.GetInt64(0)}:{reader.GetInt64(1)}");
        }

        return existingKeys;
    }

    internal static async Task<(int TierInserted, int SizeTierInserted)> SeedUniversalMatrixChildTablesCrossDatabaseAsync(
        DbConnection sourceConnection,
        DbConnection targetConnection,
        DbTransaction transaction,
        string sourceSchema,
        string targetSchema,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var sourceTiers = await LoadUniversalMatrixChildSourceRowsAsync(
            sourceConnection,
            sourceSchema,
            "GloUniversalMatrixTier",
            cancellationToken);
        var sourceSizeTiers = await LoadUniversalMatrixChildSourceRowsAsync(
            sourceConnection,
            sourceSchema,
            "GloUniversalMatrixSizeTier",
            cancellationToken);

        var serviceIds = await LoadUniversalPricingServiceIdsAsync(
            targetConnection,
            transaction,
            targetSchema,
            tenantId,
            companyId,
            cancellationToken);

        var existingTierKeys = await LoadUniversalMatrixChildExistingKeysAsync(
            targetConnection,
            transaction,
            targetSchema,
            "FgsUniversalMatrixTier",
            tenantId,
            companyId,
            cancellationToken);
        var existingSizeTierKeys = await LoadUniversalMatrixChildExistingKeysAsync(
            targetConnection,
            transaction,
            targetSchema,
            "FgsUniversalMatrixSizeTier",
            tenantId,
            companyId,
            cancellationToken);

        var tierInserted = await InsertUniversalMatrixChildRowsCrossDatabaseAsync(
            targetConnection,
            transaction,
            targetSchema,
            "FgsUniversalMatrixTier",
            sourceTiers,
            serviceIds,
            existingTierKeys,
            tenantId,
            companyId,
            cancellationToken);
        var sizeTierInserted = await InsertUniversalMatrixChildRowsCrossDatabaseAsync(
            targetConnection,
            transaction,
            targetSchema,
            "FgsUniversalMatrixSizeTier",
            sourceSizeTiers,
            serviceIds,
            existingSizeTierKeys,
            tenantId,
            companyId,
            cancellationToken);

        return (tierInserted, sizeTierInserted);
    }

    private static async Task<List<UniversalMatrixChildSourceRow>> LoadUniversalMatrixChildSourceRowsAsync(
        DbConnection sourceConnection,
        string sourceSchema,
        string gloChildTableName,
        CancellationToken cancellationToken)
    {
        var gloChildTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, gloChildTableName);
        var gloServiceTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, "GloUniversalPricingService");
        var gloChildAlias = gloChildTableName switch
        {
            "GloUniversalMatrixTier" => "gt",
            "GloUniversalMatrixSizeTier" => "gst",
            _ => throw new ArgumentException($"Unsupported glo child table '{gloChildTableName}'.", nameof(gloChildTableName)),
        };

        var sourceSql = $"""
            SELECT {gloChildAlias}."Name", {gloChildAlias}."Multiplier", {gloChildAlias}."DisplayOrder", gps."ServiceCode"
            FROM {gloChildTable} {gloChildAlias}
            INNER JOIN {gloServiceTable} gps ON gps."Id" = {gloChildAlias}."UniversalPricingServiceId"
            """;

        var rows = new List<UniversalMatrixChildSourceRow>();
        await using var sourceCommand = sourceConnection.CreateCommand();
        sourceCommand.CommandText = sourceSql;
        await using var reader = await sourceCommand.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            rows.Add(new UniversalMatrixChildSourceRow(
                reader.GetString(0),
                reader.GetDecimal(1),
                reader.GetInt16(2),
                reader.GetString(3)));
        }

        return rows;
    }

    private static async Task<Dictionary<string, long>> LoadUniversalPricingServiceIdsAsync(
        DbConnection targetConnection,
        DbTransaction transaction,
        string targetSchema,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var fgsServiceTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsUniversalPricingService");
        var serviceIds = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);

        await using var serviceCommand = targetConnection.CreateCommand();
        serviceCommand.Transaction = transaction;
        serviceCommand.CommandText = $"""
            SELECT "Id", "UniversalPricingServiceCode"
            FROM {fgsServiceTable}
            WHERE "TenantId" = @tenantId AND "CompanyId" = @companyId
            """;
        TenantSeedCommandExtensions.AddSeedParameters(serviceCommand, tenantId, companyId);

        await using var reader = await serviceCommand.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            serviceIds[reader.GetString(1)] = reader.GetInt64(0);
        }

        return serviceIds;
    }

    private static async Task<HashSet<string>> LoadUniversalMatrixChildExistingKeysAsync(
        DbConnection targetConnection,
        DbTransaction transaction,
        string targetSchema,
        string fgsChildTableName,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var fgsChildTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, fgsChildTableName);
        var existingKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await using var existingCommand = targetConnection.CreateCommand();
        existingCommand.Transaction = transaction;
        existingCommand.CommandText = $"""
            SELECT "UniversalPricingServiceId", "Name"
            FROM {fgsChildTable}
            WHERE "TenantId" = @tenantId AND "CompanyId" = @companyId
            """;
        TenantSeedCommandExtensions.AddSeedParameters(existingCommand, tenantId, companyId);

        await using var reader = await existingCommand.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            existingKeys.Add($"{reader.GetInt64(0)}:{reader.GetString(1)}");
        }

        return existingKeys;
    }

    private static async Task<int> InsertUniversalMatrixChildRowsCrossDatabaseAsync(
        DbConnection targetConnection,
        DbTransaction transaction,
        string targetSchema,
        string fgsChildTableName,
        IReadOnlyList<UniversalMatrixChildSourceRow> sourceRows,
        Dictionary<string, long> serviceIds,
        HashSet<string> existingKeys,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var fgsChildTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, fgsChildTableName);
        var inserted = 0;

        foreach (var row in sourceRows)
        {
            if (!serviceIds.TryGetValue(row.ServiceCode, out var universalPricingServiceId))
            {
                continue;
            }

            var key = $"{universalPricingServiceId}:{row.Name}";
            if (existingKeys.Contains(key))
            {
                continue;
            }

            await using var insertCommand = targetConnection.CreateCommand();
            insertCommand.Transaction = transaction;
            insertCommand.CommandText = $"""
                INSERT INTO {fgsChildTable}
                (
                    "TenantId", "CompanyId", "UniversalPricingServiceId", "Name",
                    "Multiplier", "DisplayOrder", "IsActive", "CreatedOn", "CreatedBy"
                )
                VALUES
                (
                    @tenantId, @companyId, @universalPricingServiceId, @name,
                    @multiplier, @displayOrder, true, NOW(), @seedCreatedBy
                )
                """;
            TenantSeedCommandExtensions.AddSeedParameters(insertCommand, tenantId, companyId);
            AddSeedCreatedByParameter(insertCommand);
            AddParameter(insertCommand, "universalPricingServiceId", universalPricingServiceId);
            AddParameter(insertCommand, "name", row.Name);
            AddParameter(insertCommand, "multiplier", row.Multiplier);
            AddParameter(insertCommand, "displayOrder", row.DisplayOrder);

            inserted += await insertCommand.ExecuteNonQueryAsync(cancellationToken);
            existingKeys.Add(key);
        }

        return inserted;
    }

    private static void AddSeedCreatedByParameter(DbCommand command) =>
        AddParameter(command, "seedCreatedBy", SeedTransformationTypes.SeedCreatedByValue);

    private static void AddParameter(DbCommand command, string name, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }

    private static async Task TryRollbackAsync(DbTransaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            await transaction.RollbackAsync(cancellationToken);
        }
        catch
        {
            // Best-effort rollback.
        }
    }

    private sealed record UniversalMatrixChildSourceRow(
        string Name,
        decimal Multiplier,
        short DisplayOrder,
        string ServiceCode);

    private sealed record RolePermissionSourceRow(string RoleCode, string PermissionCode);
}
