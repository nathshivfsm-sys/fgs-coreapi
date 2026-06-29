using System.Text.Json;
using Fgs.User.Application.Abstractions.Geo;
using Fgs.Messaging.Abstractions;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Security;
using Fgs.User.Application.Abstractions.Time;
using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.User.Application.Common;
using Fgs.User.Application.Features.Signup.DTOs;
using Fgs.Contracts.IntegrationEvents;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;

public sealed class CreateCompanySignupCommandHandler
    : IRequestHandler<CreateCompanySignupCommand, ApiResponse<CompanySignupResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISetupClient _setupClient;
    private readonly IInvitationTokenService _tokenService;
    private readonly IOutboxWriter _outboxWriter;
    private readonly IDateTimeProvider _dateTime;
    private readonly IConfiguration _configuration;
    private readonly IAddressLocaleResolver _addressLocaleResolver;
    private readonly ISignupUniquenessValidator _signupUniquenessValidator;
    private readonly ICacheService _cache;

    public CreateCompanySignupCommandHandler(
        IUnitOfWork unitOfWork,
        ISetupClient setupClient,
        IInvitationTokenService tokenService,
        IOutboxWriter outboxWriter,
        IDateTimeProvider dateTime,
        IConfiguration configuration,
        IAddressLocaleResolver addressLocaleResolver,
        ISignupUniquenessValidator signupUniquenessValidator,
        ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _setupClient = setupClient;
        _tokenService = tokenService;
        _outboxWriter = outboxWriter;
        _dateTime = dateTime;
        _configuration = configuration;
        _addressLocaleResolver = addressLocaleResolver;
        _signupUniquenessValidator = signupUniquenessValidator;
        _cache = cache;
    }

    public async Task<ApiResponse<CompanySignupResultDto>> Handle(
        CreateCompanySignupCommand request,
        CancellationToken cancellationToken)
    {
        var contact = request.Contact;
        var company = request.Company;
        var tenantRepo = _unitOfWork.Repository<FgsTenant>();
        var userRepo = _unitOfWork.Repository<FgsUser>();
        var selectedBusinessTypeIds = request.BusinessTypeIds
            .Distinct()
            .ToList();

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
                [SignupErrorMessages.UniqueTenantCodeFailed],
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
                    var prospectActor = SignupConstants.ProspectActor;
                    var companyUid = Guid.NewGuid();
                    const long companyNumber = 1;
                    var userId = Guid.NewGuid();
                    var invitationId = Guid.NewGuid();
                    var locationId = Guid.NewGuid();

                    var companyNameTrimmed = company.Name.Trim();
                    var emailTrimmed = contact.Email.Trim();
                    var phoneStored = SignupPhoneNormalizer.ToStorageFormat(contact.PhoneNumber);
                    var companyWebsite = string.IsNullOrWhiteSpace(company.Website) ? null : company.Website.Trim();
                    var companySize = company.CompanySize.Trim();

                    var tenant = new FgsTenant
                    {
                        FgsTenantStatusId = 2,
                        TenantCode = tenantCode,
                        Name = companyNameTrimmed,
                        LegalName = companyNameTrimmed,
                        Email = emailTrimmed,
                        PhoneNumber = phoneStored,
                        Website = companyWebsite,
                        TimeZone = timeZone,
                        DefaultCurrency = defaultCurrency,
                        DefaultLanguageId = SignupConstants.DefaultLanguageId,
                        IsActive = true,
                        CreatedOn = now,
                        CreatedBy = prospectActor
                    };

                    await tenantRepo.AddAsync(tenant, ct);
                    await _unitOfWork.SaveChangesAsync(ct);

                    var tenantId = tenant.Id;

                    var location = SignupLocationFactory.CreateCompanyLocation(
                        locationId,
                        tenantId,
                        companyNumber,
                        SignupConstants.TenantCompanyMasterEntityTypeId,
                        company.Address,
                        now);
                    location.CreatedBy = prospectActor;

                    tenant.PhysicalLocationId = locationId;
                    tenant.BillingLocationId = locationId;

                    var tenantCompany = new FgsTenantCompany
                    {
                        CompanyGuid = companyUid,
                        TenantId = tenantId,
                        CompanyNumber = companyNumber,
                        CompanySize = companySize,
                        Code = tenantCode,
                        Name = companyNameTrimmed,
                        LegalName = companyNameTrimmed,
                        Email = emailTrimmed,
                        PhoneNumber = phoneStored,
                        Website = companyWebsite,
                        TimeZone = timeZone,
                        PhysicalLocationId = locationId,
                        BillingLocationId = locationId,
                        IsActive = true,
                        CreatedOn = now,
                        CreatedBy = prospectActor
                    };

                    var user = new FgsUser
                    {
                        Id = userId,
                        TenantId = tenantId,
                        CompanyId = companyNumber,
                        Email = emailTrimmed,
                        DisplayName = contact.Name.Trim(),
                        IsActive = true,
                        CreatedOn = now,
                        CreatedBy = prospectActor
                    };

                    var userRole = new FgsUserRole
                    {
                        UserId = userId,
                        TenantId = tenantId,
                        CompanyId = companyNumber,
                        GloRoleId = SignupConstants.TenantAdminGloRoleId,
                        CreatedOn = now
                    };

                    var plainToken = _tokenService.GenerateToken();
                    var tokenHash = _tokenService.HashToken(plainToken);
                    var expiryDays = _configuration.GetValue(
                        ConfigurationKeys.Invitation.ExpiryDays,
                        SignupConstants.DefaultInvitationExpiryDays);

                    var invitation = new FgsInvitation
                    {
                        Id = invitationId,
                        UserId = userId,
                        TenantId = tenantId,
                        Email = emailTrimmed,
                        TokenHash = tokenHash,
                        Status = InvitationStatus.Pending,
                        ExpiresAtUtc = now.AddDays(expiryDays),
                        CreatedOn = now,
                        CreatedBy = prospectActor
                    };

                    await _unitOfWork.Repository<FgsLocation>().AddAsync(location, ct);
                    await _unitOfWork.Repository<FgsTenantCompany>().AddAsync(tenantCompany, ct);
                    await userRepo.AddAsync(user, ct);
                    await _unitOfWork.Repository<FgsUserRole>().AddAsync(userRole, ct);
                    await _unitOfWork.Repository<FgsInvitation>().AddAsync(invitation, ct);

                    var inviteBaseUrl = _configuration[ConfigurationKeys.Invitation.InviteBaseUrl]
                        ?? ApplicationUrlDefaults.InviteStart;
                    var inviteUrl = $"{inviteBaseUrl.TrimEnd('/')}?token={Uri.EscapeDataString(plainToken)}";

                    var expirationHours = Math.Max(
                        SignupConstants.MinimumExpirationHours,
                        (int)Math.Ceiling((invitation.ExpiresAtUtc - now).TotalHours));

                    await _unitOfWork.SaveChangesAsync(ct);

                    // All selected global business types are copied into setup.FgsBusinessType for this company.
                    (await _setupClient.AddCompanyBusinessTypesAsync(
                        tenantId,
                        tenantCompany.CompanyNumber,
                        new AddCompanyBusinessTypesRequest(
                            selectedBusinessTypeIds,
                            companyUid,
                            tenantCode,
                            companyNameTrimmed),
                        ct)).ThrowIfFailed();

                    var outboxPayload = JsonSerializer.Serialize(new CompanySignupInviteEmailEvent(
                        tenantId,
                        tenantCompany.CompanyNumber,
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
                        correlationId: invitationId,
                        tenantId: tenantId,
                        companyId: tenantCompany.CompanyNumber,
                        aggregateType: IntegrationEventTypes.AggregateTypes.Invitation,
                        aggregateId: invitationId.ToString(),
                        exchangeName: IntegrationEventExchanges.UserEvents,
                        routingKey: IntegrationEventRoutingKeys.CompanySignupInviteEmail,
                        createdBy: SignupConstants.ToGloCreatedBy(tenant.CreatedBy),
                        cancellationToken: ct);

                    return new CompanySignupResultDto(
                        tenantId,
                        tenantCompany.CompanyNumber,
                        companyUid,
                        userId,
                        invitationId,
                        inviteUrl);
                },
                cancellationToken);

            await _cache.RemoveAsync(
                CacheKeys.Build(
                    result.TenantId,
                    TenantScopeConstants.PlatformCompanyId,
                    "tenant-companies",
                    "list"),
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

        for (var attempt = 0; attempt < SignupConstants.TenantCodeSuffixAttempts; attempt++)
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
