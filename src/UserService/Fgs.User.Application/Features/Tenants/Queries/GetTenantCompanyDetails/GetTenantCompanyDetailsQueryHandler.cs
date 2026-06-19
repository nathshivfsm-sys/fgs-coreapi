using Fgs.Contracts.Api;
using Fgs.Persistence.Abstractions;
using Fgs.User.Application.Common.Locations;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.GetTenantCompanyDetails;

public sealed class GetTenantCompanyDetailsQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<GetTenantCompanyDetailsQuery, ApiResponse<TenantCompanyDetailDto>>
{
    public async Task<ApiResponse<TenantCompanyDetailDto>> Handle(
        GetTenantCompanyDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var tenant = await unitOfWork.Repository<FgsTenant>()
            .FirstOrDefaultAsync(t => t.Id == request.TenantId, cancellationToken);

        if (tenant is null)
        {
            return ApiResponse<TenantCompanyDetailDto>.Fail(["Tenant not found."], ApiStatusCodes.NotFound);
        }

        var company = await unitOfWork.Repository<FgsTenantCompany>()
            .FirstOrDefaultAsync(
                c => c.TenantId == request.TenantId && c.CompanyNumber == request.CompanyId,
                cancellationToken);

        if (company is null)
        {
            return ApiResponse<TenantCompanyDetailDto>.Fail(["Company not found."], ApiStatusCodes.NotFound);
        }

        FgsLocation? physicalLocation = null;
        if (company.PhysicalLocationId.HasValue)
        {
            physicalLocation = await unitOfWork.Repository<FgsLocation>()
                .FirstOrDefaultAsync(l => l.Id == company.PhysicalLocationId.Value, cancellationToken);
        }

        FgsLocation? billingLocation = null;
        if (company.BillingLocationId.HasValue
            && company.BillingLocationId != company.PhysicalLocationId)
        {
            billingLocation = await unitOfWork.Repository<FgsLocation>()
                .FirstOrDefaultAsync(l => l.Id == company.BillingLocationId.Value, cancellationToken);
        }
        else if (company.BillingLocationId == company.PhysicalLocationId)
        {
            billingLocation = physicalLocation;
        }

        return ApiResponse<TenantCompanyDetailDto>.Ok(new TenantCompanyDetailDto(
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
                company.BusinessTypeId,
                company.IsActive,
                LocationMapper.ToDetailDto(physicalLocation),
                LocationMapper.ToDetailDto(billingLocation),
                company.CreatedOn,
                company.CreatedBy,
                company.UpdatedOn,
                company.UpdatedBy)));
    }
}
