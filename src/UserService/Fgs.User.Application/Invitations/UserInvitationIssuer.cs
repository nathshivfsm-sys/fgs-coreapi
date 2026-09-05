using System.Text.Json;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Messaging.Abstractions;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Invitations;
using Fgs.User.Application.Abstractions.Security;
using Fgs.Foundation.Time;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Application.Invitations;

public sealed class UserInvitationIssuer(
    IUnitOfWork unitOfWork,
    IInvitationTokenService tokenService,
    IOutboxWriter outboxWriter,
    IDateTimeProvider dateTime,
    IConfiguration configuration) : IUserInvitationIssuer
{
    public async Task<IssuedInvitation> IssueAsync(
        IssueInvitationRequest request,
        CancellationToken cancellationToken = default)
    {
        var now = request.UtcNow ?? dateTime.UtcNow;
        var createdBy = string.IsNullOrWhiteSpace(request.CreatedBy)
            ? "system"
            : request.CreatedBy.Trim();
        var email = request.Email.Trim();
        var invitationRepo = unitOfWork.Repository<FgsInvitation>();

        if (request.SupersedePendingForUser)
        {
            var pending = await invitationRepo.ListAsync(
                i => i.UserId == request.UserId && i.Status == InvitationStatus.Pending,
                cancellationToken);
            foreach (var existing in pending)
            {
                existing.MarkExpired();
                existing.UpdatedOn = now;
                existing.UpdatedBy = createdBy;
                invitationRepo.Update(existing);
            }
        }

        var plainToken = tokenService.GenerateToken();
        var tokenHash = tokenService.HashToken(plainToken);
        var expiryDays = configuration.GetValue(
            ConfigurationKeys.Invitation.ExpiryDays,
            SignupConstants.DefaultInvitationExpiryDays);
        var invitationId = request.InvitationId ?? Guid.NewGuid();
        var expiresAtUtc = now.AddDays(expiryDays);

        var invitation = new FgsInvitation
        {
            Id = invitationId,
            UserId = request.UserId,
            TenantId = request.TenantId,
            Email = email,
            TokenHash = tokenHash,
            Status = InvitationStatus.Pending,
            ExpiresAtUtc = expiresAtUtc,
            CreatedOn = now,
            CreatedBy = createdBy
        };

        await invitationRepo.AddAsync(invitation, cancellationToken);

        var inviteBaseUrl = ApplicationPublicUrlResolver.ResolveInviteBaseUrl(configuration);
        var inviteUrl = $"{inviteBaseUrl.TrimEnd('/')}?token={Uri.EscapeDataString(plainToken)}";
        var expirationHours = Math.Max(
            SignupConstants.MinimumExpirationHours,
            (int)Math.Ceiling((expiresAtUtc - now).TotalHours));

        await EnqueueEmailAsync(request, invitationId, email, inviteUrl, expirationHours, cancellationToken);

        return new IssuedInvitation(invitationId, inviteUrl, expiresAtUtc, expirationHours);
    }

    private async Task EnqueueEmailAsync(
        IssueInvitationRequest request,
        Guid invitationId,
        string email,
        string inviteUrl,
        int expirationHours,
        CancellationToken cancellationToken)
    {
        switch (request.Kind)
        {
            case InvitationEmailKind.CompanyAdminSignup:
            {
                var payload = JsonSerializer.Serialize(new CompanySignupInviteEmailEvent(
                    request.TenantId,
                    request.CompanyId,
                    request.UserId,
                    invitationId,
                    email,
                    CommunicationTemplateCodes.CompanyAdminInvitation,
                    request.DisplayName,
                    PlatformName: string.Empty,
                    inviteUrl,
                    expirationHours.ToString(),
                    SupportEmail: string.Empty));

                await outboxWriter.EnqueueAsync(
                    IntegrationEventTypes.CompanySignupInviteEmail,
                    payload,
                    correlationId: invitationId,
                    tenantId: request.TenantId,
                    companyId: request.CompanyId,
                    aggregateType: IntegrationEventTypes.AggregateTypes.Invitation,
                    aggregateId: invitationId.ToString(),
                    exchangeName: IntegrationEventExchanges.UserEvents,
                    routingKey: IntegrationEventRoutingKeys.CompanySignupInviteEmail,
                    createdBy: SignupConstants.ToGloCreatedBy(request.CreatedBy),
                    cancellationToken: cancellationToken);
                break;
            }
            case InvitationEmailKind.UserInvited:
            {
                var payload = JsonSerializer.Serialize(new UserInvitedEvent(
                    request.TenantId,
                    request.CompanyId,
                    request.UserId,
                    email,
                    request.DisplayName,
                    inviteUrl,
                    request.CompanyName ?? string.Empty));

                await outboxWriter.EnqueueAsync(
                    IntegrationEventTypes.UserInvited,
                    payload,
                    correlationId: invitationId,
                    tenantId: request.TenantId,
                    companyId: request.CompanyId,
                    aggregateType: IntegrationEventTypes.AggregateTypes.Invitation,
                    aggregateId: invitationId.ToString(),
                    exchangeName: IntegrationEventExchanges.UserEvents,
                    routingKey: IntegrationEventRoutingKeys.UserInvited,
                    createdBy: SignupConstants.ToGloCreatedBy(request.CreatedBy),
                    cancellationToken: cancellationToken);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException(nameof(request.Kind), request.Kind, "Unsupported invitation email kind.");
        }
    }
}
