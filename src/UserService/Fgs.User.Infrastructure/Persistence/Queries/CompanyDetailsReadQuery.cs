using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Common.Locations;
using Fgs.User.Application.Features.Companies.Dtos;
using Fgs.User.Domain.Entities;

namespace Fgs.User.Infrastructure.Persistence.Queries;

internal sealed class CompanyDetailsReadQuery(
    IUserReadRepository<FgsTenantCompany> companyReadRepository,
    IUserReadRepository<FgsLocation> locationReadRepository) : ICompanyDetailsReadQuery
{
    public async Task<CompanyDetailDto?> GetAsync(
        long tenantId,
        long companyNumber,
        CancellationToken cancellationToken = default)
    {
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

        return Map(company, physicalLocation, billingLocation);
    }

    internal static CompanyDetailDto Map(
        FgsTenantCompany company,
        FgsLocation? physicalLocation,
        FgsLocation? billingLocation) =>
        new(
            company.Id,
            company.TenantId,
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
            company.TimeZone,
            company.IsActive,
            LocationMapper.ToDetailDto(physicalLocation),
            LocationMapper.ToDetailDto(billingLocation));
}
