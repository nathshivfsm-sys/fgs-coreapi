using Fgs.Contracts.Api;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy;
using Fgs.Security.Abstractions;
using Fgs.User.Application.Abstractions.GridPreferences;
using Fgs.User.Application.Features.GridPreferences;
using Fgs.User.Application.Features.GridPreferences.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.GridPreferences.Commands.SaveFgsUserGridPreference;

public sealed class SaveFgsUserGridPreferenceCommandHandler(
    IFgsUserGridPreferenceStore store,
    ICacheService cache,
    ITenantContext tenantContext,
    IFgsUserContext userContext)
    : IRequestHandler<SaveFgsUserGridPreferenceCommand, ApiResponse<FgsUserGridPreferenceDto>>
{
    public async Task<ApiResponse<FgsUserGridPreferenceDto>> Handle(
        SaveFgsUserGridPreferenceCommand request,
        CancellationToken cancellationToken)
    {
        if (!FgsUserGridPreferenceSupport.TryGetUserId(userContext, out var userId))
        {
            return ApiResponse<FgsUserGridPreferenceDto>.Fail(
                ["A signed-in user is required."],
                ApiStatusCodes.Unauthorized);
        }

        if (!FgsUserGridPreferenceSupport.TryReadConfiguration(request.GridConfiguration, out var json, out var error))
        {
            return ApiResponse<FgsUserGridPreferenceDto>.Fail([error], ApiStatusCodes.BadRequest);
        }

        var gridCode = request.GridCode.Trim();
        var saved = await store.UpsertAsync(gridCode, json, cancellationToken);
        var cacheKey = FgsUserGridPreferenceSupport.CacheKey(tenantContext, userId, gridCode);
        await cache.SetAsync(cacheKey, saved, cancellationToken: cancellationToken);
        return ApiResponse<FgsUserGridPreferenceDto>.Ok(saved);
    }
}
