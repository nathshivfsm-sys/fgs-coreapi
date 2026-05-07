using MediatR;
using Microsoft.Extensions.Logging;
using UserService.Application.Common.Abstractions;
using UserService.Application.Common.Models;
using UserService.Application.Common.Persistence;
using UserService.Domain.Entities;
using UserService.Domain.IntegrationEvents;

namespace UserService.Application.Signup.CreateCompanySignup;

public sealed class CreateCompanySignupCommandHandler
    : IRequestHandler<CreateCompanySignupCommand, ApiResponse<CompanySignupResponse>>
{
    private const short DefaultSubsidiaryCompanyId = 1;
    private static readonly TimeSpan InviteTtl = TimeSpan.FromDays(7);

    private readonly IUnitOfWork _unitOfWork;
    private readonly IIntegrationEventPublisher _eventPublisher;
    private readonly IInviteTokenService _inviteTokens;
    private readonly ILogger<CreateCompanySignupCommandHandler> _logger;

    public CreateCompanySignupCommandHandler(
        IUnitOfWork unitOfWork,
        IIntegrationEventPublisher eventPublisher,
        IInviteTokenService inviteTokens,
        ILogger<CreateCompanySignupCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _eventPublisher = eventPublisher;
        _inviteTokens = inviteTokens;
        _logger = logger;
    }

    public async Task<ApiResponse<CompanySignupResponse>> Handle(
        CreateCompanySignupCommand request,
        CancellationToken cancellationToken)
    {
        var email = request.AdminEmail.Trim();
        var companyName = request.CompanyName.Trim();
        var displayName = string.IsNullOrWhiteSpace(request.AdminDisplayName)
            ? email.Split('@')[0]
            : request.AdminDisplayName.Trim();

        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var transactionCommitted = false;

        Tenant tenant;
        User user;
        Invite invite;
        string plainInviteToken;

        try
        {
            tenant = Tenant.Create(companyName);
            var subsidiary = TenantCompany.Create(tenant.Id, DefaultSubsidiaryCompanyId, companyName);
            user = User.CreateAdmin(tenant.Id, DefaultSubsidiaryCompanyId, email, displayName);

            _unitOfWork.Repository<Tenant>().Add(tenant);
            _unitOfWork.Repository<TenantCompany>().Add(subsidiary);
            _unitOfWork.Repository<User>().Add(user);

            (plainInviteToken, var tokenHash) = _inviteTokens.CreateTokenWithHash();
            invite = Invite.CreatePending(
                tenant.Id,
                user.Id,
                email,
                DefaultSubsidiaryCompanyId,
                tokenHash,
                DateTimeOffset.UtcNow.Add(InviteTtl));

            _unitOfWork.Repository<Invite>().Add(invite);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            transactionCommitted = true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Company signup failed for {Email}", email);
            if (!transactionCommitted)
                await transaction.RollbackAsync(cancellationToken);

            return ApiResponse<CompanySignupResponse>.Fail(
                500,
                "Signup could not be completed.");
        }

        try
        {
            await _eventPublisher.PublishAdminUserInviteCreatedAsync(
                new AdminUserInviteCreatedEvent(companyName, email, plainInviteToken),
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(
                ex,
                "Failed to publish {Event} after successful DB commit for {Email}. Reconciliation may be required.",
                nameof(AdminUserInviteCreatedEvent),
                email);

            return ApiResponse<CompanySignupResponse>.Fail(
                503,
                "Account was created but the invite notification could not be queued. Please contact support.");
        }

        var dto = new CompanySignupResponse(tenant.Id, user.Id, invite.Id);
        return ApiResponse<CompanySignupResponse>.Ok(dto);
    }
}
