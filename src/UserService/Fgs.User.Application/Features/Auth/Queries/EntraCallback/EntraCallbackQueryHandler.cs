using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Application.Features.Auth.Queries.EntraCallback;

public sealed class EntraCallbackQueryHandler : IRequestHandler<EntraCallbackQuery, ApiResponse<EntraCallbackResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEntraExternalIdService _entraService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IEmailNormalizer _emailNormalizer;
    private readonly IDateTimeProvider _dateTime;
    private readonly IConfiguration _configuration;

    public EntraCallbackQueryHandler(
        IUnitOfWork unitOfWork,
        IEntraExternalIdService entraService,
        IJwtTokenService jwtTokenService,
        IEmailNormalizer emailNormalizer,
        IDateTimeProvider dateTime,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _entraService = entraService;
        _jwtTokenService = jwtTokenService;
        _emailNormalizer = emailNormalizer;
        _dateTime = dateTime;
        _configuration = configuration;
    }

    public async Task<ApiResponse<EntraCallbackResultDto>> Handle(
        EntraCallbackQuery request,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.State, out var invitationId))
        {
            return ApiResponse<EntraCallbackResultDto>.Fail(
                [AuthErrorMessages.InvalidOAuthState],
                ApiStatusCodes.BadRequest);
        }

        var redirectUri = _configuration[ConfigurationKeys.EntraExternalId.RedirectUri]
            ?? ApplicationUrlDefaults.EntraCallbackRedirect;

        EntraTokenResult entraUser;
        try
        {
            entraUser = await _entraService.ExchangeCodeAsync(request.Code, redirectUri, cancellationToken);
        }
        catch (Exception)
        {
            return ApiResponse<EntraCallbackResultDto>.Fail(
                [AuthErrorMessages.EntraCodeExchangeFailed],
                ApiStatusCodes.Unauthorized);
        }

        var invitationRepo = _unitOfWork.Repository<FgsInvitation>();
        var invitation = await invitationRepo.GetByIdAsync(invitationId, cancellationToken);
        if (invitation is null)
        {
            return ApiResponse<EntraCallbackResultDto>.Fail(
                [AuthErrorMessages.InvitationNotFound],
                ApiStatusCodes.NotFound);
        }

        if (invitation.ExpiresAtUtc <= _dateTime.UtcNow)
        {
            invitation.MarkExpired();
            invitationRepo.Update(invitation);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<EntraCallbackResultDto>.Fail(
                [AuthErrorMessages.InvitationNotActive],
                ApiStatusCodes.BadRequest);
        }

        var normalizedEntraEmail = _emailNormalizer.Normalize(entraUser.Email);
        var normalizedInviteEmail = _emailNormalizer.Normalize(invitation.Email);
        if (!string.Equals(normalizedEntraEmail, normalizedInviteEmail, StringComparison.Ordinal))
        {
            return ApiResponse<EntraCallbackResultDto>.Fail(
                [AuthErrorMessages.EntraEmailMismatch],
                ApiStatusCodes.BadRequest);
        }

        try
        {
            var result = await _unitOfWork.ExecuteInTransactionAsync(
                async ct =>
                {
                    var userRepo = _unitOfWork.Repository<FgsUser>();
                    var user = await userRepo.GetByIdAsync(invitation.UserId, ct)
                        ?? throw new InvalidOperationException("Invitation user not found.");

                    if (invitation.Status != InvitationStatus.Accepted)
                    {
                        user.EntraObjectId = entraUser.ObjectId;
                        user.UpdatedOn = _dateTime.UtcNow;
                        userRepo.Update(user);

                        invitation.MarkAccepted();
                        invitationRepo.Update(invitation);
                    }
                    var accessToken = _jwtTokenService.CreateToken(user);
                    var dashboardUrl = _configuration[ConfigurationKeys.Application.DashboardUrl]
                        ?? ApplicationUrlDefaults.Dashboard;

                    return new EntraCallbackResultDto(accessToken, dashboardUrl);
                },
                cancellationToken);

            return ApiResponse<EntraCallbackResultDto>.Ok(result);
        }
        catch (Exception)
        {
            return ApiResponse<EntraCallbackResultDto>.Fail(
                [AuthErrorMessages.FinalizeOnboardingFailed],
                ApiStatusCodes.InternalServerError);
        }
    }
}
