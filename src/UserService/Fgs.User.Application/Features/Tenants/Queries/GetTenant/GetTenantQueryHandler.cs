using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.GetTenant;

public sealed class GetTenantQueryHandler(
    IUserReadRepository<FgsTenant> tenantReadRepository,
    ICacheService cache)
    : IRequestHandler<GetTenantQuery, ApiResponse<TenantDto>>
{
    public async Task<ApiResponse<TenantDto>> Handle(GetTenantQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = CacheKeys.Build(
            request.TenantId,
            TenantScopeConstants.PlatformCompanyId,
            "tenant",
            request.TenantId.ToString());

        var cached = await cache.GetAsync<TenantDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<TenantDto>.Ok(cached);
        }

        var tenant = await tenantReadRepository.GetByIdAsync(request.TenantId, cancellationToken);

        if (tenant is null)
        {
            return ApiResponse<TenantDto>.Fail(["Tenant not found."], ApiStatusCodes.NotFound);
        }

        var dto = new TenantDto(
            tenant.Id,
            tenant.TenantCode,
            tenant.Name,
            tenant.FgsTenantStatusId,
            tenant.StorageBucketName);

        await cache.SetAsync(cacheKey, dto, cancellationToken: cancellationToken);
        return ApiResponse<TenantDto>.Ok(dto);
    }
}
