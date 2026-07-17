using Fgs.Contracts.Api;
using Fgs.Contracts.Auth;
using Fgs.Security.UserAuth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Common.Identity;
using Fgs.User.Application.Features.Auth.Queries.GetUserAuthProfile;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Queries.GetUserAuthProfile;

public sealed class GetUserAuthProfileQueryHandler(
    IFgsUserProfileResolver profileResolver,
    IUserAuthProfileStore profileStore) : IRequestHandler<GetUserAuthProfileQuery, ApiResponse<UserAuthProfileResultDto>>
{
    public async Task<ApiResponse<UserAuthProfileResultDto>> Handle(
        GetUserAuthProfileQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.EntraObjectId))
        {
            return ApiResponse<UserAuthProfileResultDto>.Fail(
                ["entraObjectId is required."],
                ApiStatusCodes.BadRequest);
        }

        var profile = await profileResolver.ResolveByEntraObjectIdAsync(request.EntraObjectId, cancellationToken);
        if (profile is null)
        {
            return ApiResponse<UserAuthProfileResultDto>.Fail(
                ["User profile was not found."],
                ApiStatusCodes.NotFound);
        }

        var dto = UserAuthProfileMapper.ToDto(profile);
        await profileStore.SetAsync(dto, cancellationToken);

        return ApiResponse<UserAuthProfileResultDto>.Ok(ToResult(dto));
    }

    private static UserAuthProfileResultDto ToResult(UserAuthProfileDto dto) =>
        new(
            dto.UserId,
            dto.Email,
            dto.EntraObjectId,
            dto.TenantId,
            dto.CompanyId,
            dto.IsActive,
            dto.IsDeleted,
            dto.Roles,
            dto.Permissions,
            dto.DataAccess,
            dto.PublicEndpoints);
}
