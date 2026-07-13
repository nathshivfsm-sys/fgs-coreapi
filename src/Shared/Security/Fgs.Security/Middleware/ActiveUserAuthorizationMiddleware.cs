using Fgs.Contracts.Clients;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Fgs.Security.Authorization;
using Fgs.Security.UserAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Fgs.Security.Middleware;

public sealed class ActiveUserAuthorizationMiddleware(
    RequestDelegate next,
    IOptions<TenantScopeOptions> tenantScopeOptions,
    IOptions<InternalServiceKeyOptions> internalServiceKeyOptions)
{
    public async Task InvokeAsync(
        HttpContext context,
        IFgsUserContext userContext,
        IUserAuthProfileStore profileStore,
        ITenantContextAccessor tenantContextAccessor)
    {
        if (IsInternalServiceRequest(context)
            || !userContext.IsAuthenticated
            || string.IsNullOrWhiteSpace(userContext.EntraObjectId))
        {
            await next(context);
            return;
        }

        var profile = await profileStore.GetOrLoadAsync(userContext.EntraObjectId, context.RequestAborted);
        var result = UserAuthorizationEvaluator.Evaluate(context, profile, tenantScopeOptions.Value);

        if (!result.Success)
        {
            await AuthorizationResponseWriter.WriteAsync(
                context,
                result.StatusCode!.Value,
                result.ErrorMessage!,
                context.RequestAborted);
            return;
        }

        context.Items[UserAuthHttpContextKeys.Profile] = profile!;

        if (result.ValidatedScope is not null)
        {
            tenantContextAccessor.Current = new TenantContext
            {
                TenantId = result.ValidatedScope.TenantId,
                CompanyId = result.ValidatedScope.CompanyId
            };
            context.Items[UserAuthHttpContextKeys.ValidatedScope] = result.ValidatedScope;
        }

        await next(context);
    }

    private bool IsInternalServiceRequest(HttpContext context)
    {
        var configuredKey = internalServiceKeyOptions.Value.InternalServiceKey;
        if (string.IsNullOrWhiteSpace(configuredKey))
        {
            return false;
        }

        var providedKey = context.Request.Headers[InternalServiceHeaders.ServiceKey].FirstOrDefault();
        return string.Equals(providedKey, configuredKey, StringComparison.Ordinal);
    }
}
