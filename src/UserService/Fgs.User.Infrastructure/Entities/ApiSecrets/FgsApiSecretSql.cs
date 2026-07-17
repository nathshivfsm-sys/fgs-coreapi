using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.ApiSecrets;

internal static class FgsApiSecretSql
{
    public const string Table = "identity.\"FgsApiSecret\"";

    public const string SelectDetailColumns = """
        "Id", "FgsApiClientId", "Name", "ExpiresOn", "LastUsedOn", "RevokedOn", "RevokedBy", "IsActive", "CreatedOn", "CreatedBy"
        """;

    public const string SelectSummaryColumns = """
        "Id", "FgsApiClientId", "Name", "ExpiresOn", "LastUsedOn", "RevokedOn", "IsActive", "CreatedOn", "CreatedBy"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "FgsApiClientId", "Name", "ExpiresOn", "LastUsedOn", "RevokedOn", "IsActive", "CreatedOn", "CreatedBy"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"CreatedOn\" {dir}, \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
