using System.Text.Json;
using Fgs.User.Application.Abstractions.Geo;
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
    private const int TenantCompanyMasterEntityTypeId = 2;

    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IEmailNormalizer _emailNormalizer;
    private readonly IInvitationTokenService _tokenService;
    private readonly IOutboxWriter _outboxWriter;
    private readonly IDateTimeProvider _dateTime;
    private readonly IConfiguration _configuration;
    private readonly IAddressLocaleResolver _addressLocaleResolver;
    private readonly ISignupUniquenessValidator _signupUniquenessValidator;

    public CreateCompanySignupCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IEmailNormalizer emailNormalizer,
        IInvitationTokenService tokenService,
        IOutboxWriter outboxWriter,
        IDateTimeProvider dateTime,
        IConfiguration configuration,
        IAddressLocaleResolver addressLocaleResolver,
        ISignupUniquenessValidator signupUniquenessValidator)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _emailNormalizer = emailNormalizer;
        _tokenService = tokenService;
        _outboxWriter = outboxWriter;
        _dateTime = dateTime;
        _configuration = configuration;
        _addressLocaleResolver = addressLocaleResolver;
        _signupUniquenessValidator = signupUniquenessValidator;
    }

    public async Task<ApiResponse<CompanySignupResultDto>> Handle(
        CreateCompanySignupCommand request,
        CancellationToken cancellationToken)
    {
        var contact = request.Contact;
        var company = request.Company;
        var normalizedEmail = _emailNormalizer.Normalize(contact.Email);
        var tenantRepo = _unitOfWork.Repository<FgsTenant>();
        var userRepo = _unitOfWork.Repository<FgsUser>();
        var businessTypeRepo = _unitOfWork.Repository<GloBusinessType>();

        if (!await businessTypeRepo.AnyAsync(b => b.Id == request.BusinessTypeId && b.IsActive, cancellationToken))
        {
            return ApiResponse<CompanySignupResultDto>.Fail(
                ["The selected industry is not valid."],
                ApiStatusCodes.BadRequest);
        }

        var uniquenessErrors = await _signupUniquenessValidator.ValidateAsync(request, cancellationToken);
        if (uniquenessErrors.Count > 0)
        {
            return ApiResponse<CompanySignupResultDto>.Fail(
                uniquenessErrors,
                ApiStatusCodes.Conflict);
        }

        var tenantCode = await ResolveUniqueTenantCodeAsync(company.Name, tenantRepo, cancellationToken);
        if (tenantCode is null)
        {
            return ApiResponse<CompanySignupResultDto>.Fail(
                ["Unable to generate a unique tenant code. Please try a different company name."],
                ApiStatusCodes.Conflict);
        }

        var locale = await _addressLocaleResolver.ResolveAsync(company.Address, cancellationToken);
        var timeZone = string.IsNullOrWhiteSpace(request.TimeZone)
            ? locale.TimeZoneId
            : request.TimeZone.Trim();
        var defaultCurrency = string.IsNullOrWhiteSpace(request.DefaultCurrency)
            ? locale.CurrencyCode
            : request.DefaultCurrency.Trim();

        try
        {
            var result = await _unitOfWork.ExecuteInTransactionAsync(
                async ct =>
                {
                    var now = _dateTime.UtcNow;
                    var tenantId = Guid.NewGuid();
                    var companyUid = Guid.NewGuid();
                    var userId = Guid.NewGuid();
                    var invitationId = Guid.NewGuid();
                    var locationId = Guid.NewGuid();

                    var companyNameTrimmed = company.Name.Trim();
                    var emailTrimmed = contact.Email.Trim();
                    var phoneTrimmed = contact.PhoneNumber.Trim();
                    var companyWebsite = string.IsNullOrWhiteSpace(company.Website) ? null : company.Website.Trim();

                    var location = SignupLocationFactory.CreateCompanyLocation(
                        locationId,
                        tenantId,
                        companyUid,
                        TenantCompanyMasterEntityTypeId,
                        company.Address,
                        now);

                    var tenant = new FgsTenant
                    {
                        Id = tenantId,
                        TenantCode = tenantCode,
                        Name = companyNameTrimmed,
                        Email = emailTrimmed,
                        PhoneNumber = phoneTrimmed,
                        Website = companyWebsite,
                        PhysicalLocationId = locationId,
                        TimeZone = timeZone,
                        DefaultCurrency = defaultCurrency,
                        IsActive = true,
                        CreatedOn = now
                    };

                    var tenantCompany = new FgsTenantCompany
                    {
                        CompanyGuid = companyUid,
                        TenantId = tenantId,
                        CompanyNumber = 1,
                        BusinessTypeId = request.BusinessTypeId,
                        CompanySize = company.CompanySize,
                        Code = tenantCode,
                        Name = companyNameTrimmed,
                        Email = emailTrimmed,
                        PhoneNumber = phoneTrimmed,
                        Website = companyWebsite,
                        PhysicalLocationId = locationId,
                        IsActive = true,
                        CreatedOn = now
                    };

                    var passwordHash = string.IsNullOrWhiteSpace(request.Password)
                        ? null
                        : _passwordHasher.HashPassword(request.Password.Trim());

                    var user = new FgsUser
                    {
                        Id = userId,
                        TenantId = tenantId,
                        CompanyId = companyUid,
                        Email = emailTrimmed,
                        NormalizedEmail = normalizedEmail,
                        DisplayName = contact.Name.Trim(),
                        PasswordHash = passwordHash,
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
                        Email = emailTrimmed,
                        TokenHash = tokenHash,
                        Status = InvitationStatus.Pending,
                        ExpiresAtUtc = now.AddDays(expiryDays),
                        CreatedOn = now
                    };

                    await tenantRepo.AddAsync(tenant, ct);
                    await _unitOfWork.Repository<FgsLocation>().AddAsync(location, ct);
                    await _unitOfWork.Repository<FgsTenantCompany>().AddAsync(tenantCompany, ct);
                    await userRepo.AddAsync(user, ct);
                    await _unitOfWork.Repository<FgsInvitation>().AddAsync(invitation, ct);

                    var inviteBaseUrl = _configuration["Invitation:InviteBaseUrl"]
                        ?? "https://localhost:5001/api/invite/start";
                    var inviteUrl = $"{inviteBaseUrl.TrimEnd('/')}?token={Uri.EscapeDataString(plainToken)}";

                    var expirationHours = Math.Max(
                        1,
                        (int)Math.Ceiling((invitation.ExpiresAtUtc - now).TotalHours));

                    await _unitOfWork.SaveChangesAsync(ct);

                    var outboxPayload = JsonSerializer.Serialize(new CompanySignupInviteEmailEvent(
                        tenantId,
                        companyUid,
                        userId,
                        invitationId,
                        user.Email,
                        CommunicationTemplateCodes.CompanyAdminInvitation,
                        user.DisplayName,
                        PlatformName: string.Empty,
                        inviteUrl,
                        expirationHours.ToString(),
                        SupportEmail: string.Empty));

                    await _outboxWriter.EnqueueAsync(
                        IntegrationEventTypes.CompanySignupInviteEmail,
                        outboxPayload,
                        idempotencyKey: $"signup-{invitationId:N}",
                        correlationId: invitationId.ToString(),
                        ct);

                    return new CompanySignupResultDto(
                        tenantId,
                        tenantCompany.Id,
                        companyUid,
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

    private static async Task<string?> ResolveUniqueTenantCodeAsync(
        string companyName,
        IRepository<FgsTenant> tenantRepo,
        CancellationToken cancellationToken)
    {
        var baseCode = TenantCodeGenerator.FromCompanyName(companyName);
        if (!await tenantRepo.AnyAsync(t => t.TenantCode == baseCode, cancellationToken))
        {
            return baseCode;
        }

        for (var attempt = 0; attempt < 5; attempt++)
        {
            var candidate = TenantCodeGenerator.WithSuffix(baseCode, Guid.NewGuid().ToString("N")[..6]);
            if (!await tenantRepo.AnyAsync(t => t.TenantCode == candidate, cancellationToken))
            {
                return candidate;
            }
        }

        return null;
    }
}
