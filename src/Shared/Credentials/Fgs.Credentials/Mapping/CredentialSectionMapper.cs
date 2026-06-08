namespace Fgs.Credentials.Mapping;

internal static class CredentialSectionMapper
{
    private static readonly (string Prefix, string Section)[] ProviderSections =
    [
        ("Global:SENDGRID:", "SendGrid:"),
        ("Global:ENTRA_EXTERNAL_ID:", "EntraExternalId:"),
        ("Global:AWS:", "AwsCredentials:"),
        ("Global:RABBITMQ:", "RabbitMq:"),
        ("Global:TWILIO:", "Twilio:"),
        ("Global:STRIPE:", "Stripe:"),
        ("Global:SMTP:", "Smtp:"),
        ("Global:JWT:", "Jwt:"),
        ("Global:WEBHOOK:", "Webhook:"),
        ("Global:FIREBASE:", "Firebase:")
    ];

    public static bool TryMap(string credentialKey, out string configurationKey, out string? value)
    {
        configurationKey = string.Empty;
        value = null;

        if (TryMapDatabase(credentialKey, out configurationKey))
        {
            return true;
        }

        foreach (var (prefix, section) in ProviderSections)
        {
            if (!credentialKey.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var property = credentialKey[prefix.Length..];
            configurationKey = section + MapPropertyName(prefix, property);
            return true;
        }

        return false;
    }

    public static bool TryResolveValue(
        string credentialKey,
        string configurationKey,
        IReadOnlyDictionary<string, string> values,
        out string? value)
    {
        value = null;

        if (configurationKey.StartsWith("ConnectionStrings:", StringComparison.OrdinalIgnoreCase))
        {
            var connectionName = configurationKey["ConnectionStrings:".Length..];
            if (values.TryGetValue($"Global:DATABASE:{connectionName}", out var direct))
            {
                value = direct;
                return true;
            }

            if (values.TryGetValue("Global:DATABASE:ConnectionStringName", out var named)
                && string.Equals(named, connectionName, StringComparison.OrdinalIgnoreCase)
                && values.TryGetValue("Global:DATABASE:ConnectionString", out var namedConnection))
            {
                value = namedConnection;
                return true;
            }

            return false;
        }

        if (!TryMap(credentialKey, out var mappedKey, out _))
        {
            return false;
        }

        if (!string.Equals(mappedKey, configurationKey, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        value = values.TryGetValue(credentialKey, out var stored) ? stored : null;
        return value is not null;
    }

    private static bool TryMapDatabase(string credentialKey, out string configurationKey)
    {
        configurationKey = string.Empty;
        const string databasePrefix = "Global:DATABASE:";

        if (!credentialKey.StartsWith(databasePrefix, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var suffix = credentialKey[databasePrefix.Length..];
        if (string.Equals(suffix, "ConnectionStringName", StringComparison.OrdinalIgnoreCase)
            || string.Equals(suffix, "ConnectionString", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        configurationKey = $"ConnectionStrings:{suffix}";
        return true;
    }

    private static string MapPropertyName(string prefix, string property) =>
        prefix.Equals("Global:RABBITMQ:", StringComparison.OrdinalIgnoreCase)
            ? property switch
            {
                "Username" => "UserName",
                _ => property
            }
            : property;
}
