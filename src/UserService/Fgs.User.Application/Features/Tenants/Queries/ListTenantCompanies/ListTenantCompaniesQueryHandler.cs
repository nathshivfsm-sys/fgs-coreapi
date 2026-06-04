using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Persistence.Abstractions;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.ListTenantCompanies;

public sealed class ListTenantCompaniesQueryHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<ListTenantCompaniesQuery, ApiResponse<IReadOnlyList<TenantCompanyDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<TenantCompanyDto>>> Handle(
        ListTenantCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        var companies = await unitOfWork.Repository<FgsTenantCompany>()
            .ListAsync(c => c.TenantId == request.TenantId, cancellationToken);

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
