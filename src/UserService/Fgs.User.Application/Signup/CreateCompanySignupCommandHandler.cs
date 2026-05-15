using System.Text.Json;
using Fgs.User.Application.Abstractions.Messaging;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.User.Application.Common;
using Fgs.User.Application.IntegrationEvents;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Application.Signup;

public sealed class CreateCompanySignupCommandHandler
    : IRequestHandler<CreateCompanySignupCommand, ApiResponse<CompanySignupResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailNormalizer _emailNormalizer;
    private readonly IInvitationTokenService _tokenService;
    private readonly IOutboxWriter _outboxWriter;
    private readonly IDateTimeProvider _dateTime;
    private readonly IConfiguration _configuration;

    public CreateCompanySignupCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IEmailNormalizer emailNormalizer,
        IInvitationTokenService tokenService,
        IOutboxWriter outboxWriter,
        IDateTimeProvider dateTime,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _emailNormalizer = emailNormalizer;
        _tokenService = tokenService;
        _outboxWriter = outboxWriter;
        _dateTime = dateTime;
        _configuration = configuration;
    }

    public async Task<ApiResponse<CompanySignupResultDto>> Handle(
        CreateCompanySignupCommand request,
        CancellationToken cancellationToken)
    {
        var normalizedEmail = _emailNormalizer.Normalize(request.AdminEmail);
        var tenantRepo = _unitOfWork.Repository<FgsTenant>();
        var userRepo = _unitOfWork.Repository<FgsUser>();

        if (await tenantRepo.AnyAsync(t => t.TenantCode == request.TenantCode, cancellationToken))
        {
            return ApiResponse<CompanySignupResultDto>.Fail(
                ["Tenant code is already in use."],
                ApiStatusCodes.Conflict);
        }

        try
        {
            var result = await _unitOfWork.ExecuteInTransactionAsync(
                async ct =>
                {
                    var now = _dateTime.UtcNow;
                    var tenantId = Guid.NewGuid();
                    var companyId = Guid.NewGuid();
                    var userId = Guid.NewGuid();
                    var invitationId = Guid.NewGuid();

                    var tenant = new FgsTenant
                    {
                        Id = tenantId,
                        TenantCode = request.TenantCode.Trim(),
                        Name = request.TenantName.Trim(),
                        Email = request.AdminEmail.Trim(),
                        TimeZone = request.TimeZone,
                        DefaultCurrency = request.DefaultCurrency,
                        IsActive = true,
                        CreatedOn = now
                    };

                    var company = new FgsTenantCompany
                    {
                        CompanyGuid = companyId,
                        TenantId = tenantId,
                        CompanyNumber = 1,
                        BusinessTypeId = 1,
                        Code = request.CompanyCode.Trim(),
                        Name = request.CompanyName.Trim(),
                        Email = request.AdminEmail.Trim(),
                        IsActive = true,
                        CreatedOn = now
                    };

                    var user = new FgsUser
                    {
                        Id = userId,
                        TenantId = tenantId,
                        CompanyId = companyId,
                        Email = request.AdminEmail.Trim(),
                        NormalizedEmail = normalizedEmail,
                        DisplayName = request.AdminDisplayName.Trim(),
                        PasswordHash = _passwordHasher.HashPassword(request.Password),
                        Role = UserRoleType.Admin,
                        IsActive = true,
                        CreatedOn = now
                    };

                    var plainToken = _tokenService.GenerateToken();
                    var tokenHash = _tokenService.HashToken(plainToken);
                    var expiryDays = _configuration.GetValue("Invitation:ExpiryDays", 7);

                    var invitation = new FgsInvitation
                    {
                        Id = invitationId,
                        UserId = userId,
                        TenantId = tenantId,
                        Email = request.AdminEmail.Trim(),
                        TokenHash = tokenHash,
                        Status = InvitationStatus.Pending,
                        ExpiresAtUtc = now.AddDays(expiryDays),
                        CreatedOn = now
                    };

                    await tenantRepo.AddAsync(tenant, ct);
                    await _unitOfWork.Repository<FgsTenantCompany>().AddAsync(company, ct);
                    await userRepo.AddAsync(user, ct);
                    await _unitOfWork.Repository<FgsInvitation>().AddAsync(invitation, ct);

                    var inviteBaseUrl = _configuration["Invitation:InviteBaseUrl"]
                        ?? "https://localhost:5001/api/invite/start";
                    var inviteUrl = $"{inviteBaseUrl.TrimEnd('/')}?token={Uri.EscapeDataString(plainToken)}";

                    var outboxPayload = JsonSerializer.Serialize(new CompanySignupInviteEmailEvent(
                        tenantId,
                        companyId,
                        userId,
                        invitationId,
                        user.Email,
                        user.DisplayName,
                        inviteUrl));

                    await _outboxWriter.EnqueueAsync(
                        IntegrationEventTypes.CompanySignupInviteEmail,
                        outboxPayload,
                        idempotencyKey: $"signup-{invitationId:N}",
                        correlationId: invitationId.ToString(),
                        ct);

                    return new CompanySignupResultDto(
                        tenantId,
                        companyId,
                        userId,
                        invitationId,
                        inviteUrl);
                },
                cancellationToken);

            return ApiResponse<CompanySignupResultDto>.Ok(result, ApiStatusCodes.Created);
        }
        catch (DomainException ex)
        {
            return ApiResponse<CompanySignupResultDto>.Fail([ex.Message], ApiStatusCodes.BadRequest);
        }
    }
}
