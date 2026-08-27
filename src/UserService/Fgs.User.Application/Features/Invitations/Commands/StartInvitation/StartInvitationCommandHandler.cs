using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Application.Features.Invitations;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Security;
using Fgs.Foundation.Time;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Application.Features.Invitations.Commands.StartInvitation;

public sealed class StartInvitationCommandHandler(
    IUnitOfWork unitOfWork,
    IInvitationTokenService tokenService,
    IEntraExternalIdService entraService,
    IDateTimeProvider dateTime,
    IConfiguration configuration) : IRequestHandler<StartInvitationCommand, StartInvitationResult>
{
    public async Task<StartInvitationResult> Handle(
        StartInvitationCommand request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            return new StartInvitationResult(false, null, InvitationErrorMessages.TokenRequired);
        }

        var tokenHash = tokenService.HashToken(request.Token);
        var invitations = unitOfWork.Repository<FgsInvitation>();
        var matched = await invitations.FirstOrDefaultIgnoreFiltersAsync(
            i => i.TokenHash == tokenHash,
            cancellationToken);

        if (matched is null)
        {
            return new StartInvitationResult(false, null, InvitationErrorMessages.InvalidToken);
        }

        var redirectUri = ApplicationPublicUrlResolver.ResolveEntraCallbackRedirect(configuration);

        var user = await unitOfWork.Repository<FgsUser>()
            .FirstOrDefaultIgnoreFiltersAsync(u => u.Id == matched.UserId, cancellationToken);
        var userFlow = ResolveUserFlow(user?.AuthenticationMethod ?? AuthenticationMethod.PasswordOrEmailOtp);

        if (matched.Status == InvitationStatus.Accepted)
        {
            // Already verified — send to Entra sign-in (no prompt=create).
            var loginUrl = entraService.BuildAuthorizationUrl(
                matched.Id.ToString(),
                redirectUri,
                matched.Email,
                forceSignup: false,
                userFlow: userFlow);
            return new StartInvitationResult(true, loginUrl, null);
        }

        if (matched.Status != InvitationStatus.Pending)
        {
            return new StartInvitationResult(false, null, InvitationErrorMessages.NotActive);
        }

        if (matched.ExpiresAtUtc <= dateTime.UtcNow)
        {
            matched.MarkExpired();
            invitations.Update(matched);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return new StartInvitationResult(false, null, InvitationErrorMessages.Expired);
        }

        // Pending invite — force Entra signup page so new users don't land on sign-in.
        var authorizeUrl = entraService.BuildAuthorizationUrl(
            matched.Id.ToString(),
            redirectUri,
            matched.Email,
            forceSignup: true,
            userFlow: userFlow);
        return new StartInvitationResult(true, authorizeUrl, null);
    }

    private string ResolveUserFlow(AuthenticationMethod method) =>
        EntraUserFlowResolver.Resolve(
            method,
            configuration[ConfigurationKeys.EntraExternalId.UserFlow],
            configuration[ConfigurationKeys.EntraExternalId.PasswordUserFlow]);
}
