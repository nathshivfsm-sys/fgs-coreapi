using Fgs.Contracts.Api;
using Fgs.Contracts.Auth;
using Fgs.Persistence.Abstractions;
using Fgs.Security.UserAuth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Domain.Entities;
using Fgs.User.Application.Common.Identity;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Application.Features.Auth.Commands.EntraLoginCallback;

public sealed class EntraLoginCallbackCommandHandler(
    IUnitOfWork unitOfWork,
    IEntraExternalIdService entraService,
    IEmailNormalizer emailNormalizer,
    IDateTimeProvider dateTime,
    IConfiguration configuration,
    IUserAuthProfileStore profileStore,
    IUserRoleCodesReadQuery roleCodesReadQuery) : IRequestHandler<EntraLoginCallbackCommand, ApiResponse<EntraLoginCallbackResultDto>>
{
    public async Task<ApiResponse<EntraLoginCallbackResultDto>> Handle(
        EntraLoginCallbackCommand request,
        CancellationToken cancellationToken)
    {
        if (!TryParseUserState(request.State, out var userId))
        {
            return ApiResponse<EntraLoginCallbackResultDto>.Fail(
                [AuthErrorMessages.InvalidLoginOAuthState],
                ApiStatusCodes.BadRequest);
        }

        var redirectUri = configuration[ConfigurationKeys.EntraExternalId.RedirectUri]
            ?? ApplicationUrlDefaults.EntraCallbackRedirect;

        EntraTokenResult entraUser;
        try
        {
            entraUser = await entraService.ExchangeCodeAsync(request.Code, redirectUri, cancellationToken);
        }
        catch (Exception)
        {
            return ApiResponse<EntraLoginCallbackResultDto>.Fail(
                [AuthErrorMessages.EntraCodeExchangeFailed],
                ApiStatusCodes.Unauthorized);
        }

        var userRepo = unitOfWork.Repository<FgsUser>();
        var user = await userRepo.GetByIdAsync(userId, cancellationToken);
        if (user is null || !user.IsActive || user.IsDeleted)
        {
            return ApiResponse<EntraLoginCallbackResultDto>.Fail(
                [AuthErrorMessages.UserNotActive],
                ApiStatusCodes.Forbidden);
        }

        var normalizedEntraEmail = emailNormalizer.Normalize(entraUser.Email);
        var normalizedUserEmail = emailNormalizer.Normalize(user.Email);
        if (!string.Equals(normalizedEntraEmail, normalizedUserEmail, StringComparison.Ordinal))
        {
            return ApiResponse<EntraLoginCallbackResultDto>.Fail(
                [AuthErrorMessages.EntraEmailMismatch],
                ApiStatusCodes.BadRequest);
        }

        if (string.IsNullOrWhiteSpace(user.EntraObjectId))
        {
            user.EntraObjectId = entraUser.ObjectId;
            user.UpdatedOn = dateTime.UtcNow;
            userRepo.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        await profileStore.InvalidateAsync(user.Id, user.EntraObjectId, cancellationToken);

        var roles = await roleCodesReadQuery.GetRoleCodesForUserAsync(user.Id, cancellationToken);
        var profile = new FgsUserProfile(
            user.Id,
            user.Email,
            entraUser.ObjectId,
            user.TenantId,
            user.CompanyId,
            user.IsActive,
            user.IsDeleted,
            roles);

        await profileStore.SetAsync(UserAuthProfileMapper.ToDto(profile), cancellationToken);

        var dashboardUrl = configuration[ConfigurationKeys.Application.DashboardUrl]
            ?? ApplicationUrlDefaults.Dashboard;

        return ApiResponse<EntraLoginCallbackResultDto>.Ok(
            new EntraLoginCallbackResultDto(entraUser.AccessToken, dashboardUrl));
    }

    private static bool TryParseUserState(string state, out Guid userId)
    {
        userId = default;
        if (!state.StartsWith(OAuthStatePrefixes.UserLogin, StringComparison.Ordinal))
        {
            return false;
        }

        return Guid.TryParse(state[OAuthStatePrefixes.UserLogin.Length..], out userId);
    }
}
