using Fgs.Contracts.Api;
using Fgs.Contracts.Clients;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.MultiTenancy.Constants;
using Fgs.Security.Abstractions;
using Fgs.Security.Authorization;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Tenants.Queries.ListTenantCompanies;

public sealed class ListTenantCompaniesQueryHandler(
    IUserReadRepository<FgsTenantCompany> companyReadRepository,
    ICacheService cache,
    IFgsUserContext userContext)
    : IRequestHandler<ListTenantCompaniesQuery, ApiResponse<IReadOnlyList<TenantCompanyDto>>>
{
    public async Task<ApiResponse<IReadOnlyList<TenantCompanyDto>>> Handle(
        ListTenantCompaniesQuery request,
        CancellationToken cancellationToken)
    {
        var denied = AuthenticatedUserTenantScopeGuard.DenyCrossTenantAccess<IReadOnlyList<TenantCompanyDto>>(
            userContext,
            request.TenantId);
        if (denied is not null)
        {
            return denied;
        }

        var cacheKey = CacheKeys.Build(
            request.TenantId,
            TenantScopeConstants.PlatformCompanyId,
            "tenant-companies",
            "list");

        var cached = await cache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                var companies = await companyReadRepository.ListAsync(
                    "\"TenantId\" = @tenantId",
                    new { tenantId = request.TenantId },
                    cancellationToken);

                return companies
                    .Select(c => new TenantCompanyDto(
                        c.Id,
                        c.TenantId,
                        c.CompanyNumber,
                        c.CompanyGuid,
                        c.Code,
                        c.Name,
                        c.IsActive))
                    .ToList() as IReadOnlyList<TenantCompanyDto>;
            },
            cancellationToken: cancellationToken);

        return ApiResponse<IReadOnlyList<TenantCompanyDto>>.Ok(cached ?? []);
    }
}
