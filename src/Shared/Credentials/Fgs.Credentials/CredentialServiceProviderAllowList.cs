namespace Fgs.Credentials;

/// <summary>
/// Server-side allow-list of credential providers each consumer may receive from
/// <c>/credential/resolved</c>. Mirrors DI <c>RequiredProviders</c> (+ ENTRA for standard APIs).
/// </summary>
public static class CredentialServiceProviderAllowList
{
    private static readonly Dictionary<string, string[]> ProvidersByService =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["fgs-setup-service"] = ["DATABASE", "AWS", "REDIS", "RABBITMQ", "ENTRA_EXTERNAL_ID", "DATADOG"],
            ["fgs-user-service"] = ["DATABASE", "ENTRA_EXTERNAL_ID", "AWS", "REDIS", "RABBITMQ"],
            ["fgs-audit-service"] = ["DATABASE", "ENTRA_EXTERNAL_ID"],
            ["fgs-bff-service"] = ["ENTRA_EXTERNAL_ID", "REDIS"],
            ["fgs-notification-service"] = ["DATABASE", "SENDGRID"],
            ["fgs-file-service"] = ["DATABASE", "AWS", "ENTRA_EXTERNAL_ID"],
            ["fgs-inventory-service"] = ["DATABASE", "RABBITMQ", "ENTRA_EXTERNAL_ID", "REDIS"],
            ["fgs-asset-service"] = ["DATABASE", "ENTRA_EXTERNAL_ID", "REDIS"],
            ["fgs-consumer-service"] = ["RABBITMQ", "REDIS"],
            ["fgs-billing-service"] = ["DATABASE", "ENTRA_EXTERNAL_ID"],
            ["fgs-crm-service"] = ["DATABASE", "ENTRA_EXTERNAL_ID"],
            ["fgs-scheduling-service"] = ["DATABASE", "ENTRA_EXTERNAL_ID"],
            ["fgs-reporting-service"] = ["DATABASE", "ENTRA_EXTERNAL_ID"],
            ["fgs-service-agreement-service"] = ["DATABASE", "ENTRA_EXTERNAL_ID"],
            ["fgs-communication-service"] = ["ENTRA_EXTERNAL_ID"],
            ["fgs-integration-service"] = ["DATABASE", "ENTRA_EXTERNAL_ID"],
        };

    public static bool TryGetRequiredProviders(string? serviceName, out IReadOnlyList<string> providers)
    {
        providers = Array.Empty<string>();
        if (string.IsNullOrWhiteSpace(serviceName))
        {
            return false;
        }

        if (!ProvidersByService.TryGetValue(serviceName.Trim(), out var configured))
        {
            return false;
        }

        providers = configured;
        return true;
    }
}
