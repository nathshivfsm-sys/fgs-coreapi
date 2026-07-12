using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Authorization;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Tenants.Dtos;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.GetTenantCompanyDetails;

public sealed class GetTenantCompanyDetailsQueryHandler(
    IUserReadRepository<FgsTenant> tenantReadRepository,
    ITenantCompanyDetailsReadQuery detailsReadQuery,
    ICacheService cache,
    IFgsUserContext userContext)
    : IRequestHandler<GetTenantCompanyDetailsQuery, ApiResponse<TenantCompanyDetailDto>>
{
    public async Task<ApiResponse<TenantCompanyDetailDto>> Handle(
        GetTenantCompanyDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var denied = AuthenticatedUserTenantScopeGuard.DenyCrossTenantCompanyAccess<TenantCompanyDetailDto>(
            userContext,
            request.TenantId,
            request.CompanyId);
        if (denied is not null)
        {
            return denied;
        }

        var cacheKey = CacheKeys.Build(
            request.TenantId,
            request.CompanyId,
            "tenant-company",
            request.CompanyId.ToString());

        var cached = await cache.GetAsync<TenantCompanyDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<TenantCompanyDetailDto>.Ok(cached);
        }

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

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<TenantCompanyDetailDto>.Ok(result);
    }
}
