using System.Text.Json;
using Fgs.Foundation.Caching;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using FluentValidation;

namespace Fgs.User.Application.Features.GridPreferences;

internal static class FgsUserGridPreferenceLimits
{
    public const int MaxCodeLength = 100;

    public const int MaxConfigurationLength = 65_536;
}

internal static class FgsUserGridPreferenceSupport
{
    public const string CacheEntityName = "user-grid-preference";

    private static readonly JsonDocument EmptyConfigurationDocument = JsonDocument.Parse("{}");

    public static JsonElement EmptyConfiguration => EmptyConfigurationDocument.RootElement;

    public static bool TryReadConfiguration(JsonElement configuration, out string json, out string error)
    {
        if (configuration.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            json = string.Empty;
            error = "Grid configuration is required.";
            return false;
        }

        var candidate = configuration.ValueKind == JsonValueKind.String
            ? configuration.GetString()
            : configuration.ValueKind == JsonValueKind.Object
                ? configuration.GetRawText()
                : null;

        if (candidate is null)
        {
            json = string.Empty;
            error = "Grid configuration must be a JSON object.";
            return false;
        }

        if (candidate.Length > FgsUserGridPreferenceLimits.MaxConfigurationLength)
        {
            json = string.Empty;
            error = "Grid configuration must be 65536 characters or fewer.";
            return false;
        }

        try
        {
            using var document = JsonDocument.Parse(candidate);
            if (document.RootElement.ValueKind != JsonValueKind.Object)
            {
                json = string.Empty;
                error = "Grid configuration must be a JSON object.";
                return false;
            }
        }
        catch (JsonException)
        {
            json = string.Empty;
            error = "Grid configuration must be valid JSON.";
            return false;
        }

        json = candidate;
        error = string.Empty;
        return true;
    }

    public static bool TryGetUserId(IFgsUserContext userContext, out Guid userId)
    {
        if (userContext.IsAuthenticated && userContext.UserId is Guid id)
        {
            userId = id;
            return true;
        }

        userId = default;
        return false;
    }

    public static string CacheKey(ITenantContext tenantContext, Guid userId, string gridCode) =>
        CacheKeys.Build(
            tenantContext.TenantId,
            tenantContext.CompanyId,
            CacheEntityName,
            $"{userId:D}:{gridCode}");
}

internal static class FgsUserGridPreferenceRuleExtensions
{
    public static IRuleBuilderOptions<T, string> ValidGridCode<T>(this IRuleBuilder<T, string> rule) =>
        rule
            .Must(code => !string.IsNullOrWhiteSpace(code))
            .WithMessage("Grid code is required.")
            .Must(code => code is null || code.Trim().Length <= FgsUserGridPreferenceLimits.MaxCodeLength)
            .WithMessage("Grid code must be 100 characters or fewer.");
}
