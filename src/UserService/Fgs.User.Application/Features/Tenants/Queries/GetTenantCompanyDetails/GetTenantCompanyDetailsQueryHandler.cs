using Fgs.Contracts.Api;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.GetTenantCompanyDetails;

public sealed class GetTenantCompanyDetailsQueryHandler(
    IUserReadRepository<FgsTenant> tenantReadRepository,
    ITenantCompanyDetailsReadQuery detailsReadQuery)
    : IRequestHandler<GetTenantCompanyDetailsQuery, ApiResponse<TenantCompanyDetailDto>>
{
    public async Task<ApiResponse<TenantCompanyDetailDto>> Handle(
        GetTenantCompanyDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var tenant = await tenantReadRepository.GetByIdAsync(request.TenantId, cancellationToken);
        if (tenant is null)
        {
            return ApiResponse<TenantCompanyDetailDto>.Fail(["Tenant not found."], ApiStatusCodes.NotFound);
        }

        var result = await detailsReadQuery.GetAsync(request.TenantId, request.CompanyId, cancellationToken);
        if (result is null)
        {
            return ApiResponse<TenantCompanyDetailDto>.Fail(["Company not found."], ApiStatusCodes.NotFound);
        }

        return ApiResponse<TenantCompanyDetailDto>.Ok(result);
    }
}
