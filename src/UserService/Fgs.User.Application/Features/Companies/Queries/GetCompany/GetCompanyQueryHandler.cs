using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Authorization;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Features.Companies.Dtos;
using MediatR;

namespace Fgs.User.Application.Features.Companies.Queries.GetCompany;

public sealed class GetCompanyQueryHandler(
    ICompanyDetailsReadQuery detailsReadQuery,
    ICacheService cache,
    IFgsUserContext userContext)
    : IRequestHandler<GetCompanyQuery, ApiResponse<CompanyDetailDto>>
{
    public async Task<ApiResponse<CompanyDetailDto>> Handle(
        GetCompanyQuery request,
        CancellationToken cancellationToken)
    {
        var denied = AuthenticatedUserTenantScopeGuard.DenyCrossTenantCompanyAccess<CompanyDetailDto>(
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

        var cached = await cache.GetAsync<CompanyDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<CompanyDetailDto>.Ok(cached);
        }

        var result = await detailsReadQuery.GetAsync(request.TenantId, request.CompanyId, cancellationToken);
        if (result is null)
        {
            return ApiResponse<CompanyDetailDto>.Fail(["Company not found."], ApiStatusCodes.NotFound);
        }

        await cache.SetAsync(cacheKey, result, cancellationToken: cancellationToken);
        return ApiResponse<CompanyDetailDto>.Ok(result);
    }
}
