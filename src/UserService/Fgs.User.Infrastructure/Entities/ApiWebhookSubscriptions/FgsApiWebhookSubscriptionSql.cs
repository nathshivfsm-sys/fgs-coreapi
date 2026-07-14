using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.ApiWebhookSubscriptions;

internal static class FgsApiWebhookSubscriptionSql
{
    public const string Table = "identity.\"FgsApiWebhookSubscription\"";

    public const string SelectColumns = """
        "Id", "FgsApiWebhookId", "FgsApiEventId", "CreatedOn", "CreatedBy"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "FgsApiWebhookId", "FgsApiEventId", "CreatedOn"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"CreatedOn\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
