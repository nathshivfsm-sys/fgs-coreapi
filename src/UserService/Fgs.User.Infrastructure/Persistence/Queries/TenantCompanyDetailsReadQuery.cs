using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.Locations;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Domain.Entities;

namespace Fgs.User.Infrastructure.Persistence.Queries;

internal sealed class TenantCompanyDetailsReadQuery(
    IUserReadRepository<FgsTenant> tenantReadRepository,
    IUserReadRepository<FgsTenantCompany> companyReadRepository,
    IUserReadRepository<FgsLocation> locationReadRepository) : ITenantCompanyDetailsReadQuery
{
    public async Task<TenantCompanyDetailDto?> GetAsync(
        long tenantId,
        long companyNumber,
        CancellationToken cancellationToken = default)
    {
        var tenant = await tenantReadRepository.FirstOrDefaultAsync(
            "\"Id\" = @tenantId",
            new { tenantId },
            cancellationToken);

        if (tenant is null)
        {
            return null;
        }

        var company = await companyReadRepository.FirstOrDefaultAsync(
            "\"TenantId\" = @tenantId AND \"CompanyNumber\" = @companyNumber",
            new { tenantId, companyNumber },
            cancellationToken);

        if (company is null)
        {
            return null;
        }

        FgsLocation? physicalLocation = null;
        if (company.PhysicalLocationId.HasValue)
        {
            physicalLocation = await locationReadRepository.GetByIdAsync(
                company.PhysicalLocationId.Value,
                cancellationToken);
        }

        FgsLocation? billingLocation = null;
        if (company.BillingLocationId.HasValue
            && company.BillingLocationId != company.PhysicalLocationId)
        {
            billingLocation = await locationReadRepository.GetByIdAsync(
                company.BillingLocationId.Value,
                cancellationToken);
        }
        else if (company.BillingLocationId == company.PhysicalLocationId)
        {
            billingLocation = physicalLocation;
        }

        return new TenantCompanyDetailDto(
            new TenantDetailSectionDto(
                tenant.Id,
                tenant.TenantGuid,
                tenant.TenantCode,
                tenant.Name,
                tenant.LegalName,
                tenant.Email,
                tenant.PhoneNumber,
                tenant.Website,
                tenant.TimeZone,
                tenant.DefaultCurrency,
                tenant.DefaultLanguageId,
                tenant.FgsTenantStatusId,
                tenant.IsActive),
            new CompanyDetailSectionDto(
                company.Id,
                company.CompanyNumber,
                company.CompanyGuid,
                company.Code,
                company.Name,
                company.LegalName,
                company.Email,
                company.PhoneNumber,
                company.Website,
                company.TaxId,
                company.CompanySize,
                company.BusinessTypeId,
                company.IsActive,
                LocationMapper.ToDetailDto(physicalLocation),
                LocationMapper.ToDetailDto(billingLocation)));
    }
}
