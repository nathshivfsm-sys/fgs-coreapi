using Fgs.Foundation.Paging;

namespace Fgs.User.Infrastructure.Entities.ApiWebhooks;

internal static class FgsApiWebhookSql
{
    public const string Table = "identity.\"FgsApiWebhook\"";

    public const string SelectDetailColumns = """
        "Id", "Name", "Description", "EndpointUrl", "AuthenticationType", "AuthenticationValue", "Secret",
        "TimeoutSeconds", "MaximumRetryCount", "LastSuccessfulDeliveryOn", "IsActive"
        """;

    public const string SelectSummaryColumns = """
        "Id", "Name", "Description", "EndpointUrl", "AuthenticationType",
        "TimeoutSeconds", "MaximumRetryCount", "LastSuccessfulDeliveryOn", "IsActive"
        """;

    public const string SelectLookupColumns = """
        "Id", "Name", "EndpointUrl"
        """;

    private static readonly HashSet<string> AllowedSortColumns = new(StringComparer.OrdinalIgnoreCase)
    {
        "Id", "Name", "EndpointUrl", "AuthenticationType", "TimeoutSeconds", "MaximumRetryCount",
        "LastSuccessfulDeliveryOn", "IsActive"
    };

    public static string ResolveOrderBy(string? sortBy, SortDirection direction)
    {
        var dir = direction == SortDirection.Desc ? "DESC" : "ASC";
        if (string.IsNullOrWhiteSpace(sortBy) || !AllowedSortColumns.Contains(sortBy))
        {
            return $"ORDER BY \"Name\" {dir}, \"Id\" {dir}";
        }

        var column = AllowedSortColumns.First(c => c.Equals(sortBy, StringComparison.OrdinalIgnoreCase));
        return $"ORDER BY \"{column}\" {dir}";
    }
}
