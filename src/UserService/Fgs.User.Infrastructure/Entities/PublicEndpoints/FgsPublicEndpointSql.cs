using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.PublicEndpoints;

internal static class FgsPublicEndpointSql
{
    public const string Table = "identity.\"FgsPublicEndpoint\"";

    public const string SelectDetailColumns = """
        "Id", "EndpointType", "EnvironmentCode", "BaseUrl", "DisplayName", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "EndpointType", "EnvironmentCode", "BaseUrl", "DisplayName", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "EndpointType", "EnvironmentCode", "BaseUrl", "DisplayName"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "EndpointType", "EnvironmentCode", "BaseUrl", "DisplayName", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"EndpointType\" {dir}, \"EnvironmentCode\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
