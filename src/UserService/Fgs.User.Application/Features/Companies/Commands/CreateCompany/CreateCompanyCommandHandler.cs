using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.Persistence.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Authorization;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.Locations;
using Fgs.User.Application.Features.Companies.Dtos;
using Fgs.User.Application.Features.Companies.Queries.GetCompany;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Domain.Entities;
using Fgs.User.Domain.Enums;
using MediatR;

namespace Fgs.User.Application.Features.Companies.Commands.CreateCompany;

public sealed class CreateCompanyCommandHandler(
    IUserWriteRepository<FgsTenant> tenantWriteRepository,
    IUserWriteRepository<FgsTenantCompany> companyWriteRepository,
    IUserWriteRepository<FgsTenantCompanyCache> companyCacheWriteRepository,
    IUserWriteRepository<FgsLocation> locationWriteRepository,
    IUserWriteRepository<FgsTenantServiceSetup> serviceSetupWriteRepository,
    IUserWriteRepository<FgsTenantServiceAccountsSetup> serviceAccountsSetupWriteRepository,
    IUserReadRepository<FgsTenantCompany> companyReadRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    ICacheService cache,
    IFgsUserContext userContext)
    : IRequestHandler<CreateCompanyCommand, ApiResponse<CompanyDetailDto>>
{
    public async Task<ApiResponse<CompanyDetailDto>> Handle(
        CreateCompanyCommand request,
        CancellationToken cancellationToken)
    {
        var denied = AuthenticatedUserTenantScopeGuard.DenyCrossTenantAccess<CompanyDetailDto>(
            userContext,
            request.TenantId);
        if (denied is not null)
        {
            return denied;
        }

        var tenant = await tenantWriteRepository.GetByIdAsync(request.TenantId, cancellationToken);
        if (tenant is null)
        {
            return ApiResponse<CompanyDetailDto>.Fail(["Tenant not found."], ApiStatusCodes.NotFound);
        }

        var code = request.Dto.Code.Trim();
        var codeExists = await companyReadRepository.AnyAsync(
            "\"TenantId\" = @tenantId AND LOWER(\"Code\") = LOWER(@code)",
            new { tenantId = request.TenantId, code },
            cancellationToken);
        if (codeExists)
        {
            return ApiResponse<CompanyDetailDto>.Fail(
                [$"Company code '{code}' already exists for this tenant."],
                ApiStatusCodes.Conflict);
        }

        var maxCompanyNumber = await companyReadRepository.QueryFirstAsync<long>(
            """
            SELECT COALESCE(MAX("CompanyNumber"), 0)
            FROM tenant."FgsTenantCompany"
            WHERE "TenantId" = @tenantId
            """,
            new { tenantId = request.TenantId },
            cancellationToken);
        var companyNumber = maxCompanyNumber + 1;
        var now = DateTimeOffset.UtcNow;
        var actor = userContext.UserId?.ToString() ?? userContext.DisplayName ?? "system";
        var companyGuid = Guid.NewGuid();
        var name = request.Dto.Name.Trim();

        await unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                Guid? physicalLocationId = null;
                Guid? billingLocationId = null;

                if (request.Dto.PhysicalAddress is not null)
                {
                    var physicalLocation = CreateLocation(
                        request.TenantId,
                        companyNumber,
                        request.Dto.PhysicalAddress,
                        now,
                        actor);
                    await locationWriteRepository.AddAsync(physicalLocation, ct);
                    physicalLocationId = physicalLocation.Id;

                    if (request.Dto.BillingAddress is null)
                    {
                        billingLocationId = physicalLocationId;
                    }
                    else
                    {
                        var billingLocation = CreateLocation(
                            request.TenantId,
                            companyNumber,
                            request.Dto.BillingAddress,
                            now,
                            actor);
                        await locationWriteRepository.AddAsync(billingLocation, ct);
                        billingLocationId = billingLocation.Id;
                    }
                }

                var company = new FgsTenantCompany
                {
                    CompanyGuid = companyGuid,
                    TenantId = request.TenantId,
                    CompanyNumber = companyNumber,
                    Code = code,
                    Name = name,
                    LegalName = TrimOrNull(request.Dto.LegalName) ?? name,
                    Email = TrimOrNull(request.Dto.Email),
                    PhoneNumber = string.IsNullOrWhiteSpace(request.Dto.PhoneNumber)
                        ? null
                        : SignupPhoneNormalizer.ToStorageFormat(request.Dto.PhoneNumber),
                    Website = TrimOrNull(request.Dto.Website),
                    TaxId = TrimOrNull(request.Dto.TaxId),
                    CompanySize = TrimOrNull(request.Dto.CompanySize),
                    TimeZone = TrimOrNull(request.Dto.TimeZone),
                    PhysicalLocationId = physicalLocationId,
                    BillingLocationId = billingLocationId,
                    IsActive = true,
                    CreatedOn = now,
                    CreatedBy = actor
                };

                var companyCache = new FgsTenantCompanyCache
                {
                    TenantId = request.TenantId,
                    CompanyId = companyNumber,
                    CompanyGuid = companyGuid,
                    CompanyCode = code,
                    CompanyName = name,
                    IsActive = true,
                    UpdatedOn = now
                };

                var serviceSetup = new FgsTenantServiceSetup
                {
                    TenantId = request.TenantId,
                    CompanyId = companyNumber,
                    TimeCardOptionId = TimeCardOption.None,
                    BillHoursFromDispatchOrArrive = "ARRIVE",
                    BillToStartNumber = 100,
                    POStartNumber = 100,
                    QuoteStartNumber = 100,
                    WorkOrderStartNumber = 100,
                    IsActive = true,
                    CreatedOn = now,
                    CreatedBy = actor
                };

                var serviceAccountsSetup = new FgsTenantServiceAccountsSetup
                {
                    TenantId = request.TenantId,
                    CompanyId = companyNumber,
                    IsActive = true,
                    CreatedOn = now,
                    CreatedBy = actor
                };

                await companyWriteRepository.AddAsync(company, ct);
                await companyCacheWriteRepository.AddAsync(companyCache, ct);
                await serviceSetupWriteRepository.AddAsync(serviceSetup, ct);
                await serviceAccountsSetupWriteRepository.AddAsync(serviceAccountsSetup, ct);
                await unitOfWork.SaveChangesAsync(ct);
            },
            cancellationToken);

        await cache.RemoveAsync(
            CacheKeys.Build(
                request.TenantId,
                TenantScopeConstants.PlatformCompanyId,
                "tenant-companies",
                "list"),
            cancellationToken);
        await cache.RemoveAsync(
            CacheKeys.Build(
                request.TenantId,
                companyNumber,
                "company-aggregate",
                companyNumber.ToString()),
            cancellationToken);

        var created = await mediator.Send(
            new GetCompanyQuery(request.TenantId, companyNumber),
            cancellationToken);

        if (!created.Success || created.Data is null)
        {
            return created;
        }

        return ApiResponse<CompanyDetailDto>.Ok(created.Data, ApiStatusCodes.Created);
    }

    private static FgsLocation CreateLocation(
        long tenantId,
        long companyNumber,
        LocationWriteDto address,
        DateTimeOffset now,
        string actor)
    {
        var location = new FgsLocation
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            CompanyId = companyNumber,
            MasterEntityTypeId = SignupConstants.TenantCompanyMasterEntityTypeId,
            IsActive = true,
            CreatedOn = now,
            CreatedBy = actor
        };
        LocationMapper.ApplyWriteDto(location, address, now);
        return location;
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
