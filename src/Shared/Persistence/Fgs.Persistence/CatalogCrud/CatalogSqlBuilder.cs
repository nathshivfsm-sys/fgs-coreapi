using Dapper;
using Fgs.Foundation.CatalogCrud;

namespace Fgs.Persistence.CatalogCrud;

internal static class CatalogSqlBuilder
{
    public static (string Sql, DynamicParameters Parameters) BuildGetById(
        CatalogEntityDescriptor descriptor,
        object parsedId,
        long? tenantId,
        long? companyId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Id", parsedId);

        var where = BuildTenantWhere(descriptor, tenantId, companyId, parameters);
        where.Add("\"Id\" = @Id");

        var columns = string.Join(", ", descriptor.Columns.Select(c => Quote(c.ColumnName)));
        var sql = $"""
            SELECT {columns}
            FROM {descriptor.QualifiedTableName}
            WHERE {string.Join(" AND ", where)}
            LIMIT 1
            """;

        return (sql, parameters);
    }

    public static (string Sql, DynamicParameters Parameters) BuildList(
        CatalogEntityDescriptor descriptor,
        PagedQuery paging,
        IReadOnlyDictionary<string, string?> filters,
        long? tenantId,
        long? companyId)
    {
        var parameters = new DynamicParameters();
        var where = BuildTenantWhere(descriptor, tenantId, companyId, parameters);

        if (paging.IsActive.HasValue && descriptor.SupportsSoftDelete)
        {
            where.Add("\"IsActive\" = @IsActive");
            parameters.Add("IsActive", paging.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(paging.Search) && descriptor.SearchableColumns.Count > 0)
        {
            var searchClauses = descriptor.SearchableColumns
                .Select(column => $"\"{column}\" ILIKE @Search")
                .ToList();
            where.Add($"({string.Join(" OR ", searchClauses)})");
            parameters.Add("Search", $"%{paging.Search.Trim()}%");
        }

        foreach (var filter in filters)
        {
            if (string.IsNullOrWhiteSpace(filter.Value))
            {
                continue;
            }

            var column = descriptor.Columns.FirstOrDefault(c =>
                string.Equals(c.PropertyName, filter.Key, StringComparison.OrdinalIgnoreCase));
            if (column is null)
            {
                continue;
            }

            var parameterName = $"Filter_{column.PropertyName}";
            where.Add($"\"{column.ColumnName}\" = @{parameterName}");
            parameters.Add(parameterName, filter.Value);
        }

        var sortColumn = ResolveSortColumn(descriptor, paging.SortBy);
        var sortDirection = paging.SortDirection == SortDirection.Desc ? "DESC" : "ASC";
        var offset = (paging.Page - 1) * paging.PageSize;

        parameters.Add("Limit", paging.PageSize);
        parameters.Add("Offset", offset);

        var columns = string.Join(", ", descriptor.Columns.Select(c => Quote(c.ColumnName)));
        var whereClause = where.Count == 0 ? "TRUE" : string.Join(" AND ", where);

        var sql = $"""
            SELECT {columns}
            FROM {descriptor.QualifiedTableName}
            WHERE {whereClause}
            ORDER BY "{sortColumn}" {sortDirection}
            LIMIT @Limit OFFSET @Offset;

            SELECT COUNT(1)
            FROM {descriptor.QualifiedTableName}
            WHERE {whereClause};
            """;

        return (sql, parameters);
    }

    public static (string Sql, DynamicParameters Parameters) BuildExists(
        CatalogEntityDescriptor descriptor,
        IReadOnlyDictionary<string, object?> propertyValues,
        object? excludeId,
        long? tenantId,
        long? companyId)
    {
        var parameters = new DynamicParameters();
        var where = BuildTenantWhere(descriptor, tenantId, companyId, parameters);

        foreach (var pair in propertyValues)
        {
            var column = descriptor.Columns.First(c =>
                string.Equals(c.PropertyName, pair.Key, StringComparison.Ordinal));
            var parameterName = $"Exists_{column.PropertyName}";
            where.Add($"\"{column.ColumnName}\" = @{parameterName}");
            parameters.Add(parameterName, pair.Value);
        }

        if (excludeId is not null)
        {
            where.Add("\"Id\" <> @ExcludeId");
            parameters.Add("ExcludeId", excludeId);
        }

        var sql = $"""
            SELECT EXISTS(
                SELECT 1
                FROM {descriptor.QualifiedTableName}
                WHERE {string.Join(" AND ", where)}
            )
            """;

        return (sql, parameters);
    }

    private static List<string> BuildTenantWhere(
        CatalogEntityDescriptor descriptor,
        long? tenantId,
        long? companyId,
        DynamicParameters parameters)
    {
        var where = new List<string>();

        if (descriptor.Variant == CatalogEntityVariant.NullableTenantScope)
        {
            if (tenantId.HasValue && companyId.HasValue)
            {
                where.Add("(\"TenantId\" IS NULL AND \"CompanyId\" IS NULL OR (\"TenantId\" = @TenantId AND \"CompanyId\" = @CompanyId))");
                parameters.Add("TenantId", tenantId.Value);
                parameters.Add("CompanyId", companyId.Value);
            }

            return where;
        }

        if (tenantId.HasValue)
        {
            where.Add("\"TenantId\" = @TenantId");
            parameters.Add("TenantId", tenantId.Value);
        }

        if (companyId.HasValue)
        {
            where.Add("\"CompanyId\" = @CompanyId");
            parameters.Add("CompanyId", companyId.Value);
        }

        return where;
    }

    private static string ResolveSortColumn(CatalogEntityDescriptor descriptor, string? sortBy)
    {
        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            var match = descriptor.SortableColumns.FirstOrDefault(column =>
                string.Equals(column, sortBy, StringComparison.OrdinalIgnoreCase));
            if (match is not null)
            {
                return match;
            }
        }

        return descriptor.SortableColumns.FirstOrDefault() ?? "Id";
    }

    private static string Quote(string columnName) => $"\"{columnName}\"";
}
