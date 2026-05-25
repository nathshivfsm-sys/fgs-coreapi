using System.Text.Json;
using Fgs.User.Application.Abstractions.Identity;
using Fgs.User.Application.Abstractions.Messaging;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Auth;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Application.IntegrationEvents;
using Fgs.User.Application.TenantProvisioning;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Application.Features.Auth.Queries.EntraCallback;

public sealed class EntraCallbackQueryHandler : IRequestHandler<EntraCallbackQuery, ApiResponse<EntraCallbackResultDto>>
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    private readonly IUnitOfWork _unitOfWork;
    private readonly IEntraExternalIdService _entraService;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IEmailNormalizer _emailNormalizer;
    private readonly IDateTimeProvider _dateTime;
    private readonly IConfiguration _configuration;
    private readonly IOutboxWriter _outboxWriter;

    public EntraCallbackQueryHandler(
        IUnitOfWork unitOfWork,
        IEntraExternalIdService entraService,
        IJwtTokenService jwtTokenService,
        IEmailNormalizer emailNormalizer,
        IDateTimeProvider dateTime,
        IConfiguration configuration,
        IOutboxWriter outboxWriter)
    {
        _unitOfWork = unitOfWork;
        _entraService = entraService;
        _jwtTokenService = jwtTokenService;
        _emailNormalizer = emailNormalizer;
        _dateTime = dateTime;
        _configuration = configuration;
        _outboxWriter = outboxWriter;
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
                        ?? throw new InvalidOperationException(AuthErrorMessages.InvitationUserNotFound);

                    if (invitation.Status != InvitationStatus.Accepted)
                    {
                        user.EntraObjectId = entraUser.ObjectId;
                        user.UpdatedOn = _dateTime.UtcNow;
                        userRepo.Update(user);

                        invitation.MarkAccepted();
                        invitationRepo.Update(invitation);

                        await EnqueueTenantProvisionRequestedAsync(invitation, ct);
                    }

                    var roleCodes = await ResolveUserRoleCodesAsync(user.Id, ct);

                    var accessToken = _jwtTokenService.CreateToken(user, roleCodes);
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

    private async Task<IReadOnlyList<string>> ResolveUserRoleCodesAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var userRoles = await _unitOfWork.Repository<FgsUserRole>()
            .ListAsync(ur => ur.UserId == userId, cancellationToken);

        if (userRoles.Count == 0)
        {
            return [];
        }

        var gloRoleRepo = _unitOfWork.Repository<GloRole>();
        var fgsRoleRepo = _unitOfWork.Repository<FgsRole>();
        var roleCodes = new List<string>(userRoles.Count);

        foreach (var userRole in userRoles)
        {
            if (userRole.GloRoleId is { } gloRoleId)
            {
                var gloRole = await gloRoleRepo.FirstOrDefaultAsync(r => r.Id == gloRoleId, cancellationToken);
                if (gloRole is not null)
                {
                    roleCodes.Add(gloRole.RoleCode);
                }

                continue;
            }

            if (userRole.FgsRoleId is { } fgsRoleId)
            {
                var fgsRole = await fgsRoleRepo.FirstOrDefaultAsync(r => r.Id == fgsRoleId, cancellationToken);
                if (fgsRole is not null)
                {
                    roleCodes.Add(fgsRole.RoleCode);
                }
            }
        }

        return roleCodes;
    }

    private async Task EnqueueTenantProvisionRequestedAsync(
        FgsInvitation invitation,
        CancellationToken cancellationToken)
    {
        var tenantRepo = _unitOfWork.Repository<FgsTenant>();
        var tenant = await tenantRepo.FirstOrDefaultAsync(t => t.Id == invitation.TenantId, cancellationToken)
            ?? throw new InvalidOperationException(AuthErrorMessages.TenantNotFound);

        var company = await _unitOfWork.Repository<FgsTenantCompany>()
            .FirstOrDefaultAsync(c => c.TenantId == invitation.TenantId, cancellationToken)
            ?? throw new InvalidOperationException(AuthErrorMessages.TenantCompanyNotFound);

        if (tenant.FgsTenantStatusId == TenantStatusIds.Active)
        {
            return;
        }

        tenant.FgsTenantStatusId = TenantStatusIds.Provisioning;
        tenant.UpdatedOn = _dateTime.UtcNow;
        tenantRepo.Update(tenant);

        var correlationId = invitation.Id;
        var provisionEvent = new TenantProvisionRequestedEvent(
            tenant.Id,
            company.CompanyNumber,
            tenant.TenantCode,
            correlationId,
            invitation.UserId);

        await _outboxWriter.EnqueueAsync(
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
