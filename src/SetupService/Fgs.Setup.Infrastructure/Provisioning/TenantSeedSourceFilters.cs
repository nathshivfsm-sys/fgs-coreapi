namespace Fgs.Setup.Infrastructure.Provisioning;

internal static class TenantSeedSourceFilters
{
    private static readonly IReadOnlyDictionary<string, string> FiltersBySeedCode =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["ALL_GloRole"] = "\"IsAssignable\" = true AND \"IsActive\" = true"
        };

    public static string? TryGetFilter(string? seedCode) =>
        seedCode is not null
        && FiltersBySeedCode.TryGetValue(seedCode, out var filter)
            ? filter
            : null;
}
