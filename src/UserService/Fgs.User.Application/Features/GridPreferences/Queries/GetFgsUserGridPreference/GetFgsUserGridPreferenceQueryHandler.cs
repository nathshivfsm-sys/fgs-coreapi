using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.GridPreferences;
using Fgs.User.Application.Features.GridPreferences;
using Fgs.User.Application.Features.GridPreferences.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.GridPreferences.Queries.GetFgsUserGridPreference;

public sealed class GetFgsUserGridPreferenceQueryHandler(
    IFgsUserGridPreferenceStore store,
    ICacheService cache,
    ITenantContext tenantContext,
    IFgsUserContext userContext)
    : IRequestHandler<GetFgsUserGridPreferenceQuery, ApiResponse<FgsUserGridPreferenceDto>>
{
    public async Task<ApiResponse<FgsUserGridPreferenceDto>> Handle(
        GetFgsUserGridPreferenceQuery request,
        CancellationToken cancellationToken)
    {
        if (!FgsUserGridPreferenceSupport.TryGetUserId(userContext, out var userId))
        {
            return ApiResponse<FgsUserGridPreferenceDto>.Fail(
                ["A signed-in user is required."],
                ApiStatusCodes.Unauthorized);
        }

        var gridCode = request.GridCode.Trim();
        var cacheKey = FgsUserGridPreferenceSupport.CacheKey(tenantContext, userId, gridCode);
        var preference = await cache.GetOrSetAsync(
            cacheKey,
            () => store.GetAsync(gridCode, cancellationToken),
            cancellationToken: cancellationToken);

        return ApiResponse<FgsUserGridPreferenceDto>.Ok(
            preference ?? new FgsUserGridPreferenceDto(gridCode, []));
    }
}
