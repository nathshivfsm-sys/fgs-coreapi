using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.Locations;
using Fgs.User.Application.Features.Signup;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Application.Features.Tenants.Queries.GetTenantCompanyDetails;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Commands.UpdateTenantCompanyDetails;

public sealed class UpdateTenantCompanyDetailsCommandHandler(
    IUserWriteRepository<FgsTenant> tenantWriteRepository,
    IUserWriteRepository<FgsTenantCompany> companyWriteRepository,
    IUserWriteRepository<FgsLocation> locationWriteRepository,
    IUnitOfWork unitOfWork,
    IMediator mediator,
    ICacheService cache)
    : IRequestHandler<UpdateTenantCompanyDetailsCommand, ApiResponse<TenantCompanyDetailDto>>
{
    public async Task<ApiResponse<TenantCompanyDetailDto>> Handle(
        UpdateTenantCompanyDetailsCommand request,
        CancellationToken cancellationToken)
    {
        var tenantRequest = request.Request.Tenant;
        var companyRequest = request.Request.Company;
        var now = DateTimeOffset.UtcNow;

        ApiResponse<TenantCompanyDetailDto>? failure = null;

        await unitOfWork.ExecuteInTransactionAsync(
            async ct =>
            {
                var tenant = await tenantWriteRepository.GetByIdAsync(request.TenantId, ct);

                if (tenant is null)
                {
                    failure = ApiResponse<TenantCompanyDetailDto>.Fail(["Tenant not found."], ApiStatusCodes.NotFound);
                    return;
                }

                var company = await companyWriteRepository.FirstOrDefaultAsync(
                    c => c.TenantId == request.TenantId && c.CompanyNumber == request.CompanyId,
                    ct);

                if (company is null)
                {
                    failure = ApiResponse<TenantCompanyDetailDto>.Fail(["Company not found."], ApiStatusCodes.NotFound);
                    return;
                }

                ApplyTenant(tenant, tenantRequest, now);
                ApplyCompany(company, companyRequest, now);

                await UpdateLocationsAsync(company, companyRequest, now, ct);

                tenantWriteRepository.Update(tenant);
                companyWriteRepository.Update(company);
                await unitOfWork.SaveChangesAsync(ct);
            },
            cancellationToken);

        if (failure is not null)
        {
            return failure;
        }

        await cache.RemoveByPrefixAsync(
            CacheKeys.EntityPrefix(request.TenantId, request.CompanyId, "tenant-company"),
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
                TenantScopeConstants.PlatformCompanyId,
                "tenant",
                request.TenantId.ToString()),
            cancellationToken);

        return await mediator.Send(
            new GetTenantCompanyDetailsQuery(request.TenantId, request.CompanyId),
            cancellationToken);
    }

    private static void ApplyTenant(FgsTenant tenant, UpdateTenantSectionRequest request, DateTimeOffset now)
    {
        tenant.Name = request.Name.Trim();
        tenant.LegalName = TrimOrNull(request.LegalName);
        tenant.Email = TrimOrNull(request.Email);
        tenant.PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber)
            ? null
            : SignupPhoneNormalizer.ToStorageFormat(request.PhoneNumber);
        tenant.Website = TrimOrNull(request.Website);
        tenant.DefaultCurrency = TrimOrNull(request.DefaultCurrency);
        tenant.DefaultLanguageId = request.DefaultLanguageId;
        tenant.UpdatedOn = now;
    }

    private static void ApplyCompany(FgsTenantCompany company, UpdateCompanySectionRequest request, DateTimeOffset now)
    {
        company.Name = request.Name.Trim();
        company.LegalName = TrimOrNull(request.LegalName);
        company.Email = TrimOrNull(request.Email);
        company.PhoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber)
            ? null
            : SignupPhoneNormalizer.ToStorageFormat(request.PhoneNumber);
        company.Website = TrimOrNull(request.Website);
        company.TaxId = TrimOrNull(request.TaxId);
        company.CompanySize = TrimOrNull(request.CompanySize);
        company.TimeZone = TrimOrNull(request.TimeZone);
        company.UpdatedOn = now;
    }

    private async Task UpdateLocationsAsync(
        FgsTenantCompany company,
        UpdateCompanySectionRequest request,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        if (request.PhysicalAddress is null)
        {
            return;
        }

        FgsLocation physicalLocation;

        if (company.PhysicalLocationId.HasValue)
        {
            physicalLocation = await locationWriteRepository.GetByIdAsync(
                                   company.PhysicalLocationId.Value,
                                   cancellationToken)
                               ?? throw new InvalidOperationException("Physical location not found.");
            LocationMapper.ApplyWriteDto(physicalLocation, request.PhysicalAddress, now);
            locationWriteRepository.Update(physicalLocation);
        }
        else
        {
            physicalLocation = new FgsLocation
            {
                Id = Guid.NewGuid(),
                TenantId = company.TenantId,
                CompanyId = company.CompanyNumber,
                MasterEntityTypeId = SignupConstants.TenantCompanyMasterEntityTypeId,
                IsActive = true,
                CreatedOn = now
            };
            LocationMapper.ApplyWriteDto(physicalLocation, request.PhysicalAddress, now);
            await locationWriteRepository.AddAsync(physicalLocation, cancellationToken);
            company.PhysicalLocationId = physicalLocation.Id;
        }

        if (request.BillingAddress is null)
        {
            company.BillingLocationId = physicalLocation.Id;
            return;
        }

        if (company.BillingLocationId.HasValue && company.BillingLocationId != company.PhysicalLocationId)
        {
            var billingLocation = await locationWriteRepository.GetByIdAsync(
                                      company.BillingLocationId.Value,
                                      cancellationToken)
                                  ?? throw new InvalidOperationException("Billing location not found.");
            LocationMapper.ApplyWriteDto(billingLocation, request.BillingAddress, now);
            locationWriteRepository.Update(billingLocation);
            return;
        }

        if (company.BillingLocationId == company.PhysicalLocationId)
        {
            var billingLocation = new FgsLocation
            {
                Id = Guid.NewGuid(),
                TenantId = company.TenantId,
                CompanyId = company.CompanyNumber,
                MasterEntityTypeId = SignupConstants.TenantCompanyMasterEntityTypeId,
                IsActive = true,
                CreatedOn = now
            };
            LocationMapper.ApplyWriteDto(billingLocation, request.BillingAddress, now);
            await locationWriteRepository.AddAsync(billingLocation, cancellationToken);
            company.BillingLocationId = billingLocation.Id;
        }
    }

    private static string? TrimOrNull(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
