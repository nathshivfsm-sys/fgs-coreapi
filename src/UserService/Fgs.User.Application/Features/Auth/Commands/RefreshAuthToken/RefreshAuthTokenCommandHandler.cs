using Fgs.Contracts.Api;
using Fgs.Persistence.Abstractions;
using Fgs.Security.UserAuth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Common.Identity;
using Fgs.User.Application.Features.Auth.Dtos;
using Fgs.User.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Fgs.User.Application.Features.Auth.Commands.RefreshAuthToken;

public sealed class RefreshAuthTokenCommandValidator : AbstractValidator<RefreshAuthTokenCommand>
{
    public RefreshAuthTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
    }
}

public sealed class RefreshAuthTokenCommandHandler(
    IUnitOfWork unitOfWork,
    IEntraExternalIdService entraService,
    ILoginAuthorizationProfileBuilder profileBuilder,
    IUserAuthProfileStore profileStore) : IRequestHandler<RefreshAuthTokenCommand, ApiResponse<LoginProfileDto>>
{
    public async Task<ApiResponse<LoginProfileDto>> Handle(
        RefreshAuthTokenCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [AuthErrorMessages.RefreshTokenRequired],
                ApiStatusCodes.BadRequest);
        }

        EntraTokenResult tokens;
        try
        {
            tokens = await entraService.RefreshTokenAsync(request.RefreshToken, cancellationToken);
        }
        catch (Exception ex)
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [$"{AuthErrorMessages.RefreshTokenFailed} {ex.Message}"],
                ApiStatusCodes.Unauthorized);
        }

        var user = await unitOfWork.Repository<FgsUser>()
            .FirstOrDefaultIgnoreFiltersAsync(
                u => !u.IsDeleted
                     && u.IsActive
                     && u.EntraObjectId == tokens.ObjectId,
                cancellationToken);

        if (user is null)
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [AuthErrorMessages.UserNotFound],
                ApiStatusCodes.NotFound);
        }

        var profile = await profileBuilder.BuildAsync(user, cancellationToken);
        await profileStore.SetAsync(UserAuthProfileMapper.ToDto(profile), cancellationToken);

        return ApiResponse<LoginProfileDto>.Ok(
            LoginProfileFactory.FromTokensAndProfile(tokens, profile, user.DisplayName));
    }
}
