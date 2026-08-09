using System.Text;
using Fgs.Setup.Application.Features.TenantProvisioning;
using Fgs.Setup.Domain.Entities;

namespace Fgs.Setup.Infrastructure.Provisioning;

internal static class TenantSeedSqlBuilder
{
    public static string QualifyTable(string schemaName, string tableName) =>
        $"{QuoteIdentifier(schemaName)}.{QuoteIdentifier(tableName)}";

    public static string BuildInsertSelectSql(
        string qualifiedTarget,
        string qualifiedSource,
        IReadOnlyList<GloSeedTableColumnMapping> columns,
        string? additionalWhereClause)
    {
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

        var whereClause = string.IsNullOrWhiteSpace(additionalWhereClause)
            ? string.Empty
            : $" WHERE {additionalWhereClause}";

        return $"""
            INSERT INTO {qualifiedTarget} ({targetColumns})
            SELECT {selectExpressions}
            FROM {qualifiedSource}{whereClause}
            ON CONFLICT DO NOTHING
            """;
    }

    public static string? BuildBusinessTypeFilterClause(
        bool sourceHasBusinessTypeId,
        bool businessTypeColumnIsNullable,
        bool hasBusinessTypeFilter)
    {
        if (!sourceHasBusinessTypeId || !hasBusinessTypeFilter)
        {
            return null;
        }

        var column = QuoteIdentifier(SeedTransformationTypes.SourceColumns.BusinessTypeId);
        var parameter = $"@{SeedTransformationTypes.SqlParameters.BusinessTypeIds}";

        return businessTypeColumnIsNullable
            ? $"({column} = ANY({parameter}) OR {column} IS NULL)"
            : $"{column} = ANY({parameter})";
    }

    public static string? BuildTenantScopeFilterClause(
        GloSeedTableMapping mapping,
        IReadOnlyList<GloSeedTableColumnMapping> columns,
        bool sourceHasTenantId)
    {
        if (!string.Equals(mapping.SourceSchemaName, "tenant", StringComparison.OrdinalIgnoreCase)
            || !sourceHasTenantId)
        {
            return null;
        }

        var seedsTenantId = columns.Any(c =>
            string.Equals(c.TargetColumnName, SeedTransformationTypes.TargetColumns.TenantId, StringComparison.OrdinalIgnoreCase)
            && string.Equals(c.TransformationType, SeedTransformationTypes.TenantId, StringComparison.Ordinal));

        if (!seedsTenantId)
        {
            return null;
        }

        var tenantColumn = QuoteIdentifier(SeedTransformationTypes.TargetColumns.TenantId);
        return $"{tenantColumn} = @{SeedTransformationTypes.SqlParameters.TenantId}";
    }

    public static string? BuildSourceFilterClause(GloSeedTableMapping mapping) =>
        TenantSeedSourceFilters.TryGetFilter(mapping.SeedCode);

    public static string? CombineWhereClauses(params string?[] clauses)
    {
        var parts = clauses
            .Where(clause => !string.IsNullOrWhiteSpace(clause))
            .Select(clause => clause!.Trim())
            .ToArray();

        return parts.Length == 0 ? null : string.Join(" AND ", parts);
    }

    internal static string BuildSelectExpression(GloSeedTableColumnMapping column)
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

    internal static string QuoteIdentifier(string identifier) =>
        $"\"{identifier.Replace("\"", "\"\"", StringComparison.Ordinal)}\"";

    internal static string ToSqlLiteral(string? value) =>
        value is null ? "NULL" : $"'{value.Replace("'", "''", StringComparison.Ordinal)}'";
}
