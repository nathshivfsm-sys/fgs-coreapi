using Fgs.User.Application.Abstractions.Geo;
using Fgs.User.Application.Abstractions.Invitations;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Time;
using Fgs.Contracts.Api;
using Fgs.Contracts.Signup;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using Fgs.User.Domain.Exceptions;
using MediatR;
using ContractAuthenticationMethod = Fgs.Contracts.Signup.AuthenticationMethod;
using DomainAuthenticationMethod = Fgs.User.Domain.Enums.AuthenticationMethod;

namespace Fgs.User.Application.Features.Signup.Commands.CreateCompanySignup;

public sealed class CreateCompanySignupCommandHandler
    : IRequestHandler<CreateCompanySignupCommand, ApiResponse<CompanySignupResultDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserInvitationIssuer _invitationIssuer;
    private readonly IDateTimeProvider _dateTime;
    private readonly IAddressLocaleResolver _addressLocaleResolver;
    private readonly ISignupUniquenessValidator _signupUniquenessValidator;
    private readonly ICacheService _cache;

    public CreateCompanySignupCommandHandler(
        IUnitOfWork unitOfWork,
        IUserInvitationIssuer invitationIssuer,
        IDateTimeProvider dateTime,
        IAddressLocaleResolver addressLocaleResolver,
        ISignupUniquenessValidator signupUniquenessValidator,
        ICacheService cache)
    {
        _unitOfWork = unitOfWork;
        _invitationIssuer = invitationIssuer;
        _dateTime = dateTime;
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

                    var tenantCompanyCache = new FgsTenantCompanyCache
                    {
                        TenantId = tenantId,
                        CompanyId = companyNumber,
                        CompanyGuid = companyUid,
                        CompanyCode = tenantCode,
                        CompanyName = companyNameTrimmed,
                        IsActive = true,
                        UpdatedOn = now
                    };

                    var user = new FgsUser
                    {
                        Id = userId,
                        TenantId = tenantId,
                        CompanyId = companyNumber,
                        Email = emailTrimmed,
                        DisplayName = contact.Name.Trim(),
                        PhoneNumber = phoneStored,
                        AuthenticationMethod = MapAuthenticationMethod(request.AuthenticationMethod),
                        IsActive = true,
                        CreatedOn = now,
                        CreatedBy = prospectActor
                    };

                    var tenantAdminRole = new FgsRole
                    {
                        TenantId = tenantId,
                        CompanyId = companyNumber,
                        RoleCode = SignupConstants.TenantAdminRoleCode,
                        Name = SignupConstants.TenantAdminRoleName,
                        IsBuiltIn = true,
                        DisplayOrder = 1,
                        IsActive = true,
                        CreatedOn = now,
                        CreatedBy = prospectActor
                    };

                    var serviceSetup = new FgsTenantServiceSetup
                    {
                        TenantId = tenantId,
                        CompanyId = companyNumber,
                        TimeCardOptionId = TimeCardOption.None,
                        BillHoursFromDispatchOrArrive = "ARRIVE",
                        BillToStartNumber = 100,
                        POStartNumber = 100,
                        QuoteStartNumber = 100,
                        WorkOrderStartNumber = 100,
                        IsActive = true,
                        CreatedOn = now,
                        CreatedBy = prospectActor
                    };

                    await _unitOfWork.Repository<FgsLocation>().AddAsync(location, ct);
                    await _unitOfWork.Repository<FgsTenantCompany>().AddAsync(tenantCompany, ct);
                    await _unitOfWork.Repository<FgsTenantCompanyCache>().AddAsync(tenantCompanyCache, ct);
                    await _unitOfWork.Repository<FgsUser>().AddAsync(user, ct);
                    await _unitOfWork.Repository<FgsRole>().AddAsync(tenantAdminRole, ct);
                    await _unitOfWork.Repository<FgsTenantServiceSetup>().AddAsync(serviceSetup, ct);
                    await _unitOfWork.SaveChangesAsync(ct);

                    var userRole = new FgsUserRole
                    {
                        UserId = userId,
                        TenantId = tenantId,
                        CompanyId = companyNumber,
                        FgsRoleId = tenantAdminRole.Id,
                        CreatedOn = now,
                        CreatedBy = prospectActor
                    };
                    await _unitOfWork.Repository<FgsUserRole>().AddAsync(userRole, ct);

                    var issued = await _invitationIssuer.IssueAsync(
                        new IssueInvitationRequest(
                            userId,
                            tenantId,
                            companyNumber,
                            emailTrimmed,
                            user.DisplayName,
                            InvitationEmailKind.CompanyAdminSignup,
                            InvitationId: invitationId,
                            CreatedBy: prospectActor,
                            UtcNow: now),
                        ct);

                    return new CompanySignupResultDto(
                        tenantId,
                        tenantCompany.CompanyNumber,
                        companyUid,
                        userId,
                        issued.InvitationId,
                        issued.InviteUrl,
                        tenantCode);
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

    private static DomainAuthenticationMethod MapAuthenticationMethod(
        ContractAuthenticationMethod? authenticationMethod) =>
        authenticationMethod is null
            ? DomainAuthenticationMethod.PasswordOrEmailOtp
            : (DomainAuthenticationMethod)(short)authenticationMethod.Value;
}
