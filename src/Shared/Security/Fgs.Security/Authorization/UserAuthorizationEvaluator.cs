using Fgs.Contracts.Auth;
using Fgs.Security.Constants;
using Fgs.Security.Services;
using Microsoft.AspNetCore.Http;

namespace Fgs.Security.Authorization;

public static class UserAuthorizationEvaluator
{
    public static UserAuthorizationResult Evaluate(
        HttpContext context,
        UserAuthProfileDto? profile,
        TenantScopeOptions options)
    {
        if (profile is null)
        {
            return UserAuthorizationResult.Fail(
                StatusCodes.Status403Forbidden,
                UserAuthorizationMessages.ProfileNotFound);
        }

        if (!profile.IsActive)
        {
            return UserAuthorizationResult.Fail(
                StatusCodes.Status403Forbidden,
                UserAuthorizationMessages.UserInactive);
        }

        if (profile.IsDeleted)
        {
            return UserAuthorizationResult.Fail(
                StatusCodes.Status403Forbidden,
                UserAuthorizationMessages.UserDeleted);
        }

        if (ShouldSkipScopeValidation(context, options))
        {
            return UserAuthorizationResult.Ok();
        }

        var scopeResult = ResolveRequestedScope(context);
        if (!scopeResult.Success)
        {
            return scopeResult;
        }

        var (tenantId, companyId) = scopeResult.Values;

        if (!tenantId.HasValue)
        {
            return UserAuthorizationResult.Fail(
                StatusCodes.Status400BadRequest,
                UserAuthorizationMessages.TenantScopeMissing);
        }

        if (tenantId.Value != profile.TenantId)
        {
            return UserAuthorizationResult.Fail(
                StatusCodes.Status403Forbidden,
                UserAuthorizationMessages.TenantMismatch);
        }

        var isTenantAdmin = profile.IsInRole(FgsRoleCodes.TenantAdmin);
        var effectiveCompanyId = companyId ?? (isTenantAdmin ? null : profile.CompanyId);

        if (!effectiveCompanyId.HasValue)
        {
            return UserAuthorizationResult.Fail(
                StatusCodes.Status400BadRequest,
                UserAuthorizationMessages.CompanyScopeMissing);
        }

        if (!isTenantAdmin && effectiveCompanyId.Value != profile.CompanyId)
        {
            return UserAuthorizationResult.Fail(
                StatusCodes.Status403Forbidden,
                UserAuthorizationMessages.CompanyMismatch);
        }

        return UserAuthorizationResult.Ok(new ValidatedUserScope(tenantId.Value, effectiveCompanyId.Value));
    }

    private static bool ShouldSkipScopeValidation(HttpContext context, TenantScopeOptions options)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        return options.SkipPathPrefixes.Any(prefix =>
            path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
    }

    private static ScopeResolutionResult ResolveRequestedScope(HttpContext context)
    {
        var (headerTenant, headerCompany) = FgsRequestAuthContext.ExtractTenantScope(context);
        var routeTenant = TryGetRouteLong(context, "tenantId");
        var routeCompany = TryGetRouteLong(context, "companyId");
        var queryTenant = TryGetQueryLong(context, "tenantId");
        var queryCompany = TryGetQueryLong(context, "companyId");

        var tenantConflict = DetectConflict(headerTenant, routeTenant, queryTenant);
        if (tenantConflict.HasValue)
        {
            return ScopeResolutionResult.FromFailure(
                StatusCodes.Status403Forbidden,
                tenantConflict.Value
                    ? UserAuthorizationMessages.RouteTenantMismatch
                    : UserAuthorizationMessages.TenantMismatch);
        }

        var companyConflict = DetectConflict(headerCompany, routeCompany, queryCompany);
        if (companyConflict.HasValue)
        {
            return ScopeResolutionResult.FromFailure(
                StatusCodes.Status403Forbidden,
                companyConflict.Value
                    ? UserAuthorizationMessages.RouteCompanyMismatch
                    : UserAuthorizationMessages.CompanyMismatch);
        }

        return ScopeResolutionResult.FromValues(
            headerTenant ?? routeTenant ?? queryTenant,
            headerCompany ?? routeCompany ?? queryCompany);
    }

    private static bool? DetectConflict(long? header, long? route, long? query)
    {
        var values = new[] { header, route, query }.Where(v => v.HasValue).Select(v => v!.Value).Distinct().ToList();
        if (values.Count <= 1)
        {
            return null;
        }

        return route.HasValue || query.HasValue;
    }

    private static long? TryGetRouteLong(HttpContext context, string key) =>
        context.Request.RouteValues.TryGetValue(key, out var value)
        && long.TryParse(value?.ToString(), out var parsed)
            ? parsed
            : null;

    private static long? TryGetQueryLong(HttpContext context, string key) =>
        long.TryParse(context.Request.Query[key].FirstOrDefault(), out var parsed)
            ? parsed
            : null;

    private sealed record ScopeResolutionResult(
        bool Success,
        long? TenantId,
        long? CompanyId,
        int? StatusCode,
        string? ErrorMessage)
    {
        public (long? TenantId, long? CompanyId) Values => (TenantId, CompanyId);

        public static ScopeResolutionResult FromValues(long? tenantId, long? companyId) =>
            new(true, tenantId, companyId, null, null);

        public static ScopeResolutionResult FromFailure(int statusCode, string errorMessage) =>
            new(false, null, null, statusCode, errorMessage);

        public static implicit operator UserAuthorizationResult(ScopeResolutionResult result) =>
            result.Success
                ? UserAuthorizationResult.Ok()
                : UserAuthorizationResult.Fail(result.StatusCode!.Value, result.ErrorMessage!);
    }
}
