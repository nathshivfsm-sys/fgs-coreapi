using System.Text.Json;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Contracts.IntegrationEvents;
using Fgs.Foundation.Time;
using Fgs.Messaging.Abstractions;
using Fgs.Persistence.Abstractions;
using Fgs.Security.UserAuth;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Common;
using Fgs.User.Application.Common.Identity;
using Fgs.User.Application.Features.Auth.Dtos;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
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
    IUserAuthProfileStore profileStore,
    IDateTimeProvider dateTime,
    IOutboxWriter outboxWriter) : IRequestHandler<ExchangeLoginCodeCommand, ApiResponse<LoginProfileDto>>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public async Task<ApiResponse<LoginProfileDto>> Handle(
        ExchangeLoginCodeCommand request,
        CancellationToken cancellationToken)
    {
        if (OAuthStatePrefixes.TryParseUserLoginState(request.State, out var loginUserId))
        {
            return await HandleLoginAsync(request, loginUserId, cancellationToken);
        }

        if (Guid.TryParse(request.State, out var invitationId))
        {
            return await HandleInvitationAsync(request, invitationId, cancellationToken);
        }

        return ApiResponse<LoginProfileDto>.Fail(
            [AuthErrorMessages.InvalidOAuthState],
            ApiStatusCodes.BadRequest);
    }

    private async Task<ApiResponse<LoginProfileDto>> HandleLoginAsync(
        ExchangeLoginCodeCommand request,
        Guid userId,
        CancellationToken cancellationToken)
    {
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
                cancellationToken,
                pkceState.UserFlow);
        }
        catch (Exception ex)
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [$"{AuthErrorMessages.EntraCodeExchangeFailed} {ex.Message}"],
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

    private async Task<ApiResponse<LoginProfileDto>> HandleInvitationAsync(
        ExchangeLoginCodeCommand request,
        Guid invitationId,
        CancellationToken cancellationToken)
    {
        var pkceState = await loginPkceStore.TakeAsync(request.State, cancellationToken);
        if (pkceState is null)
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
                cancellationToken,
                pkceState.UserFlow);
        }
        catch (Exception ex)
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [$"{AuthErrorMessages.EntraCodeExchangeFailed} {ex.Message}"],
                ApiStatusCodes.Unauthorized);
        }

        var invitationRepo = unitOfWork.Repository<FgsInvitation>();
        var invitation = await invitationRepo.GetByIdAsync(invitationId, cancellationToken);
        if (invitation is null)
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [AuthErrorMessages.InvitationNotFound],
                ApiStatusCodes.NotFound);
        }

        if (pkceState.UserId != invitation.UserId)
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [AuthErrorMessages.PkceStateExpired],
                ApiStatusCodes.BadRequest);
        }

        if (invitation.ExpiresAtUtc <= dateTime.UtcNow)
        {
            invitation.MarkExpired();
            invitationRepo.Update(invitation);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResponse<LoginProfileDto>.Fail(
                [AuthErrorMessages.InvitationNotActive],
                ApiStatusCodes.BadRequest);
        }

        var normalizedEntraEmail = emailNormalizer.Normalize(entraUser.Email);
        var normalizedInviteEmail = emailNormalizer.Normalize(invitation.Email);
        if (!string.Equals(normalizedEntraEmail, normalizedInviteEmail, StringComparison.Ordinal))
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [AuthErrorMessages.EntraEmailMismatch],
                ApiStatusCodes.BadRequest);
        }

        try
        {
            return await unitOfWork.ExecuteInTransactionAsync(
                async ct =>
                {
                    var userRepo = unitOfWork.Repository<FgsUser>();
                    var user = await userRepo.GetByIdAsync(invitation.UserId, ct)
                        ?? throw new InvalidOperationException(AuthErrorMessages.InvitationUserNotFound);

                    if (invitation.Status != InvitationStatus.Accepted)
                    {
                        await profileStore.InvalidateAsync(user.Id, user.EntraObjectId, ct);

                        user.EntraObjectId = entraUser.ObjectId;
                        user.UpdatedOn = dateTime.UtcNow;
                        userRepo.Update(user);

                        invitation.MarkAccepted();
                        invitationRepo.Update(invitation);

                        await EnqueueTenantProvisionRequestedAsync(invitation, ct);
                    }
                    else if (string.IsNullOrWhiteSpace(user.EntraObjectId))
                    {
                        user.EntraObjectId = entraUser.ObjectId;
                        user.UpdatedOn = dateTime.UtcNow;
                        userRepo.Update(user);
                    }

                    var profile = await profileBuilder.BuildAsync(user, ct);
                    await profileStore.InvalidateAsync(user.Id, user.EntraObjectId, ct);
                    await profileStore.SetAsync(UserAuthProfileMapper.ToDto(profile), ct);

                    return ApiResponse<LoginProfileDto>.Ok(
                        LoginProfileFactory.FromTokensAndProfile(entraUser, profile, user.DisplayName));
                },
                cancellationToken);
        }
        catch (Exception ex)
        {
            return ApiResponse<LoginProfileDto>.Fail(
                [$"{AuthErrorMessages.FinalizeOnboardingFailed} {ex.Message}"],
                ApiStatusCodes.InternalServerError);
        }
    }

    private async Task EnqueueTenantProvisionRequestedAsync(
        FgsInvitation invitation,
        CancellationToken cancellationToken)
    {
        var tenantRepo = unitOfWork.Repository<FgsTenant>();
        var tenant = await tenantRepo.FirstOrDefaultAsync(t => t.Id == invitation.TenantId, cancellationToken)
            ?? throw new InvalidOperationException(AuthErrorMessages.TenantNotFound);

        var company = await unitOfWork.Repository<FgsTenantCompany>()
            .FirstOrDefaultAsync(c => c.TenantId == invitation.TenantId, cancellationToken)
            ?? throw new InvalidOperationException(AuthErrorMessages.TenantCompanyNotFound);

        if (tenant.FgsTenantStatusId == TenantStatusIds.Active)
        {
            return;
        }

        tenant.FgsTenantStatusId = TenantStatusIds.Provisioning;
        tenant.UpdatedOn = dateTime.UtcNow;
        tenantRepo.Update(tenant);

        var correlationId = invitation.Id;
        var provisionEvent = new TenantProvisionRequestedEvent(
            tenant.Id,
            company.CompanyNumber,
            tenant.TenantCode,
            correlationId,
            invitation.UserId);

        await outboxWriter.EnqueueAsync(
            IntegrationEventTypes.TenantProvisionRequested,
            JsonSerializer.Serialize(provisionEvent, JsonOptions),
            correlationId,
            tenantId: tenant.Id,
            companyId: company.CompanyNumber,
            aggregateType: IntegrationEventTypes.AggregateTypes.Tenant,
            aggregateId: tenant.Id.ToString(),
            exchangeName: IntegrationEventExchanges.TenantEvents,
            routingKey: IntegrationEventRoutingKeys.TenantProvisionRequested,
            createdBy: SignupConstants.ToGloCreatedBy(tenant.CreatedBy),
            cancellationToken: cancellationToken);
    }
}
