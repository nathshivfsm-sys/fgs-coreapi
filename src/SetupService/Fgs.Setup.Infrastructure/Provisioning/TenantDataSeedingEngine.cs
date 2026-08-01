using System.Data;
using System.Data.Common;
using Fgs.Setup.Application.Abstractions.Provisioning;
using Fgs.Setup.Application.Features.TenantProvisioning;
using Fgs.Setup.Domain.Entities;
using Fgs.Setup.Infrastructure.Common.Options;
using Fgs.Setup.Infrastructure.Database;
using Fgs.Setup.Infrastructure.Database.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fgs.Setup.Infrastructure.Provisioning;

public sealed class TenantDataSeedingEngine(
    FgsSetupDbContext dbContext,
    ITenantSeedDatabaseConnectionFactory connectionFactory,
    IOptions<TenantProvisioningOptions> provisioningOptions,
    ILogger<TenantDataSeedingEngine> logger) : ITenantDataSeedingEngine
{
    private readonly int _batchSize = Math.Max(1, provisioningOptions.Value.SeedingBatchSize);

    public async Task<TenantDataSeedResult> SeedTenantDataAsync(
        long tenantId,
        long companyId,
        IReadOnlyList<int>? gloBusinessTypeIds = null,
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
            logger.LogWarning(
                "No active GloSeedTableMapping rows found; skipping tenant data seed for tenant {TenantId}",
                tenantId);
            return new TenantDataSeedResult(0, 0, 0, []);
        }

        var columnMappings = await dbContext.GloSeedTableColumnMappings
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.SeedTableMappingId)
            .ThenBy(c => c.ColumnOrder)
            .ThenBy(c => c.Id)
            .ToListAsync(cancellationToken);

        var primaryConnection = dbContext.Database.GetDbConnection();
        if (primaryConnection.State != ConnectionState.Open)
        {
            await primaryConnection.OpenAsync(cancellationToken);
        }

        var defaultDatabase = await GetCurrentDatabaseNameAsync(primaryConnection, cancellationToken);
        await using var connectionScope = new TenantSeedConnectionScope(
            connectionFactory,
            primaryConnection,
            defaultDatabase);

        var validator = new TenantSeedMetadataValidator();
        var businessTypeIds = gloBusinessTypeIds?
            .Where(id => id > 0)
            .Distinct()
            .ToArray() ?? [];
        var hasBusinessTypeFilter = businessTypeIds.Length > 0;

        if (!hasBusinessTypeFilter)
        {
            logger.LogInformation(
                "Tenant seed for tenant {TenantId} has no business type filter; business-type-scoped source rows with required BusinessTypeId will be skipped",
                tenantId);
        }

        var tableResults = new List<TenantSeedTableResult>();

        foreach (var mapping in mappings)
        {
            var columns = columnMappings
                .Where(c => c.SeedTableMappingId == mapping.Id)
                .ToList();

            if (columns.Count == 0)
            {
                logger.LogWarning(
                    "Seed mapping {SeedCode} (id {MappingId}) has no active column mappings; skipping",
                    mapping.SeedCode,
                    mapping.Id);
                tableResults.Add(new TenantSeedTableResult(
                    mapping.SeedCode,
                    TenantSeedTableOutcome.Skipped,
                    SeedTransformationTypes.ErrorMessages.NoColumnMappingsFormat));
                continue;
            }

            if (columns.Any(c =>
                    string.Equals(
                        c.TransformationType,
                        SeedTransformationTypes.JoinedParent,
                        StringComparison.OrdinalIgnoreCase)))
            {
                logger.LogInformation(
                    "Seed mapping {SeedCode} uses JOINED_PARENT; skipping flat seed (handled by joined-child soft path)",
                    mapping.SeedCode);
                tableResults.Add(new TenantSeedTableResult(
                    mapping.SeedCode,
                    TenantSeedTableOutcome.Skipped,
                    string.Format(
                        SeedTransformationTypes.ErrorMessages.JoinedParentHandledBySoftPathFormat,
                        mapping.SeedCode)));
                continue;
            }

            TenantSeedTableResult tableResult;
            try
            {
                tableResult = await SeedSingleTableAsync(
                    connectionScope,
                    validator,
                    mapping,
                    columns,
                    tenantId,
                    companyId,
                    businessTypeIds,
                    hasBusinessTypeFilter,
                    cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Unexpected error seeding {SeedCode} for tenant {TenantId}, company {CompanyId}",
                    mapping.SeedCode,
                    tenantId,
                    companyId);
                tableResult = new TenantSeedTableResult(
                    mapping.SeedCode,
                    TenantSeedTableOutcome.Failed,
                    ex.Message);
            }

            tableResults.Add(tableResult);
            LogTableResult(tableResult, mapping.SeedCode, tenantId, companyId);
        }

        try
        {
            await SeedInventorySubCategoriesAsync(
                connectionScope,
                mappings,
                tenantId,
                companyId,
                businessTypeIds,
                hasBusinessTypeFilter,
                cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Inventory sub-category seed failed for tenant {TenantId}, company {CompanyId}",
                tenantId,
                companyId);
            tableResults.Add(new TenantSeedTableResult(
                "FgsInventorySubCategory",
                TenantSeedTableOutcome.Failed,
                ex.Message));
        }

        try
        {
            await SeedUniversalMatrixTiersAsync(
                connectionScope,
                mappings,
                tenantId,
                companyId,
                cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Universal matrix tier seed failed for tenant {TenantId}, company {CompanyId}",
                tenantId,
                companyId);
            tableResults.Add(new TenantSeedTableResult(
                "FgsUniversalMatrixTier",
                TenantSeedTableOutcome.Failed,
                ex.Message));
        }

        var succeeded = tableResults.Count(r => r.Outcome == TenantSeedTableOutcome.Succeeded);
        var skipped = tableResults.Count(r => r.Outcome == TenantSeedTableOutcome.Skipped);
        var failed = tableResults.Count(r => r.Outcome == TenantSeedTableOutcome.Failed);

        logger.LogInformation(
            "Tenant seed summary for tenant {TenantId}: {Succeeded} succeeded, {Skipped} skipped, {Failed} failed (total {Total})",
            tenantId,
            succeeded,
            skipped,
            failed,
            tableResults.Count);

        return new TenantDataSeedResult(succeeded, skipped, failed, tableResults);
    }

    private async Task<TenantSeedTableResult> SeedSingleTableAsync(
        TenantSeedConnectionScope connectionScope,
        TenantSeedMetadataValidator validator,
        GloSeedTableMapping mapping,
        IReadOnlyList<GloSeedTableColumnMapping> columns,
        long tenantId,
        long companyId,
        int[] businessTypeIds,
        bool hasBusinessTypeFilter,
        CancellationToken cancellationToken)
    {
        var validation = await validator.ValidateMappingAsync(
            connectionScope,
            mapping,
            columns,
            cancellationToken);

        if (!validation.IsValid)
        {
            return new TenantSeedTableResult(
                mapping.SeedCode,
                TenantSeedTableOutcome.Failed,
                validation.ErrorMessage);
        }

        var sourceMetadata = validation.SourceMetadata!;
        if (sourceMetadata.HasBusinessTypeId
            && !sourceMetadata.BusinessTypeIdIsNullable
            && !hasBusinessTypeFilter)
        {
            return new TenantSeedTableResult(
                mapping.SeedCode,
                TenantSeedTableOutcome.Skipped,
                "Source table requires BusinessTypeId filtering but no business types were supplied.");
        }

        var (sourceConnection, targetConnection) = await connectionScope.GetSourceAndTargetConnectionsAsync(
            mapping.SourceDatabaseName,
            mapping.TargetDatabaseName,
            cancellationToken);

        var isCrossDatabase = connectionScope.IsCrossDatabase(
            mapping.SourceDatabaseName,
            mapping.TargetDatabaseName);

        var seedsTenantId = columns.Any(c =>
            string.Equals(c.TargetColumnName, SeedTransformationTypes.TargetColumns.TenantId, StringComparison.OrdinalIgnoreCase)
            && string.Equals(c.TransformationType, SeedTransformationTypes.TenantId, StringComparison.Ordinal));

        await using var transaction = await targetConnection.BeginTransactionAsync(cancellationToken);

        try
        {
            if (seedsTenantId
                && await TargetAlreadySeededAsync(
                    targetConnection,
                    transaction,
                    mapping.TargetSchemaName,
                    mapping.TargetTableName,
                    tenantId,
                    cancellationToken))
            {
                await transaction.CommitAsync(cancellationToken);
                return new TenantSeedTableResult(
                    mapping.SeedCode,
                    TenantSeedTableOutcome.Skipped,
                    $"Target {validation.TargetDatabaseName}.{mapping.TargetSchemaName}.{mapping.TargetTableName} already has rows for tenant {tenantId}.");
            }

            int inserted;
            if (isCrossDatabase)
            {
                logger.LogInformation(
                    "Cross-database seed {SeedCode}: {SourceDatabase}.{SourceSchema}.{SourceTable} -> {TargetDatabase}.{TargetSchema}.{TargetTable}",
                    mapping.SeedCode,
                    validation.SourceDatabaseName,
                    mapping.SourceSchemaName,
                    mapping.SourceTableName,
                    validation.TargetDatabaseName,
                    mapping.TargetSchemaName,
                    mapping.TargetTableName);

                inserted = await TenantSeedCrossDatabaseCopier.CopyAsync(
                    sourceConnection,
                    targetConnection,
                    transaction,
                    mapping,
                    columns,
                    sourceMetadata,
                    tenantId,
                    companyId,
                    businessTypeIds,
                    hasBusinessTypeFilter,
                    _batchSize,
                    logger,
                    cancellationToken);
            }
            else
            {
                inserted = await ExecuteSameDatabaseSeedAsync(
                    targetConnection,
                    transaction,
                    mapping,
                    columns,
                    sourceMetadata,
                    businessTypeIds,
                    hasBusinessTypeFilter,
                    tenantId,
                    companyId,
                    cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

            return new TenantSeedTableResult(
                mapping.SeedCode,
                TenantSeedTableOutcome.Succeeded,
                isCrossDatabase
                    ? $"Cross-database copy completed ({validation.SourceDatabaseName} -> {validation.TargetDatabaseName})."
                    : null,
                inserted);
        }
        catch (Exception ex)
        {
            await TryRollbackAsync(transaction, cancellationToken);
            return new TenantSeedTableResult(
                mapping.SeedCode,
                TenantSeedTableOutcome.Failed,
                ex.Message);
        }
    }

    private static async Task<int> ExecuteSameDatabaseSeedAsync(
        DbConnection targetConnection,
        DbTransaction transaction,
        GloSeedTableMapping mapping,
        IReadOnlyList<GloSeedTableColumnMapping> columns,
        TenantSeedMetadataValidator.TableMetadata sourceMetadata,
        int[] businessTypeIds,
        bool hasBusinessTypeFilter,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var whereClause = TenantSeedSqlBuilder.CombineWhereClauses(
            TenantSeedSqlBuilder.BuildTenantScopeFilterClause(
                mapping,
                columns,
                sourceMetadata.Columns.ContainsKey(SeedTransformationTypes.TargetColumns.TenantId)),
            TenantSeedSqlBuilder.BuildBusinessTypeFilterClause(
                sourceMetadata.HasBusinessTypeId,
                sourceMetadata.BusinessTypeIdIsNullable,
                hasBusinessTypeFilter),
            TenantSeedSqlBuilder.BuildSourceFilterClause(mapping));

        var sql = TenantSeedSqlBuilder.BuildInsertSelectSql(
            TenantSeedSqlBuilder.QualifyTable(mapping.TargetSchemaName, mapping.TargetTableName),
            TenantSeedSqlBuilder.QualifyTable(mapping.SourceSchemaName, mapping.SourceTableName),
            columns,
            whereClause);

        await using var command = targetConnection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = sql;
        TenantSeedCommandExtensions.AddSeedParameters(command, tenantId, companyId, businessTypeIds);

        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task SeedInventorySubCategoriesAsync(
        TenantSeedConnectionScope connectionScope,
        IReadOnlyList<GloSeedTableMapping> mappings,
        long tenantId,
        long companyId,
        int[] businessTypeIds,
        bool hasBusinessTypeFilter,
        CancellationToken cancellationToken)
    {
        var connections = await TenantJoinedChildSeedHelper.ResolveConnectionsAsync(
            connectionScope,
            mappings,
            FgsDatabaseSchemas.Inventory,
            cancellationToken);

        var validator = new TenantSeedMetadataValidator();
        var sourceDatabase = connectionScope.ResolveSourceDatabaseName(connections.SourceDatabaseName);
        var targetDatabase = connectionScope.ResolveTargetDatabaseName(connections.TargetDatabaseName);

        var sourceSubMetadata = await validator.GetTableMetadataAsync(
            connections.SourceConnection, sourceDatabase, connections.SourceSchema, "GloInventorySubCategory", cancellationToken);
        var sourceCategoryMetadata = await validator.GetTableMetadataAsync(
            connections.SourceConnection, sourceDatabase, connections.SourceSchema, "GloInventoryCategory", cancellationToken);
        var targetSubMetadata = await validator.GetTableMetadataAsync(
            connections.TargetConnection, targetDatabase, connections.TargetSchema, "FgsInventorySubCategory", cancellationToken);
        var targetCategoryMetadata = await validator.GetTableMetadataAsync(
            connections.TargetConnection, targetDatabase, connections.TargetSchema, "FgsInventoryCategory", cancellationToken);

        if (!sourceSubMetadata.Exists
            || !sourceCategoryMetadata.Exists
            || !targetSubMetadata.Exists
            || !targetCategoryMetadata.Exists)
        {
            logger.LogWarning(
                "Skipping inventory sub-category seed because required catalog tables are missing (source sub={SourceSub}, source category={SourceCategory}, target sub={TargetSub}, target category={TargetCategory})",
                sourceSubMetadata.Exists,
                sourceCategoryMetadata.Exists,
                targetSubMetadata.Exists,
                targetCategoryMetadata.Exists);
            return;
        }

        if (await TenantJoinedChildSeedHelper.IsTenantCompanySeededAsync(
                connections.TargetConnection,
                connections.TargetSchema,
                "FgsInventorySubCategory",
                tenantId,
                companyId,
                cancellationToken))
        {
            logger.LogInformation(
                "FgsInventorySubCategory already seeded for tenant {TenantId}, company {CompanyId}; skipping",
                tenantId,
                companyId);
            return;
        }

        if (sourceCategoryMetadata.HasBusinessTypeId
            && !sourceCategoryMetadata.BusinessTypeIdIsNullable
            && !hasBusinessTypeFilter)
        {
            logger.LogWarning(
                "Skipping inventory sub-category seed for tenant {TenantId}: business type filter required",
                tenantId);
            return;
        }

        var inserted = await TenantJoinedChildSeedHelper.ExecuteInTargetTransactionAsync(
            connections.TargetConnection,
            async (transaction, ct) =>
            {
                if (connections.IsCrossDatabase)
                {
                    return await SeedInventorySubCategoriesCrossDatabaseAsync(
                        connections.SourceConnection,
                        connections.TargetConnection,
                        transaction,
                        connections.SourceSchema,
                        connections.TargetSchema,
                        sourceCategoryMetadata,
                        tenantId,
                        companyId,
                        businessTypeIds,
                        hasBusinessTypeFilter,
                        ct);
                }

                return await SeedInventorySubCategoriesSameDatabaseAsync(
                    connections.TargetConnection,
                    transaction,
                    connections.SourceSchema,
                    connections.TargetSchema,
                    sourceCategoryMetadata,
                    tenantId,
                    companyId,
                    businessTypeIds,
                    hasBusinessTypeFilter,
                    ct);
            },
            cancellationToken);

        logger.LogInformation(
            "Seeded {RowCount} FgsInventorySubCategory row(s) for tenant {TenantId}, company {CompanyId}",
            inserted,
            tenantId,
            companyId);
    }

    private static async Task<int> SeedInventorySubCategoriesSameDatabaseAsync(
        DbConnection connection,
        DbTransaction transaction,
        string sourceSchema,
        string targetSchema,
        TenantSeedMetadataValidator.TableMetadata sourceCategoryMetadata,
        long tenantId,
        long companyId,
        int[] businessTypeIds,
        bool hasBusinessTypeFilter,
        CancellationToken cancellationToken)
    {
        var gloSubCategoryTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, "GloInventorySubCategory");
        var gloCategoryTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, "GloInventoryCategory");
        var fgsCategoryTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsInventoryCategory");
        var fgsSubCategoryTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsInventorySubCategory");

        var gloBusinessFilter = hasBusinessTypeFilter && sourceCategoryMetadata.HasBusinessTypeId
            ? $" AND gc.\"BusinessTypeId\" = ANY(@{SeedTransformationTypes.SqlParameters.BusinessTypeIds})"
            : string.Empty;

        var sql = $"""
            INSERT INTO {fgsSubCategoryTable}
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
            FROM {gloSubCategoryTable} glo
            INNER JOIN {gloCategoryTable} gc ON gc."Id" = glo."InventoryCategoryId"
            INNER JOIN {fgsCategoryTable} fc
                ON fc."TenantId" = @tenantId
               AND fc."CompanyId" = @companyId
               AND fc."CategoryCode" = gc."CategoryCode"
            WHERE NOT EXISTS (
                SELECT 1
                FROM {fgsSubCategoryTable} existing
                WHERE existing."TenantId" = @tenantId
                  AND existing."CompanyId" = @companyId
                  AND existing."InventoryCategoryId" = fc."Id"
                  AND existing."SubCategoryCode" = glo."SubCategoryCode"
            ){gloBusinessFilter}
            """;

        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = sql;
        TenantSeedCommandExtensions.AddSeedParameters(command, tenantId, companyId);
        AddSeedCreatedByParameter(command);

        if (!string.IsNullOrEmpty(gloBusinessFilter))
        {
            TenantSeedCommandExtensions.AddBusinessTypeIdsParameter(command, businessTypeIds);
        }

        return await command.ExecuteNonQueryAsync(cancellationToken);
    }

    private async Task<int> SeedInventorySubCategoriesCrossDatabaseAsync(
        DbConnection sourceConnection,
        DbConnection targetConnection,
        DbTransaction transaction,
        string sourceSchema,
        string targetSchema,
        TenantSeedMetadataValidator.TableMetadata sourceCategoryMetadata,
        long tenantId,
        long companyId,
        int[] businessTypeIds,
        bool hasBusinessTypeFilter,
        CancellationToken cancellationToken)
    {
        var gloSubCategoryTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, "GloInventorySubCategory");
        var gloCategoryTable = TenantSeedSqlBuilder.QualifyTable(sourceSchema, "GloInventoryCategory");
        var fgsCategoryTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsInventoryCategory");
        var fgsSubCategoryTable = TenantSeedSqlBuilder.QualifyTable(targetSchema, "FgsInventorySubCategory");

        var businessFilter = hasBusinessTypeFilter && sourceCategoryMetadata.HasBusinessTypeId
            ? $"WHERE gc.\"BusinessTypeId\" = ANY(@{SeedTransformationTypes.SqlParameters.BusinessTypeIds})"
            : string.Empty;

        var sourceSql = $"""
            SELECT
                glo."SubCategoryCode",
                glo."Name",
                glo."Description",
                glo."DisplayOrder",
                glo."IsActive",
                gc."CategoryCode"
            FROM {gloSubCategoryTable} glo
            INNER JOIN {gloCategoryTable} gc ON gc."Id" = glo."InventoryCategoryId"
            {businessFilter}
            """;

        var sourceRows = new List<(string SubCategoryCode, string Name, string? Description, short DisplayOrder, bool IsActive, string CategoryCode)>();
        await using (var sourceCommand = sourceConnection.CreateCommand())
        {
            sourceCommand.CommandText = sourceSql;
            if (!string.IsNullOrEmpty(businessFilter))
            {
                TenantSeedCommandExtensions.AddBusinessTypeIdsParameter(sourceCommand, businessTypeIds);
            }

            await using var reader = await sourceCommand.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                sourceRows.Add((
                    reader.GetString(0),
                    reader.GetString(1),
                    await reader.IsDBNullAsync(2, cancellationToken) ? null : reader.GetString(2),
                    reader.GetInt16(3),
                    reader.GetBoolean(4),
                    reader.GetString(5)));
            }
        }

        var targetCategories = new Dictionary<string, long>(StringComparer.OrdinalIgnoreCase);
        await using (var categoryCommand = targetConnection.CreateCommand())
        {
            categoryCommand.Transaction = transaction;
            categoryCommand.CommandText = $"""
                SELECT "Id", "CategoryCode"
                FROM {fgsCategoryTable}
                WHERE "TenantId" = @tenantId AND "CompanyId" = @companyId
                """;
            TenantSeedCommandExtensions.AddSeedParameters(categoryCommand, tenantId, companyId);

            await using var reader = await categoryCommand.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                targetCategories[reader.GetString(1)] = reader.GetInt64(0);
            }
        }

        var existingKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        await using (var existingCommand = targetConnection.CreateCommand())
        {
            existingCommand.Transaction = transaction;
            existingCommand.CommandText = $"""
                SELECT "InventoryCategoryId", "SubCategoryCode"
                FROM {fgsSubCategoryTable}
                WHERE "TenantId" = @tenantId AND "CompanyId" = @companyId
                """;
            TenantSeedCommandExtensions.AddSeedParameters(existingCommand, tenantId, companyId);

            await using var reader = await existingCommand.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                existingKeys.Add($"{reader.GetInt64(0)}:{reader.GetString(1)}");
            }
        }

        var inserted = 0;
        foreach (var row in sourceRows)
        {
            if (!targetCategories.TryGetValue(row.CategoryCode, out var inventoryCategoryId))
            {
                continue;
            }

            var key = $"{inventoryCategoryId}:{row.SubCategoryCode}";
            if (existingKeys.Contains(key))
            {
                continue;
            }

            await using var insertCommand = targetConnection.CreateCommand();
            insertCommand.Transaction = transaction;
            insertCommand.CommandText = $"""
                INSERT INTO {fgsSubCategoryTable}
                (
                    "TenantId", "CompanyId", "InventoryCategoryId", "SubCategoryCode",
                    "Name", "Description", "DisplayOrder", "IsSystem", "IsActive", "CreatedOn", "CreatedBy"
                )
                VALUES
                (
                    @tenantId, @companyId, @inventoryCategoryId, @subCategoryCode,
                    @name, @description, @displayOrder, true, @isActive, NOW(), @seedCreatedBy
                )
                """;
            TenantSeedCommandExtensions.AddSeedParameters(insertCommand, tenantId, companyId);
            AddSeedCreatedByParameter(insertCommand);
            AddParameter(insertCommand, "inventoryCategoryId", inventoryCategoryId);
            AddParameter(insertCommand, "subCategoryCode", row.SubCategoryCode);
            AddParameter(insertCommand, "name", row.Name);
            AddParameter(insertCommand, "description", row.Description ?? (object)DBNull.Value);
            AddParameter(insertCommand, "displayOrder", row.DisplayOrder);
            AddParameter(insertCommand, "isActive", row.IsActive);

            inserted += await insertCommand.ExecuteNonQueryAsync(cancellationToken);
            existingKeys.Add(key);
        }

        return inserted;
    }

    private async Task SeedUniversalMatrixTiersAsync(
        TenantSeedConnectionScope connectionScope,
        IReadOnlyList<GloSeedTableMapping> mappings,
        long tenantId,
        long companyId,
        CancellationToken cancellationToken)
    {
        var connections = await TenantJoinedChildSeedHelper.ResolveConnectionsAsync(
            connectionScope,
            mappings,
            FgsDatabaseSchemas.Setup,
            cancellationToken);

        var validator = new TenantSeedMetadataValidator();
        var sourceDatabase = connectionScope.ResolveSourceDatabaseName(connections.SourceDatabaseName);
        var targetDatabase = connectionScope.ResolveTargetDatabaseName(connections.TargetDatabaseName);

        var sourceTierMetadata = await validator.GetTableMetadataAsync(
            connections.SourceConnection, sourceDatabase, connections.SourceSchema, "GloUniversalMatrixTier", cancellationToken);
        var sourceServiceMetadata = await validator.GetTableMetadataAsync(
            connections.SourceConnection, sourceDatabase, connections.SourceSchema, "GloUniversalPricingService", cancellationToken);
        var targetTierMetadata = await validator.GetTableMetadataAsync(
            connections.TargetConnection, targetDatabase, connections.TargetSchema, "FgsUniversalMatrixTier", cancellationToken);
        var targetSizeTierMetadata = await validator.GetTableMetadataAsync(
            connections.TargetConnection, targetDatabase, connections.TargetSchema, "FgsUniversalMatrixSizeTier", cancellationToken);
        var targetServiceMetadata = await validator.GetTableMetadataAsync(
            connections.TargetConnection, targetDatabase, connections.TargetSchema, "FgsUniversalPricingService", cancellationToken);

        if (!sourceTierMetadata.Exists
            || !sourceServiceMetadata.Exists
            || !targetTierMetadata.Exists
            || !targetSizeTierMetadata.Exists
            || !targetServiceMetadata.Exists)
        {
            logger.LogWarning(
                "Skipping universal matrix tier seed because required tables are missing (source tier={SourceTier}, source service={SourceService}, target tier={TargetTier}, target size tier={TargetSizeTier}, target service={TargetService})",
                sourceTierMetadata.Exists,
                sourceServiceMetadata.Exists,
                targetTierMetadata.Exists,
                targetSizeTierMetadata.Exists,
                targetServiceMetadata.Exists);
            return;
        }

        if (await TenantJoinedChildSeedHelper.IsTenantCompanySeededAsync(
                connections.TargetConnection,
                connections.TargetSchema,
                "FgsUniversalMatrixTier",
                tenantId,
                companyId,
                cancellationToken))
        {
            logger.LogInformation(
                "FgsUniversalMatrixTier already seeded for tenant {TenantId}, company {CompanyId}; skipping",
                tenantId,
                companyId);
            return;
        }

        var (tierInserted, sizeTierInserted) = await TenantJoinedChildSeedHelper.ExecuteInTargetTransactionAsync(
            connections.TargetConnection,
            async (transaction, ct) =>
            {
                if (connections.IsCrossDatabase)
                {
                    return await TenantJoinedChildSeedHelper.SeedUniversalMatrixChildTablesCrossDatabaseAsync(
                        connections.SourceConnection,
                        connections.TargetConnection,
                        transaction,
                        connections.SourceSchema,
                        connections.TargetSchema,
                        tenantId,
                        companyId,
                        ct);
                }

                var tierCount = await TenantJoinedChildSeedHelper.SeedUniversalMatrixChildTableSameDatabaseAsync(
                    connections.TargetConnection,
                    transaction,
                    connections.SourceSchema,
                    connections.TargetSchema,
                    "GloUniversalMatrixTier",
                    "FgsUniversalMatrixTier",
                    tenantId,
                    companyId,
                    ct);
                var sizeTierCount = await TenantJoinedChildSeedHelper.SeedUniversalMatrixChildTableSameDatabaseAsync(
                    connections.TargetConnection,
                    transaction,
                    connections.SourceSchema,
                    connections.TargetSchema,
                    "GloUniversalMatrixSizeTier",
                    "FgsUniversalMatrixSizeTier",
                    tenantId,
                    companyId,
                    ct);

                return (tierCount, sizeTierCount);
            },
            cancellationToken);

        logger.LogInformation(
            "Seeded {TierCount} FgsUniversalMatrixTier and {SizeTierCount} FgsUniversalMatrixSizeTier row(s) for tenant {TenantId}, company {CompanyId}",
            tierInserted,
            sizeTierInserted,
            tenantId,
            companyId);
    }

    private void LogTableResult(
        TenantSeedTableResult tableResult,
        string seedCode,
        long tenantId,
        long companyId)
    {
        switch (tableResult.Outcome)
        {
            case TenantSeedTableOutcome.Succeeded:
                logger.LogInformation(
                    "Seed {SeedCode} succeeded: inserted {RowCount} row(s) for tenant {TenantId}, company {CompanyId}. {Details}",
                    seedCode,
                    tableResult.RowsInserted,
                    tenantId,
                    companyId,
                    tableResult.Message);
                break;
            case TenantSeedTableOutcome.Skipped:
                logger.LogInformation(
                    "Seed {SeedCode} skipped for tenant {TenantId}: {Reason}",
                    seedCode,
                    tenantId,
                    tableResult.Message);
                break;
            case TenantSeedTableOutcome.Failed:
                logger.LogError(
                    "Seed {SeedCode} failed for tenant {TenantId}, company {CompanyId}: {Reason}",
                    seedCode,
                    tenantId,
                    companyId,
                    tableResult.Message);
                break;
        }
    }

    private static async Task<bool> TargetAlreadySeededAsync(
        DbConnection connection,
        DbTransaction transaction,
        string targetSchema,
        string targetTable,
        long tenantId,
        CancellationToken cancellationToken)
    {
        var qualifiedTarget = TenantSeedSqlBuilder.QualifyTable(targetSchema, targetTable);
        var tenantColumn = TenantSeedSqlBuilder.QuoteIdentifier(SeedTransformationTypes.TargetColumns.TenantId);
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

    private static async Task<string> GetCurrentDatabaseNameAsync(
        DbConnection connection,
        CancellationToken cancellationToken)
    {
        await using var command = connection.CreateCommand();
        command.CommandText = "SELECT current_database()";
        var result = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToString(result)
            ?? throw new InvalidOperationException("Unable to resolve current database name.");
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
}
