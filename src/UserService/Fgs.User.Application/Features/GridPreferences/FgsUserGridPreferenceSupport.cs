using Fgs.Foundation.Caching;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using FluentValidation;

namespace Fgs.User.Application.Features.GridPreferences;

internal static class FgsUserGridPreferenceLimits
{
    public const int MaxCodeLength = 100;

    public const int MaxColumnCount = 100;
}

internal static class FgsUserGridPreferenceSupport
{
    public const string CacheEntityName = "user-grid-preference";

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
