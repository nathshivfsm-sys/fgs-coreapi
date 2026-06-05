using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Invitations;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
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
        var matched = await invitations.FirstOrDefaultAsync(
            i => i.TokenHash == tokenHash,
            cancellationToken);

        if (matched is null)
        {
            return new StartInvitationResult(false, null, InvitationErrorMessages.InvalidToken);
        }

        var redirectUri = configuration[ConfigurationKeys.EntraExternalId.RedirectUri]
            ?? ApplicationUrlDefaults.EntraCallbackRedirect;

        if (matched.Status == InvitationStatus.Accepted)
        {
            var loginUrl = entraService.BuildAuthorizationUrl(matched.Id, redirectUri, matched.Email);
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

        var authorizeUrl = entraService.BuildAuthorizationUrl(matched.Id, redirectUri, matched.Email);
        return new StartInvitationResult(true, authorizeUrl, null);
    }
}
