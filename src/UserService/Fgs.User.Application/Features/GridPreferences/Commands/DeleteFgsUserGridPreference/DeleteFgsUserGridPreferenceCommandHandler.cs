using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.GridPreferences;
using Fgs.User.Application.Features.GridPreferences;
using MediatR;

namespace Fgs.User.Application.Features.GridPreferences.Commands.DeleteFgsUserGridPreference;

public sealed class DeleteFgsUserGridPreferenceCommandHandler(
    IFgsUserGridPreferenceStore store,
    ICacheService cache,
    ITenantContext tenantContext,
    IFgsUserContext userContext)
    : IRequestHandler<DeleteFgsUserGridPreferenceCommand, ApiResponse<object>>
{
    public async Task<ApiResponse<object>> Handle(
        DeleteFgsUserGridPreferenceCommand request,
        CancellationToken cancellationToken)
    {
        if (!FgsUserGridPreferenceSupport.TryGetUserId(userContext, out var userId))
        {
            return ApiResponse<object>.Fail(
                ["A signed-in user is required."],
                ApiStatusCodes.Unauthorized);
        }

        var gridCode = request.GridCode.Trim();
        await store.DeleteAsync(gridCode, cancellationToken);
        await cache.RemoveAsync(
            FgsUserGridPreferenceSupport.CacheKey(tenantContext, userId, gridCode),
            cancellationToken);
        return ApiResponse<object>.Ok(new object());
    }
}
