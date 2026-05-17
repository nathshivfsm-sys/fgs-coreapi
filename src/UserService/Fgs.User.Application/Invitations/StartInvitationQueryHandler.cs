using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Application.Invitations;

public sealed class StartInvitationQueryHandler(
    IUnitOfWork unitOfWork,
    IInvitationTokenService tokenService,
    IEntraExternalIdService entraService,
    IDateTimeProvider dateTime,
    IConfiguration configuration) : IRequestHandler<StartInvitationQuery, StartInvitationResult>
{
    public async Task<StartInvitationResult> Handle(
        StartInvitationQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Token))
        {
            return new StartInvitationResult(false, null, "Invitation token is required.");
        }

        var invitations = unitOfWork.Repository<FgsInvitation>();
        var pending = await invitations.ListAsync(
            i => !i.IsDeleted && i.Status == InvitationStatus.Pending,
            cancellationToken);

        var matched = pending.FirstOrDefault(i => tokenService.VerifyToken(request.Token, i.TokenHash));
        if (matched is null)
        {
            return new StartInvitationResult(false, null, "Invalid invitation token.");
        }

        if (matched.ExpiresAtUtc <= dateTime.UtcNow)
        {
            matched.MarkExpired();
            invitations.Update(matched);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return new StartInvitationResult(false, null, "Invitation has expired.");
        }

        var redirectUri = configuration["EntraExternalId:RedirectUri"]
            ?? "https://localhost:5001/api/auth/entra/callback";
        var authorizeUrl = entraService.BuildAuthorizationUrl(matched.Id, redirectUri, matched.Email);
        return new StartInvitationResult(true, authorizeUrl, null);
    }
}
