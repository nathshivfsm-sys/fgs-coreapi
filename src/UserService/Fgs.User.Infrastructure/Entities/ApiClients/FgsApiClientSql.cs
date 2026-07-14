using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.ApiClients;

internal static class FgsApiClientSql
{
    public const string Table = "identity.\"FgsApiClient\"";

    public const string SelectDetailColumns = """
        "Id", "ClientId", "ApplicationName", "Description", "ContactName", "ContactEmail", "RateLimitPerMinute", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "ClientId", "ApplicationName", "Description", "ContactName", "ContactEmail", "RateLimitPerMinute", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "ClientId", "ApplicationName"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "ClientId", "ApplicationName", "ContactName", "ContactEmail", "RateLimitPerMinute", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"ApplicationName\" {dir}, \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
