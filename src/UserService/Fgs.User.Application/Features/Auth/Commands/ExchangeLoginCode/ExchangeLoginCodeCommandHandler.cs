using Fgs.Contracts.Api;
using Fgs.Persistence.Abstractions;
using Fgs.Security.UserAuth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Common;
using Fgs.User.Application.Common.Identity;
using Fgs.User.Application.Features.Auth.Dtos;
using Fgs.User.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Commands.ExchangeLoginCode;

public sealed class ExchangeLoginCodeCommandValidator : AbstractValidator<ExchangeLoginCodeCommand>
{
    public ExchangeLoginCodeCommandValidator()
    {
        RuleFor(x => x.Code).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
    }
}

public sealed class ExchangeLoginCodeCommandHandler(
    IUnitOfWork unitOfWork,
    IEntraExternalIdService entraService,
    IEmailNormalizer emailNormalizer,
    ILoginPkceStore loginPkceStore,
    ILoginAuthorizationProfileBuilder profileBuilder,
    IUserAuthProfileStore profileStore) : IRequestHandler<ExchangeLoginCodeCommand, ApiResponse<LoginProfileDto>>
{
    public async Task<ApiResponse<LoginProfileDto>> Handle(
        ExchangeLoginCodeCommand request,
        CancellationToken cancellationToken)
    {
        if (!TryParseUserState(request.State, out var userId))
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [AuthErrorMessages.InvalidLoginOAuthState],
                ApiStatusCodes.BadRequest);
        }

        var pkceState = await loginPkceStore.TakeAsync(request.State, cancellationToken);
        if (pkceState is null || pkceState.UserId != userId)
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [AuthErrorMessages.PkceStateExpired],
                ApiStatusCodes.BadRequest);
        }

        EntraTokenResult entraUser;
        try
        {
            entraUser = await entraService.ExchangeLoginCodeAsync(
                request.Code,
                pkceState.RedirectUri,
                pkceState.CodeVerifier,
                cancellationToken);
        }
        catch (Exception)
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [AuthErrorMessages.EntraCodeExchangeFailed],
                ApiStatusCodes.Unauthorized);
        }

        var userRepo = unitOfWork.Repository<FgsUser>();
        var user = await userRepo.GetByIdAsync(userId, cancellationToken);
        if (user is null || !user.IsActive || user.IsDeleted)
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [AuthErrorMessages.UserNotActive],
                ApiStatusCodes.Forbidden);
        }

        var normalizedEntraEmail = emailNormalizer.Normalize(entraUser.Email);
        var normalizedUserEmail = emailNormalizer.Normalize(user.Email);
        if (!string.Equals(normalizedEntraEmail, normalizedUserEmail, StringComparison.Ordinal))
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [AuthErrorMessages.EntraEmailMismatch],
                ApiStatusCodes.BadRequest);
        }

        if (string.IsNullOrWhiteSpace(user.EntraObjectId))
        {
            user.EntraObjectId = entraUser.ObjectId;
            userRepo.Update(user);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
        else if (!string.Equals(user.EntraObjectId, entraUser.ObjectId, StringComparison.OrdinalIgnoreCase))
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [AuthErrorMessages.EntraEmailMismatch],
                ApiStatusCodes.BadRequest);
        }

        var profile = await profileBuilder.BuildAsync(user, cancellationToken);
        await profileStore.InvalidateAsync(user.Id, user.EntraObjectId, cancellationToken);
        await profileStore.SetAsync(UserAuthProfileMapper.ToDto(profile), cancellationToken);

        return ApiResponse<LoginProfileDto>.Ok(
            LoginProfileFactory.FromTokensAndProfile(entraUser, profile, user.DisplayName));
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
