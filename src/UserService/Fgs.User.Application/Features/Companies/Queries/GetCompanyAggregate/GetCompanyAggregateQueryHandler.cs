using Fgs.Contracts.Api;
using Fgs.Foundation.Caching;
using Fgs.Foundation.Caching.Abstractions;
using Fgs.Security.Abstractions;
using Fgs.Security.Authorization;
using Fgs.User.Application.Abstractions.Persistence;
using Fgs.User.Application.Abstractions.ServiceAccountsSetups;
using Fgs.User.Application.Abstractions.ServiceSetups;
using Fgs.User.Application.Features.Companies.Dtos;
using Fgs.User.Application.Features.Tenants.Commands.UpdateTenant;
using Fgs.User.Domain.Entities;
using MediatR;

namespace Fgs.User.Application.Features.Companies.Queries.GetCompanyAggregate;

public sealed class GetCompanyAggregateQueryHandler(
    IUserReadRepository<FgsTenant> tenantReadRepository,
    ICompanyDetailsReadQuery companyDetailsReadQuery,
    IFgsTenantServiceSetupReadRepository serviceSetupReadRepository,
    IFgsTenantServiceAccountsSetupReadRepository serviceAccountsSetupReadRepository,
    ICacheService cache,
    IFgsUserContext userContext)
    : IRequestHandler<GetCompanyAggregateQuery, ApiResponse<CompanyAggregateDto>>
{
    public async Task<ApiResponse<CompanyAggregateDto>> Handle(
        GetCompanyAggregateQuery request,
        CancellationToken cancellationToken)
    {
        var denied = AuthenticatedUserTenantScopeGuard.DenyCrossTenantCompanyAccess<CompanyAggregateDto>(
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
            "company-aggregate",
            request.CompanyId.ToString());

        var cached = await cache.GetAsync<CompanyAggregateDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            return ApiResponse<CompanyAggregateDto>.Ok(cached);
        }

        var tenant = await tenantReadRepository.GetByIdAsync(request.TenantId, cancellationToken);
        if (tenant is null)
        {
            return ApiResponse<CompanyAggregateDto>.Fail(["Tenant not found."], ApiStatusCodes.NotFound);
        }

        var company = await companyDetailsReadQuery.GetAsync(
            request.TenantId,
            request.CompanyId,
            cancellationToken);
        if (company is null)
        {
            return ApiResponse<CompanyAggregateDto>.Fail(["Company not found."], ApiStatusCodes.NotFound);
        }

        var serviceSetup = await serviceSetupReadRepository.GetByTenantCompanyAsync(
            request.TenantId,
            request.CompanyId,
            cancellationToken);
        var serviceAccountsSetup = await serviceAccountsSetupReadRepository.GetByTenantCompanyAsync(
            request.TenantId,
            request.CompanyId,
            cancellationToken);

        var aggregate = new CompanyAggregateDto(
            UpdateTenantCommandHandler.Map(tenant),
            company,
            serviceSetup,
            serviceAccountsSetup);

        await cache.SetAsync(cacheKey, aggregate, cancellationToken: cancellationToken);
        return ApiResponse<CompanyAggregateDto>.Ok(aggregate);
    }
}
