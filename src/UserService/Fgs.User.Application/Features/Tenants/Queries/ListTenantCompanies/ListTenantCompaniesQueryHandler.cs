using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.ListTenantCompanies;

public sealed class ListTenantCompaniesQueryHandler(IUserReadRepository<FgsTenantCompany> companyReadRepository)
    : IRequestHandler<ListTenantCompaniesQuery, ApiResponse<IReadOnlyList<TenantCompanyDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<TenantCompanyDto>>> Handle(
        ListTenantCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        var companies = await companyReadRepository.ListAsync(
            "\"TenantId\" = @tenantId",
            new { tenantId = request.TenantId },
            cancellationToken);

        var dtos = companies
            .Select(c => new TenantCompanyDto(
                c.Id,
                c.TenantId,
                c.CompanyNumber,
                c.CompanyGuid,
                c.Code,
                c.Name,
                c.IsActive))
            .ToList();

        return ApiResponse<IReadOnlyList<TenantCompanyDto>>.Ok(dtos);
    }
}
