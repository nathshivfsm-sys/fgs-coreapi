using Fgs.Foundation.Paging;

namespace Fgs.Setup.Infrastructure.Common;

/// <summary>
/// Shared ORDER BY resolution for Setup Dapper list queries (whitelist + optional alias/NULLS LAST).
/// </summary>
internal static class SetupSqlOrderBy
{
    public static string Resolve(
        string? sortBy,
        SortDirection direction,
        IReadOnlySet<string> allowedColumns,
        string defaultColumn = "Id",
        string? tableAlias = null,
        string? nullsLastTiebreakerColumn = null)
    {
        ArgumentNullException.ThrowIfNull(allowedColumns);

        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        var prefix = string.IsNullOrWhiteSpace(tableAlias) ? string.Empty : $"{tableAlias}.";

        if (string.IsNullOrWhiteSpace(sortBy) || !allowedColumns.Contains(sortBy))
        {
            if (ContainsColumn(allowedColumns, "DisplayOrder"))
            {
                return $"ORDER BY {prefix}\"DisplayOrder\" {dir} NULLS LAST, {prefix}\"Id\" {dir}";
            }

            if (ContainsColumn(allowedColumns, "SortOrder"))
            {
                return $"ORDER BY {prefix}\"SortOrder\" {dir} NULLS LAST, {prefix}\"Id\" {dir}";
            }

            return $"ORDER BY {prefix}\"{defaultColumn}\" {dir}";
        }

        var resolvedKey = allowedColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));

        if (nullsLastTiebreakerColumn is not null && IsNullableSortColumn(resolvedKey))
        {
            return $"ORDER BY {prefix}\"{resolvedKey}\" {dir} NULLS LAST, {prefix}\"{nullsLastTiebreakerColumn}\" {dir}";
        }

        if (IsNullableSortColumn(resolvedKey))
        {
            return $"ORDER BY {prefix}\"{resolvedKey}\" {dir} NULLS LAST, {prefix}\"Id\" {dir}";
        }

        return $"ORDER BY {prefix}\"{resolvedKey}\" {dir}";
    }

    private static bool ContainsColumn(IReadOnlySet<string> allowedColumns, string column) =>
        allowedColumns.Contains(column);

    private static bool IsNullableSortColumn(string column) =>
        column.Equals("DisplayOrder", StringComparison.OrdinalIgnoreCase)
        || column.Equals("SortOrder", StringComparison.OrdinalIgnoreCase);
}
